using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.Mayan;
using Pims.Av;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentService implementation provides document managing capabilities.
    /// </summary>
    public class DocumentService : BaseService, IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IDocumentActivityRepository documentActivityRespository;
        private readonly IEdmsDocumentRepository documentStorageRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IAvService avService;
        private readonly IMapper mapper;

        public DocumentService(
            ClaimsPrincipal user,
            ILogger<DocumentService> logger,
            IDocumentRepository documentRepository,
            IDocumentActivityRepository documentActivityRespository,
            IEdmsDocumentRepository documentStorageRepository,
            IDocumentTypeRepository documentTypeRepository,
            IAvService avService,
            IMapper mapper)
            : base(user, logger)
        {
            this.documentRepository = documentRepository;
            this.documentActivityRespository = documentActivityRespository;
            this.documentActivityRespository = documentActivityRespository;
            this.documentStorageRepository = documentStorageRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.avService = avService;
            this.mapper = mapper;
        }

        public IList<PimsDocumentTyp> GetPimsDocumentTypes()
        {
            this.Logger.LogInformation("Retrieving PIMS document types");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            return documentTypeRepository.GetAll();
        }

        public IList<PimsActivityInstanceDocument> GetActivityDocuments(long activityId)
        {
            this.Logger.LogInformation("Retrieving PIMS document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            return documentActivityRespository.GetAllByActivity(activityId);
        }

        public async Task<bool> DeleteActivityDocumentAsync(PimsActivityInstanceDocument activityDocument, bool commitTransaction = true)
        {
            this.Logger.LogInformation("Deleting PIMS document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            IList<PimsActivityInstanceDocument> existingActivityDocuments = documentActivityRespository.GetAllByDocument(activityDocument.DocumentId);
            if (existingActivityDocuments.Count == 1)
            {
                documentActivityRespository.Delete(activityDocument);
                var deletedDocumentFlag = await DeleteDocumentAsync(activityDocument.Document);
                if(commitTransaction)
                {
                    documentActivityRespository.CommitTransaction();
                }
                return deletedDocumentFlag;
            }
            else
            {
                documentActivityRespository.Delete(activityDocument);
                if (commitTransaction)
                {
                    documentActivityRespository.CommitTransaction();
                }
                return true;
            }
        }

        public async Task<DocumentUploadResponse> UploadActivityDocumentAsync(long activityId, DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            ExternalResult<DocumentDetail> externalResult = await UploadDocumentAsync(uploadRequest.DocumentTypeMayanId, uploadRequest.File);
            DocumentUploadResponse response = new DocumentUploadResponse() { ExternalResult = externalResult };
            if (externalResult.Status == ExternalResultStatus.Success)
            {
                var externalDocument = externalResult.Payload;

                // Save metadata of document
                IList<Task<ExternalResult<DocumentMetadata>>> metadataTasks = new List<Task<ExternalResult<DocumentMetadata>>>();
                foreach (var metadata in uploadRequest?.DocumentMetadata ?? new List<DocumentMetadataUpdateModel>())
                {
                    metadataTasks.Add(documentStorageRepository.CreateDocumentMetadataAsync(externalDocument.Id, metadata.MetadataTypeId, metadata.Value));
                }
                await Task.WhenAll(metadataTasks.ToArray());
                // Create the pims document
                PimsDocument newPimsDocument = new PimsDocument()
                {
                    FileName = externalDocument.Label,
                    DocumentTypeId = uploadRequest.DocumentTypeId,
                    DocumentStatusTypeCode = uploadRequest.DocumentStatusCode,
                    MayanId = externalDocument.Id,
                };

                // Create the pims document activity relationship
                PimsActivityInstanceDocument newActivityDocument = new PimsActivityInstanceDocument()
                {
                    ActivityInstanceId = activityId,
                    Document = newPimsDocument,
                };
                newActivityDocument = documentActivityRespository.Add(newActivityDocument);
                documentActivityRespository.CommitTransaction();

                response.DocumentRelationship = mapper.Map<DocumentRelationshipModel>(newActivityDocument);
            }
            return response;
        }

        public async Task<bool> UpdateActivityDocumentMetadataAsync(long documentId, DocumentUpdateMetadataRequest updateRequest)
        {
            this.Logger.LogInformation("Updating document for single activity");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentEdit);

            // update metadata of document
            IList<Task<ExternalResult<DocumentMetadata>>> metadataTasks = new List<Task<ExternalResult<DocumentMetadata>>>();
            foreach (var metadata in updateRequest.DocumentMetadata ?? new List<DocumentMetadataUpdateModel>())
            {
                metadataTasks.Add(documentStorageRepository.UpdateDocumentMetadataAsync(updateRequest.MayanDocumentId, metadata.Id, metadata.Value));
            }
            if (metadataTasks.Count > 0) { await Task.WhenAll(metadataTasks.ToArray()); }

            // update the pims document status
            PimsDocument existingDocument = documentRepository.Get(documentId);
            if (existingDocument is not null)
            {
                existingDocument.DocumentStatusTypeCode = updateRequest.DocumentStatusCode;
                documentRepository.Update(existingDocument);
                this.Logger.LogInformation("Metadata & Status for Document with id {id} update successfully", documentId);
                return true;
            }
            throw new BadRequestException("Document Id not found.");
        }

        public async Task<bool> DeleteDocumentAsync(PimsDocument document)
        {
            this.Logger.LogInformation("Deleting document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            int relationCount = documentRepository.GetTotalRelationCount(document.DocumentId);
            if (relationCount > 1)
            {
                throw new InvalidOperationException("Documents can only be removed if there is one or less relationships");
            }
            else
            {
                // If the storage deletion was successfull or the id was not found on the storage (already deleted) delete the pims reference.
                ExternalResult<string> result = await documentStorageRepository.DeleteDocument(document.MayanId);
                if (result.Status == ExternalResultStatus.Success || result.HttpStatusCode == HttpStatusCode.NotFound)
                {
                    documentRepository.Delete(document);
                    documentRepository.CommitTransaction();
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public async Task<ExternalResult<QueryResult<DocumentType>>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage document types");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<QueryResult<DocumentType>> result = await documentStorageRepository.GetDocumentTypesAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage documents");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<QueryResult<DocumentDetail>> result = await documentStorageRepository.GetDocumentsListAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataType(long mayanDocumentTypeId, string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentTypeMetadataType>> result = await documentStorageRepository.GetDocumentTypeMetadataTypesAsync(mayanDocumentTypeId, ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentMetadata>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage document metadata");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<QueryResult<DocumentMetadata>> result = await documentStorageRepository.GetDocumentMetadataAsync(mayanDocumentId, ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(long mayanDocumentId, long mayanFileId)
        {
            this.Logger.LogInformation("Downloading storage document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<FileDownload> downloadResult = await documentStorageRepository.DownloadFileAsync(mayanDocumentId, mayanFileId);
            return downloadResult;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(long mayanDocumentId)
        {
            this.Logger.LogInformation("Downloading storage document latest");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<DocumentDetail> documentResult = await documentStorageRepository.GetDocumentAsync(mayanDocumentId);
            if (documentResult.Status == ExternalResultStatus.Success)
            {
                if (documentResult.Payload != null)
                {
                    ExternalResult<FileDownload> downloadResult = await documentStorageRepository.DownloadFileAsync(documentResult.Payload.Id, documentResult.Payload.FileLatest.Id);
                    return downloadResult;
                }
                else
                {
                    return new ExternalResult<FileDownload>()
                    {
                        Status = ExternalResultStatus.Error,
                        Message = $"No document with id ${mayanDocumentId} found in the storage",
                    };
                }
            }
            else
            {
                return new ExternalResult<FileDownload>()
                {
                    Status = documentResult.Status,
                    Message = documentResult.Message,
                    HttpStatusCode = documentResult.HttpStatusCode,
                };
            }
        }

        public async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(long documentType, IFormFile fileRaw)
        {
            this.Logger.LogInformation("Uploading storage document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            await this.avService.ScanAsync(fileRaw);
            ExternalResult<DocumentDetail> result = await documentStorageRepository.UploadDocumentAsync(documentType, fileRaw);
            return result;
        }
    }
}
