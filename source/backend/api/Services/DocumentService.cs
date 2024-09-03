using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Config;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Document.UpdateMetadata;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Api.Repositories.Mayan;
using Pims.Av;
using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Polly;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentService implementation provides document managing capabilities.
    /// </summary>
    public class DocumentService : BaseService, IDocumentService
    {
        protected const string MayanGenericErrorMessage = "Error response received from Mayan Document Service";
        private static readonly string[] ValidExtensions =
        {
                "txt",
                "pdf",
                "docx",
                "doc",
                "xlsx",
                "xls",
                "html",
                "odt",
                "png",
                "jpg",
                "bmp",
                "tif",
                "tiff",
                "jpeg",
                "gif",
                "shp",
                "gml",
                "kml",
                "kmz",
                "msg",
        };

        private static readonly string MayanConfigSectionKey = "Mayan";
        private readonly MayanConfig _config;

        private readonly IDocumentRepository documentRepository;
        private readonly IEdmsDocumentRepository documentStorageRepository;
        private readonly IDocumentTypeRepository documentTypeRepository;
        private readonly IAvService avService;
        private readonly IMapper mapper;
        private readonly IOptionsMonitor<AuthClientOptions> keycloakOptions;

        public DocumentService(
            ClaimsPrincipal user,
            IConfiguration configuration,
            ILogger<DocumentService> logger,
            IDocumentRepository documentRepository,
            IEdmsDocumentRepository documentStorageRepository,
            IDocumentTypeRepository documentTypeRepository,
            IAvService avService,
            IMapper mapper,
            IOptionsMonitor<AuthClientOptions> options)
            : base(user, logger)
        {
            this.documentRepository = documentRepository;
            this.documentStorageRepository = documentStorageRepository;
            this.documentTypeRepository = documentTypeRepository;
            this.avService = avService;
            this.mapper = mapper;
            this.keycloakOptions = options;
            _config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, _config);
        }

        public IList<PimsDocumentTyp> GetPimsDocumentTypes()
        {
            this.Logger.LogInformation("Retrieving PIMS document types");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            return documentTypeRepository.GetAll();
        }

        public IList<PimsDocumentTyp> GetPimsDocumentTypes(DocumentRelationType relationshipType)
        {
            this.Logger.LogInformation("Retrieving PIMS document types for relationship type {relationshipType}", relationshipType);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            string categoryType;
            switch (relationshipType)
            {
                case DocumentRelationType.ResearchFiles:
                    categoryType = "RESEARCH";
                    break;
                case DocumentRelationType.AcquisitionFiles:
                    categoryType = "ACQUIRE";
                    break;
                case DocumentRelationType.Leases:
                    categoryType = "LEASLIC";
                    break;
                case DocumentRelationType.Projects:
                    categoryType = "PROJECT";
                    break;
                case DocumentRelationType.ManagementFiles:
                    categoryType = "MANAGEMENT";
                    break;
                case DocumentRelationType.DispositionFiles:
                    categoryType = "DISPOSE";
                    break;
                default:
                    throw new InvalidDataException("The requested category relationship does not exist");
            }
            return documentTypeRepository.GetByCategory(categoryType);
        }

        public async Task<DocumentUploadResponse> UploadDocumentAsync(DocumentUploadRequest uploadRequest)
        {
            this.Logger.LogInformation("Uploading document");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            ExternalResponse<DocumentDetailModel> externalResponse = await UploadDocumentAsync(uploadRequest.DocumentTypeMayanId, uploadRequest.File);
            DocumentUploadResponse response = new DocumentUploadResponse()
            {
                DocumentExternalResponse = externalResponse,
                MetadataExternalResponse = new List<ExternalResponse<DocumentMetadataModel>>(),
            };

            if (externalResponse.Status == ExternalResponseStatus.Success)
            {
                var externalDocument = externalResponse.Payload;
                if (externalDocument.FileLatest == null && _config.UploadRetries > 0)
                {
                    var retryPolicy = Policy<ExternalResponse<DocumentDetailModel>>
                        .HandleResult(result => result.HttpStatusCode != HttpStatusCode.OK || result.Payload.FileLatest == null)
                        .Or<System.Text.Json.JsonException>()
                        .WaitAndRetryAsync(_config.UploadRetries, (int retry) => TimeSpan.FromSeconds(Math.Pow(2, retry)));
                    var detail = await retryPolicy.ExecuteAsync(async () => await GetStorageDocumentDetail(externalDocument.Id));
                    if(detail?.Payload?.FileLatest == null)
                    {
                        response.DocumentExternalResponse.Status = ExternalResponseStatus.Error;
                        response.DocumentExternalResponse.Message = "Timed out waiting for Mayan to process document";
                        return response;
                    }
                    else
                    {
                        _ = PrecacheDocumentPreviews(externalDocument.Id, detail.Payload.FileLatest.Id);
                    }
                }
                else if (externalDocument.FileLatest != null)
                {
                    _ = PrecacheDocumentPreviews(externalDocument.Id, externalDocument.FileLatest.Id);
                }
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

                    response.MetadataExternalResponse = await CreateMetadata(externalDocument.Id, creates);
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
            else
            {
                throw GetMayanResponseError(externalResponse.Message);
            }
            return response;
        }

        public async Task<DocumentUpdateResponse> UpdateDocumentAsync(DocumentUpdateRequest updateRequest)
        {
            this.Logger.LogInformation("Updating document {documentId}", updateRequest.DocumentId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentEdit);

            // update the pims document status
            PimsDocument existingDocument = documentRepository.TryGet(updateRequest.DocumentId);
            if (existingDocument == null)
            {
                throw new BadRequestException($"Document Id {updateRequest.DocumentId} not found.");
            }
            existingDocument.DocumentStatusTypeCode = updateRequest.DocumentStatusCode;
            documentRepository.Update(existingDocument);

            DocumentUpdateResponse response = new DocumentUpdateResponse()
            {
                MetadataExternalResponse = new List<ExternalResponse<DocumentMetadataModel>>(),
            };

            // Retrieve the existing metadata and check if it needs to be updated.
            ExternalResponse<QueryResponse<DocumentMetadataModel>> existingMetadata = await documentStorageRepository.TryGetDocumentMetadataAsync(updateRequest.MayanDocumentId);

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
                response.MetadataExternalResponse.AddRange(await UpdateMetadata(updateRequest.MayanDocumentId, updates));

                // Create metadata of document
                response.MetadataExternalResponse.AddRange(await CreateMetadata(updateRequest.MayanDocumentId, creates));

                // Delete metadata of document
                response.MetadataExternalResponse.AddRange(await DeleteMetadata(updateRequest.MayanDocumentId, deletes));

                foreach (var task in response.MetadataExternalResponse)
                {
                    // Flag to know if at least one call was successful.
                    metadataUpdateSucessful = metadataUpdateSucessful || task.Status == ExternalResponseStatus.Success;
                }
            }

            if (metadataUpdateSucessful)
            {
                documentRepository.CommitTransaction();
                this.Logger.LogInformation("Metadata & Status for Document with id {id} update successfully", updateRequest.DocumentId);
            }
            else
            {
                string error = $"Metadata & Status for Document with id {updateRequest.DocumentId} update aborted. {existingMetadata.Message}";
                this.Logger.LogError(error);
                throw GetMayanResponseError(error);
            }

            return response;
        }

        public async Task<ExternalResponse<string>> DeleteDocumentAsync(PimsDocument document)
        {
            this.Logger.LogInformation("Deleting document {documentId}", document.Internal_Id);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentDelete);

            // If the storage deletion was successful or the id was not found on the storage (already deleted) delete the pims reference.
            ExternalResponse<string> result = await documentStorageRepository.TryDeleteDocument(document.MayanId);
            if (result.Status == ExternalResponseStatus.Success || result.HttpStatusCode == HttpStatusCode.NotFound)
            {
                documentRepository.Delete(document);
                documentRepository.CommitTransaction();
            }
            else
            {
                throw GetMayanResponseError(result.Message);
            }

            return result;
        }

        public async Task<ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>>> GetStorageDocumentTypes(string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage document types");
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.DocumentView, keycloakOptions);

            ExternalResponse<QueryResponse<Models.Mayan.Document.DocumentTypeModel>> result = await documentStorageRepository.TryGetDocumentTypesAsync(ordering, page, pageSize);
            if (result.Status != ExternalResponseStatus.Success)
            {
                throw GetMayanResponseError(result.Message);
            }
            return result;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentDetailModel>>> GetStorageDocumentList(string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage documents");
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResponse<QueryResponse<DocumentDetailModel>> result = await documentStorageRepository.TryGetDocumentsListAsync(ordering, page, pageSize);
            if (result.Status != ExternalResponseStatus.Success)
            {
                throw GetMayanResponseError(result.Message);
            }
            return result;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>>> GetDocumentTypeMetadataType(long mayanDocumentTypeId, string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving document type metadata type {mayanDocumentTypeId}", mayanDocumentTypeId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResponse<QueryResponse<DocumentTypeMetadataTypeModel>> result = await documentStorageRepository.TryGetDocumentTypeMetadataTypesAsync(mayanDocumentTypeId, ordering, page, pageSize);
            if (result.Status != ExternalResponseStatus.Success)
            {
                throw GetMayanResponseError(result.Message);
            }
            return result;
        }

        public async Task<ExternalResponse<QueryResponse<DocumentMetadataModel>>> GetStorageDocumentMetadata(long mayanDocumentId, string ordering = "", int? page = null, int? pageSize = null)
        {
            this.Logger.LogInformation("Retrieving storage document metadata {mayanDocumentId}", mayanDocumentId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResponse<QueryResponse<DocumentMetadataModel>> result = await documentStorageRepository.TryGetDocumentMetadataAsync(mayanDocumentId, ordering, page, pageSize);
            if (result.Status != ExternalResponseStatus.Success)
            {
                throw GetMayanResponseError(result.Message);
            }
            return result;
        }

        public async Task<ExternalResponse<DocumentDetailModel>> GetStorageDocumentDetail(long mayanDocumentId)
        {
            this.Logger.LogInformation("Retrieving storage document, {mayanDocumentId}", mayanDocumentId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResponse<DocumentDetailModel> result = await documentStorageRepository.TryGetDocumentAsync(mayanDocumentId);
            if (result.Status != ExternalResponseStatus.Success)
            {
                throw GetMayanResponseError(result.Message);
            }
            return result;
        }

        public async Task<ExternalResponse<FileDownloadResponse>> DownloadFileAsync(long mayanDocumentId, long mayanFileId)
        {
            this.Logger.LogInformation("Downloading storage document {mayanDocumentId} {mayanFileId}", mayanDocumentId, mayanFileId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResponse<FileDownloadResponse> downloadResult = await documentStorageRepository.TryDownloadFileAsync(mayanDocumentId, mayanFileId);
            if (IsValidDocumentExtension(downloadResult.Payload.FileName))
            {
                if (downloadResult.Status != ExternalResponseStatus.Success)
                {
                    throw GetMayanResponseError(downloadResult.Message);
                }
                return downloadResult;
            }
            else
            {
                return new ExternalResponse<FileDownloadResponse>()
                {
                    Status = ExternalResponseStatus.Error,
                    Message = $"Document with id ${mayanDocumentId} has an invalid extension",
                };
            }
        }

        public async Task<ExternalResponse<FileStreamResponse>> StreamFileAsync(long mayanDocumentId, long mayanFileId)
        {
            this.Logger.LogInformation("Streaming storage document {mayanDocumentId} {mayanFileId}", mayanDocumentId, mayanFileId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            ExternalResponse<FileStreamResponse> downloadResult = await documentStorageRepository.TryStreamFileAsync(mayanDocumentId, mayanFileId);
            if (IsValidDocumentExtension(downloadResult.Payload.FileName))
            {
                if (downloadResult.Status != ExternalResponseStatus.Success)
                {
                    throw GetMayanResponseError(downloadResult.Message);
                }
                return downloadResult;
            }
            else
            {
                return new ExternalResponse<FileStreamResponse>()
                {
                    Status = ExternalResponseStatus.Error,
                    Message = $"Document with id ${mayanDocumentId} has an invalid extension",
                };
            }
        }

        public async Task<ExternalResponse<FileDownloadResponse>> DownloadFileLatestAsync(long mayanDocumentId)
        {
            this.Logger.LogInformation("Downloading storage document latest {mayanDocumentId}", mayanDocumentId);

            ExternalResponse<DocumentDetailModel> documentResult = await documentStorageRepository.TryGetDocumentAsync(mayanDocumentId);
            if (documentResult.Status == ExternalResponseStatus.Success)
            {
                if (documentResult.Payload != null)
                {
                    if (IsValidDocumentExtension(documentResult.Payload.FileLatest.FileName))
                    {
                        ExternalResponse<FileDownloadResponse> downloadResult = await documentStorageRepository.TryDownloadFileAsync(documentResult.Payload.Id, documentResult.Payload.FileLatest.Id);
                        return downloadResult;
                    }
                    else
                    {
                        return new ExternalResponse<FileDownloadResponse>()
                        {
                            Status = ExternalResponseStatus.Error,
                            Message = $"Document with id ${mayanDocumentId} has an invalid extension",
                        };
                    }
                }
                else
                {
                    return new ExternalResponse<FileDownloadResponse>()
                    {
                        Status = ExternalResponseStatus.Error,
                        Message = $"No document with id ${mayanDocumentId} found in the storage",
                    };
                }
            }
            else
            {
                throw GetMayanResponseError(documentResult.Message);
            }
        }

        public async Task<ExternalResponse<FileStreamResponse>> StreamFileLatestAsync(long mayanDocumentId)
        {
            this.Logger.LogInformation("Streaming storage document latest {mayanDocumentId}", mayanDocumentId);

            ExternalResponse<DocumentDetailModel> documentResult = await documentStorageRepository.TryGetDocumentAsync(mayanDocumentId);
            if (documentResult.Status == ExternalResponseStatus.Success)
            {
                if (documentResult.Payload != null)
                {
                    if (IsValidDocumentExtension(documentResult.Payload.FileLatest.FileName))
                    {
                        ExternalResponse<FileStreamResponse> downloadResult = await documentStorageRepository.TryStreamFileAsync(documentResult.Payload.Id, documentResult.Payload.FileLatest.Id);
                        return downloadResult;
                    }
                    else
                    {
                        return new ExternalResponse<FileStreamResponse>()
                        {
                            Status = ExternalResponseStatus.Error,
                            Message = $"Document with id ${mayanDocumentId} has an invalid extension",
                        };
                    }
                }
                else
                {
                    return new ExternalResponse<FileStreamResponse>()
                    {
                        Status = ExternalResponseStatus.Error,
                        Message = $"No document with id ${mayanDocumentId} found in the storage",
                    };
                }
            }
            else
            {
                throw GetMayanResponseError(documentResult.Message);
            }
        }

        public async Task<ExternalResponse<QueryResponse<FilePageModel>>> GetDocumentFilePageListAsync(long documentId, long documentFileId)
        {
            this.Logger.LogInformation("Retrieving pages for document: {documentId} file: {documentFileId}", documentId, documentFileId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            var result = await documentStorageRepository.TryGetFilePageListAsync(documentId, documentFileId, _config.PreviewPages, 1);
            if (result.Status != ExternalResponseStatus.Success)
            {
                throw GetMayanResponseError(result.Message);
            }
            return result;
        }

        public async Task<HttpResponseMessage> DownloadFilePageImageAsync(long mayanDocumentId, long mayanFileId, long mayanFilePageId)
        {
            this.Logger.LogInformation("Downloading file document page for document: {mayanDocumentId} file: {mayanFileId} page(id): {mayanFilePageId}", mayanDocumentId, mayanFileId, mayanFilePageId);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentView);

            var retryPolicy = Policy<HttpResponseMessage>
                        .HandleResult(result => result?.StatusCode != HttpStatusCode.OK || result?.Content == null)
                        .WaitAndRetryAsync(_config.ImageRetries, (int retry) => TimeSpan.FromSeconds(Math.Pow(2, retry)));

            var result = await retryPolicy.ExecuteAsync(async () => await documentStorageRepository.TryGetFilePageImage(mayanDocumentId, mayanFileId, mayanFilePageId));

            if (result.StatusCode != HttpStatusCode.OK)
            {
                throw GetMayanResponseError(await result.Content.ReadAsStringAsync());
            }
            return result;
        }

        private static bool IsValidDocumentExtension(string fileName)
        {
            var fileNameExtension = Path.GetExtension(fileName).Replace(".", string.Empty).ToLower();
            return ValidExtensions.Contains(fileNameExtension);
        }

        private async Task PrecacheDocumentPreviews(long documentId, long documentFileId)
        {
            this.Logger.LogInformation("Precaching the first {_config.PreviewPages} pages in document {documentId}, file {documentFileId}", _config.PreviewPages, documentId, documentFileId);

            // Note that Mayan must generate a list of pages from an uploaded file, so retry if that is not available right away.
            var retryPolicy = Policy<ExternalResponse<QueryResponse<FilePageModel>>>
                        .HandleResult(result => result.HttpStatusCode != HttpStatusCode.OK || result?.Payload?.Results?.Any() == false)
                        .WaitAndRetryAsync(_config.UploadRetries, (int retry) => TimeSpan.FromSeconds(Math.Pow(2, retry)));
            var pages = await retryPolicy.ExecuteAsync(async () => await documentStorageRepository.TryGetFilePageListAsync(documentId, documentFileId, _config.PreviewPages, 1));

            if (pages?.Payload?.Results != null)
            {
                this.Logger.LogInformation("Detected {count} pages to cache", pages.Payload.Results.Count);

                // requesting a document page image forces it to be retrieved faster.
                for (var i = 0; i < pages.Payload.Results.Count - 1; i++)
                {
                    var page = pages.Payload.Results[i];

                    // resulting images are not displayed, just requested in order to populate cache.
                    _ = documentStorageRepository.TryGetFilePageImage(documentId, documentFileId, page.Id);
                }
            }
        }

        private async Task<ExternalResponse<DocumentDetailModel>> UploadDocumentAsync(long documentType, IFormFile fileRaw)
        {
            this.Logger.LogInformation("Uploading storage document {documentType}", documentType);
            this.User.ThrowIfNotAuthorized(Permissions.DocumentAdd);

            await this.avService.ScanAsync(fileRaw);
            if (IsValidDocumentExtension(fileRaw.FileName))
            {
                ExternalResponse<DocumentDetailModel> result = await documentStorageRepository.TryUploadDocumentAsync(documentType, fileRaw);
                return result;
            }
            else
            {
                throw new InvalidDataException("The file extension is not valid for uploading");
            }
        }

        private async Task<List<ExternalResponse<DocumentMetadataModel>>> CreateMetadata(long mayanDocumentId, List<DocumentMetadataUpdateModel> metadataRequest)
        {
            // Save metadata of document
            List<Task<ExternalResponse<DocumentMetadataModel>>> metadataCreateTasks = new List<Task<ExternalResponse<DocumentMetadataModel>>>();
            foreach (var metadata in metadataRequest)
            {
                metadataCreateTasks.Add(documentStorageRepository.TryCreateDocumentMetadataAsync(mayanDocumentId, metadata.MetadataTypeId, metadata.Value));
            }

            await Task.WhenAll(metadataCreateTasks.ToArray());

            List<ExternalResponse<DocumentMetadataModel>> result = new List<ExternalResponse<DocumentMetadataModel>>();

            // Add the metadata response
            foreach (var task in metadataCreateTasks)
            {
                result.Add(task.Result);
            }

            return result;
        }

        private async Task<List<ExternalResponse<DocumentMetadataModel>>> UpdateMetadata(long mayanDocumentId, List<DocumentMetadataUpdateModel> metadataRequest)
        {
            // Save metadata of document
            List<Task<ExternalResponse<DocumentMetadataModel>>> metadataUpdateTasks = new List<Task<ExternalResponse<DocumentMetadataModel>>>();
            foreach (var metadata in metadataRequest)
            {
                metadataUpdateTasks.Add(documentStorageRepository.TryUpdateDocumentMetadataAsync(mayanDocumentId, metadata.Id, metadata.Value));
            }

            await Task.WhenAll(metadataUpdateTasks.ToArray());

            List<ExternalResponse<DocumentMetadataModel>> result = new List<ExternalResponse<DocumentMetadataModel>>();

            // Add the metadata response
            foreach (var task in metadataUpdateTasks)
            {
                result.Add(task.Result);
            }

            return result;
        }

        private async Task<List<ExternalResponse<DocumentMetadataModel>>> DeleteMetadata(long mayanDocumentId, List<DocumentMetadataUpdateModel> metadataRequest)
        {
            // Save metadata of document
            List<Task<ExternalResponse<string>>> metadataDeleteTasks = new List<Task<ExternalResponse<string>>>();
            foreach (var metadata in metadataRequest)
            {
                metadataDeleteTasks.Add(documentStorageRepository.TryDeleteDocumentMetadataAsync(mayanDocumentId, metadata.Id));
            }

            await Task.WhenAll(metadataDeleteTasks.ToArray());

            List<ExternalResponse<DocumentMetadataModel>> result = new List<ExternalResponse<DocumentMetadataModel>>();

            // Add the metadata response
            foreach (var task in metadataDeleteTasks)
            {
                result.Add(new ExternalResponse<DocumentMetadataModel>()
                {
                    Status = task.Result.Status,
                    Message = task.Result.Message,
                    HttpStatusCode = task.Result.HttpStatusCode,
                });
            }

            return result;
        }

        private MayanRepositoryException GetMayanResponseError(string errorMesage)
        {
            if (_config.ExposeErrors)
            {
                return new MayanRepositoryException(errorMesage);
            }

            return new MayanRepositoryException(MayanGenericErrorMessage);
        }
    }
}
