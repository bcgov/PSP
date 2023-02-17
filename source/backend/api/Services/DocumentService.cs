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
using Pims.Api.Models.Download;
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
        private readonly IEdmsDocumentRepository documentStorageRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IAvService avService;
        private readonly IMapper mapper;

        public DocumentService(
            ClaimsPrincipal user,
            ILogger<DocumentService> logger,
            IDocumentRepository documentRepository,
            IEdmsDocumentRepository documentStorageRepository,
            IDocumentTypeRepository documentTypeRepository,
            IAvService avService,
            IMapper mapper)
            : base(user, logger)
        {
            this.documentRepository = documentRepository;
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

        public async Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            ExternalResult<DocumentDetail> externalResult = await UploadDocumentAsync(uploadRequest.DocumentTypeMayanId, uploadRequest.File);
            DocumentUploadResponse response = new DocumentUploadResponse()
            {
                DocumentExternalResult = externalResult,
                MetadataExternalResult = new List<ExternalResult<DocumentMetadata>>(),
            };

            if (externalResult.Status == ExternalResultStatus.Success)
            {
                var externalDocument = externalResult.Payload;

                // Create metadata of document
                if (uploadRequest.DocumentMetadata != null)
                {
                    List<DocumentMetadataUpdateModel> creates = new List<DocumentMetadataUpdateModel>();
                    foreach (var metadata in uploadRequest.DocumentMetadata)
                    {
                        if (!string.IsNullOrEmpty(metadata.Value))
                        {
                            creates.Add(metadata);
                        }
                    }

                    response.MetadataExternalResult = await CreateMetadata(externalDocument.Id, creates);
                }

                // Create the pims document
                PimsDocument newPimsDocument = new PimsDocument()
                {
                    FileName = externalDocument.Label,
                    DocumentTypeId = uploadRequest.DocumentTypeId,
                    DocumentStatusTypeCode = uploadRequest.DocumentStatusCode,
                    MayanId = externalDocument.Id,
                };

                documentRepository.Add(newPimsDocument);
                documentRepository.CommitTransaction();

                response.Document = mapper.Map<DocumentModel>(newPimsDocument);
            }
            return response;
        }

        public async Task<DocumentUpdateResponse> UpdateDocumentAsync(DocumentUpdateRequest updateRequest)
        {
            this.Logger.LogInformation("Updating document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentEdit);

            // update the pims document status
            PimsDocument existingDocument = documentRepository.TryGet(updateRequest.DocumentId);
            if (existingDocument == null)
            {
                throw new BadRequestException("Document Id not found.");
            }
            existingDocument.DocumentStatusTypeCode = updateRequest.DocumentStatusCode;
            documentRepository.Update(existingDocument);

            DocumentUpdateResponse response = new DocumentUpdateResponse()
            {
                MetadataExternalResult = new List<ExternalResult<DocumentMetadata>>(),
            };

            // Retrieve the existing metadata and check if it needs to be updated.
            ExternalResult<QueryResult<DocumentMetadata>> existingMetadata = await documentStorageRepository.TryGetDocumentMetadataAsync(updateRequest.MayanDocumentId);

            List<DocumentMetadataUpdateModel> updates = new List<DocumentMetadataUpdateModel>();
            List<DocumentMetadataUpdateModel> creates = new List<DocumentMetadataUpdateModel>();
            List<DocumentMetadataUpdateModel> deletes = new List<DocumentMetadataUpdateModel>();

            foreach (var updateMetadata in updateRequest.DocumentMetadata)
            {
                var existing = existingMetadata.Payload.Results.FirstOrDefault(x => x.MetadataType.Id == updateMetadata.MetadataTypeId);

                if (existing == null)
                {
                    if (!string.IsNullOrEmpty(updateMetadata.Value))
                    {
                        creates.Add(updateMetadata);
                    }
                }
                else if (existing.Value != updateMetadata.Value && !string.IsNullOrEmpty(updateMetadata.Value))
                {
                    updateMetadata.Id = existing.Id;
                    updates.Add(updateMetadata);
                }
                else if (string.IsNullOrEmpty(updateMetadata.Value))
                {
                    updateMetadata.Id = existing.Id;
                    deletes.Add(updateMetadata);
                }
            }

            var metadataUpdateSucessful = false;

            // If there are no changes, mark the external update as successfull
            if (updates.Count == 0 && creates.Count == 0 && deletes.Count == 0)
            {
                metadataUpdateSucessful = true;
            }
            else
            {
                // Update metadata of document
                response.MetadataExternalResult.AddRange(await UpdateMetadata(updateRequest.MayanDocumentId, updates));

                // Create metadata of document
                response.MetadataExternalResult.AddRange(await CreateMetadata(updateRequest.MayanDocumentId, creates));

                // Delete metadata of document
                response.MetadataExternalResult.AddRange(await DeleteMetadata(updateRequest.MayanDocumentId, deletes));

                foreach (var task in response.MetadataExternalResult)
                {
                    // Flag to know if at least one call was successful.
                    metadataUpdateSucessful = metadataUpdateSucessful || task.Status == ExternalResultStatus.Success;
                }
            }

            if (metadataUpdateSucessful)
            {
                documentRepository.CommitTransaction();
                this.Logger.LogInformation("Metadata & Status for Document with id {id} update successfully", updateRequest.DocumentId);
            }
            else
            {
                this.Logger.LogError("Metadata & Status for Document with id {id} update aborted", updateRequest.DocumentId);
            }

            return response;
        }

        public async Task<ExternalResult<string>> DeleteDocumentAsync(PimsDocument document)
        {
            this.Logger.LogInformation("Deleting document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            // If the storage deletion was successful or the id was not found on the storage (already deleted) delete the pims reference.
            ExternalResult<string> result = await documentStorageRepository.TryDeleteDocument(document.MayanId);
            if (result.Status == ExternalResultStatus.Success || result.HttpStatusCode == HttpStatusCode.NotFound)
            {
                documentRepository.Delete(document);
                documentRepository.CommitTransaction();
            }

            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentType>>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage document types");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<QueryResult<DocumentType>> result = await documentStorageRepository.TryGetDocumentTypesAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentDetail>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage documents");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<QueryResult<DocumentDetail>> result = await documentStorageRepository.TryGetDocumentsListAsync(ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentTypeMetadataType>>> GetDocumentTypeMetadataType(long mayanDocumentTypeId, string ordering = "", int? page = null, int? pageSize = null)
        {
            ExternalResult<QueryResult<DocumentTypeMetadataType>> result = await documentStorageRepository.TryGetDocumentTypeMetadataTypesAsync(mayanDocumentTypeId, ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<QueryResult<DocumentMetadata>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage document metadata");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<QueryResult<DocumentMetadata>> result = await documentStorageRepository.TryGetDocumentMetadataAsync(mayanDocumentId, ordering, page, pageSize);
            return result;
        }

        public async Task<ExternalResult<DocumentDetail>> GetStorageDocumentDetail(long mayanDocumentId)
        {
            this.Logger.LogInformation("Retrieving storage document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<DocumentDetail> result = await documentStorageRepository.TryGetDocumentAsync(mayanDocumentId);
            return result;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileAsync(long mayanDocumentId, long mayanFileId)
        {
            this.Logger.LogInformation("Downloading storage document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<FileDownload> downloadResult = await documentStorageRepository.TryDownloadFileAsync(mayanDocumentId, mayanFileId);
            return downloadResult;
        }

        public async Task<ExternalResult<FileDownload>> DownloadFileLatestAsync(long mayanDocumentId)
        {
            this.Logger.LogInformation("Downloading storage document latest");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResult<DocumentDetail> documentResult = await documentStorageRepository.TryGetDocumentAsync(mayanDocumentId);
            if (documentResult.Status == ExternalResultStatus.Success)
            {
                if (documentResult.Payload != null)
                {
                    ExternalResult<FileDownload> downloadResult = await documentStorageRepository.TryDownloadFileAsync(documentResult.Payload.Id, documentResult.Payload.FileLatest.Id);
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

        private async Task<ExternalResult<DocumentDetail>> UploadDocumentAsync(long documentType, IFormFile fileRaw)
        {
            this.Logger.LogInformation("Uploading storage document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            await this.avService.ScanAsync(fileRaw);
            ExternalResult<DocumentDetail> result = await documentStorageRepository.TryUploadDocumentAsync(documentType, fileRaw);
            return result;
        }

        private async Task<List<ExternalResult<DocumentMetadata>>> CreateMetadata(long mayanDocumentId, List<DocumentMetadataUpdateModel> metadataRequest)
        {
            // Save metadata of document
            IList<Task<ExternalResult<DocumentMetadata>>> metadataCreateTasks = new List<Task<ExternalResult<DocumentMetadata>>>();
            foreach (var metadata in metadataRequest)
            {
                metadataCreateTasks.Add(documentStorageRepository.TryCreateDocumentMetadataAsync(mayanDocumentId, metadata.MetadataTypeId, metadata.Value));
            }

            await Task.WhenAll(metadataCreateTasks.ToArray());

            List<ExternalResult<DocumentMetadata>> result = new List<ExternalResult<DocumentMetadata>>();

            // Add the metadata response
            foreach (var task in metadataCreateTasks)
            {
                result.Add(task.Result);
            }

            return result;
        }

        private async Task<List<ExternalResult<DocumentMetadata>>> UpdateMetadata(long mayanDocumentId, List<DocumentMetadataUpdateModel> metadataRequest)
        {
            // Save metadata of document
            IList<Task<ExternalResult<DocumentMetadata>>> metadataUpdateTasks = new List<Task<ExternalResult<DocumentMetadata>>>();
            foreach (var metadata in metadataRequest)
            {
                metadataUpdateTasks.Add(documentStorageRepository.TryUpdateDocumentMetadataAsync(mayanDocumentId, metadata.Id, metadata.Value));
            }

            await Task.WhenAll(metadataUpdateTasks.ToArray());

            List<ExternalResult<DocumentMetadata>> result = new List<ExternalResult<DocumentMetadata>>();

            // Add the metadata response
            foreach (var task in metadataUpdateTasks)
            {
                result.Add(task.Result);
            }

            return result;
        }

        private async Task<List<ExternalResult<DocumentMetadata>>> DeleteMetadata(long mayanDocumentId, List<DocumentMetadataUpdateModel> metadataRequest)
        {
            // Save metadata of document
            IList<Task<ExternalResult<string>>> metadataDeleteTasks = new List<Task<ExternalResult<string>>>();
            foreach (var metadata in metadataRequest)
            {
                metadataDeleteTasks.Add(documentStorageRepository.TryDeleteDocumentMetadataAsync(mayanDocumentId, metadata.Id));
            }

            await Task.WhenAll(metadataDeleteTasks.ToArray());

            List<ExternalResult<DocumentMetadata>> result = new List<ExternalResult<DocumentMetadata>>();

            // Add the metadata response
            foreach (var task in metadataDeleteTasks)
            {
                result.Add(new ExternalResult<DocumentMetadata>()
                {
                    Status = task.Result.Status,
                    Message = task.Result.Message,
                    HttpStatusCode = task.Result.HttpStatusCode,
                });
            }

            return result;
        }
    }
}
