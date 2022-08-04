using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Repositories.Mayan;
using Pims.Av;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentService implementation provides document managing capabilities.
    /// </summary>
    public class DocumentService : IDocumentService
    {
        private readonly IDocumentRepository documentRepository;
        private readonly IDocumentActivityRepository documentActivityRespository;
        private readonly IEdmsDocumentRepository documentStorageRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IAvService avService;
        private readonly IMapper mapper;

        public DocumentService(
            IDocumentRepository documentRepository,
            IDocumentActivityRepository documentActivityRespository,
            IEdmsDocumentRepository documentStorageRepository,
            IDocumentTypeRepository documentTypeRepository,
            IAvService avService,
            IMapper mapper)
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
            return documentTypeRepository.GetAll();
        }

        public IList<PimsActivityInstanceDocument> GetActivityDocuments(long activityId)
        {
            return documentActivityRespository.GetAllByActivity(activityId);
        }

        public async Task<bool> DeleteActivityDocumentAsync(PimsActivityInstanceDocument activityDocument)
        {
            IList<PimsActivityInstanceDocument> existingActivityDocuments = documentActivityRespository.GetAllByDocument(activityDocument.DocumentId);
            if (existingActivityDocuments.Count == 1)
            {
                documentActivityRespository.Delete(activityDocument);
                return await DeleteDocumentAsync(activityDocument.Document);
            }
            else
            {
                documentActivityRespository.Delete(activityDocument);
                documentActivityRespository.CommitTransaction();
                return true;
            }
        }

        public async Task<DocumentUploadResponse> UploadActivityDocumentAsync(long activityId, DocumentUploadRequest uploadRequest)
        {
            ExternalResult<DocumentDetail> externalResult = await UploadDocumentAsync(uploadRequest.DocumentTypeMayanId, uploadRequest.File);
            DocumentUploadResponse response = new DocumentUploadResponse() { ExternalResult = externalResult };
            if (externalResult.Status == ExternalResultStatus.Success)
            {
                // Create the pims document
                var externalDocument = externalResult.Payload;
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

        public async Task<bool> DeleteDocumentAsync(PimsDocument document)
        {
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
            ExternalResult<QueryResult<DocumentType>> result = await documentStorageRepository.GetDocumentTypesAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentDetail>> result = await documentStorageRepository.GetDocumentsListAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentMetadata>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentMetadata>> result = await documentStorageRepository.GetDocumentMetadataAsync(mayanDocumentId, ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(long mayanDocumentId, long mayanFileId)
        {
            ExternalResult<FileDownload> downloadResult = await documentStorageRepository.DownloadFileAsync(mayanDocumentId, mayanFileId);
            return downloadResult;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(long mayanDocumentId)
        {
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
            await this.avService.ScanAsync(fileRaw);
            ExternalResult<DocumentDetail> result = await documentStorageRepository.UploadDocumentAsync(documentType, fileRaw);
            return result;
        }
    }
}
