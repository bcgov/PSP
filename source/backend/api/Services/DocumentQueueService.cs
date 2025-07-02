using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    /// <summary>
    /// DocumentQueueService implementation provides document queue managing capabilities.
    /// </summary>
    public class DocumentQueueService : BaseService, IDocumentQueueService
    {
        private readonly IDocumentQueueRepository _documentQueueRepository;
        private readonly IDocumentRepository _documentRepository;
        private readonly IDocumentTypeRepository _documentTypeRepository;
        private readonly IDocumentService _documentService;
        private readonly IOptionsMonitor<AuthClientOptions> _keycloakOptions;

        public DocumentQueueService(
            ClaimsPrincipal user,
            ILogger<DocumentService> logger,
            IDocumentQueueRepository documentQueueRepository,
            IDocumentRepository documentRepository,
            IDocumentTypeRepository documentTypeRepository,
            IDocumentService documentService,
            IOptionsMonitor<AuthClientOptions> options)
            : base(user, logger)
        {
            this._documentQueueRepository = documentQueueRepository;
            this._documentRepository = documentRepository;
            this._documentTypeRepository = documentTypeRepository;
            this._documentService = documentService;
            this._keycloakOptions = options;
        }

        /// <summary>
        /// Get document in the document queue based on the specified id.
        /// </summary>
        /// <param name="documentQueueId">The id of the document in the queue.</param>
        /// <returns><see cref="PimsDocumentQueue"/> that match the id criteria.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        /// <exception cref="KeyNotFoundException">If the requested Id does not exist.</exception>
        public PimsDocumentQueue GetById(long documentQueueId)
        {
            this.Logger.LogInformation("Retrieving queued PIMS document using id {documentQueueId}", documentQueueId);
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            var documentQueue = _documentQueueRepository.TryGetById(documentQueueId);
            if (documentQueue == null)
            {
                throw new KeyNotFoundException($"Unable to find queued document by id: ${documentQueueId}");
            }
            return documentQueue;
        }

        /// <summary>
        /// Searches for documents in the document queue based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter criteria to apply when searching the document queue.</param>
        /// <returns>An enumerable collection of <see cref="PimsDocumentQueue"/> that match the filter criteria.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        public IEnumerable<PimsDocumentQueue> SearchDocumentQueue(DocumentQueueFilter filter)
        {
            this.Logger.LogInformation("Retrieving queued PIMS documents using filter {filter}", filter.Serialize());
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            var queuedDocuments = _documentQueueRepository.GetAllByFilter(filter);

            return filter.MaxFileSize != null
                ? FilterDocumentsByMaxFileSize(queuedDocuments, filter.MaxFileSize.Value)
                : queuedDocuments;
        }

        /// <summary>
        /// Updates the specified document queue.
        /// </summary>
        /// <param name="documentQueue">The document queue object to update.</param>
        /// <returns>The updated document queue object.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        public async Task<PimsDocumentQueue> Update(PimsDocumentQueue documentQueue)
        {
            this.Logger.LogInformation("Updating queued document {documentQueueId}", documentQueue.DocumentQueueId);
            this.Logger.LogDebug("Incoming queued document {document}", documentQueue.Serialize());

            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            await _documentQueueRepository.Update(documentQueue);

            return documentQueue;
        }

        /// <summary>
        /// Polls for the status of a document in mayan, and updates the queue based on the result.
        /// </summary>
        /// <param name="documentQueue">The document queue object containing the document details.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated document queue object, or null if the polling failed.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the document queue does not have a valid document ID or related document.</exception>
        public async Task<PimsDocumentQueue> PollForDocument(PimsDocumentQueue documentQueue)
        {
            this.Logger.LogInformation("Polling queued document {documentQueueId}", documentQueue.DocumentQueueId);
            this.Logger.LogDebug("Polling queued document {document}", documentQueue.Serialize());

            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            ValidateDocumentQueueForPolling(documentQueue);

            var databaseDocumentQueue = _documentQueueRepository.TryGetById(documentQueue.DocumentQueueId);
            ValidateDatabaseDocumentQueueForPolling(databaseDocumentQueue, documentQueue.DocumentQueueId);

            var relatedDocument = _documentRepository.TryGet(documentQueue.DocumentId.Value);
            await ValidateRelatedDocumentForPolling(relatedDocument, databaseDocumentQueue);

            return await PollDocumentStatusAsync(databaseDocumentQueue, relatedDocument);
        }

        public async Task<PimsDocumentQueue> Upload(PimsDocumentQueue documentQueue)
        {
            this.Logger.LogInformation("Uploading queued document {documentQueueId}", documentQueue.DocumentQueueId);
            this.Logger.LogDebug("Uploading queued document {document}", documentQueue.Serialize());

            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            var databaseDocumentQueue = _documentQueueRepository.TryGetById(documentQueue.DocumentQueueId);
            ValidateDatabaseDocumentQueueForUpload(databaseDocumentQueue, documentQueue.DocumentQueueId);

            databaseDocumentQueue.DocProcessStartDt = DateTime.UtcNow;

            if (!ValidateQueuedDocument(databaseDocumentQueue, documentQueue))
            {
                databaseDocumentQueue.MayanError = "Document is invalid.";
                await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                return databaseDocumentQueue;
            }

            databaseDocumentQueue = HandleRetryForErroredDocument(databaseDocumentQueue);

            var relatedDocument = _documentRepository.TryGetDocumentRelationships(databaseDocumentQueue.DocumentId.Value);
            await ValidateRelatedDocumentForUpload(relatedDocument, databaseDocumentQueue);

            return await UploadDocumentAsync(databaseDocumentQueue, relatedDocument);
        }

        private List<PimsDocumentQueue> FilterDocumentsByMaxFileSize(IEnumerable<DocumentQueueSearchResult> queuedDocuments, long maxFileSize)
        {
            List<PimsDocumentQueue> documentsBelowMaxFileSize = new();
            long totalFileSize = 0;

            foreach (var currentDocument in queuedDocuments)
            {
                if (currentDocument.DocumentSize + totalFileSize <= maxFileSize)
                {
                    totalFileSize += currentDocument.DocumentSize;
                    documentsBelowMaxFileSize.Add(currentDocument);
                }
            }

            if (documentsBelowMaxFileSize.Count == 0 && queuedDocuments.Any())
            {
                documentsBelowMaxFileSize.Add(queuedDocuments.First());
            }

            this.Logger.LogDebug("Returning {length} documents below file size", documentsBelowMaxFileSize.Count);
            return documentsBelowMaxFileSize;
        }

        private void ValidateDocumentQueueForPolling(PimsDocumentQueue documentQueue)
        {
            if (documentQueue.DocumentId == null)
            {
                this.Logger.LogError("Polled queued document does not have a document Id {documentQueueId}", documentQueue.DocumentQueueId);
                throw new InvalidDataException("DocumentId is required to poll for a document.");
            }
        }

        private void ValidateDatabaseDocumentQueueForPolling(PimsDocumentQueue databaseDocumentQueue, long documentQueueId)
        {
            if (databaseDocumentQueue == null)
            {
                this.Logger.LogError("Unable to find document queue with {id}", documentQueueId);
                throw new KeyNotFoundException($"Unable to find document queue with matching id: {documentQueueId}");
            }

            if (databaseDocumentQueue.DocumentQueueStatusTypeCode != DocumentQueueStatusTypes.PROCESSING.ToString())
            {
                this.Logger.LogError("Document Queue {documentQueueId} is not in valid state, aborting poll.", documentQueueId);
                throw new InvalidOperationException("Document queue is not in a valid state for polling.");
            }
        }

        private async Task ValidateRelatedDocumentForPolling(PimsDocument relatedDocument, PimsDocumentQueue databaseDocumentQueue)
        {
            if (relatedDocument?.MayanId == null || relatedDocument?.MayanId < 0)
            {
                this.Logger.LogError("Queued Document {documentQueueId} has no Mayan ID and is invalid.", databaseDocumentQueue.DocumentQueueId);
                databaseDocumentQueue.MayanError = "Document does not have a valid MayanId.";
                await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                throw new InvalidDataException("Document does not have a valid MayanId.");
            }
        }

        private async Task<PimsDocumentQueue> PollDocumentStatusAsync(PimsDocumentQueue databaseDocumentQueue, PimsDocument relatedDocument)
        {
            var documentDetailsResponse = await _documentService.GetStorageDocumentDetail(relatedDocument.MayanId.Value);

            if (documentDetailsResponse.Status != ExternalResponseStatus.Success || documentDetailsResponse?.Payload == null)
            {
                this.Logger.LogError("Polling for queued document {documentQueueId} failed with status {documentDetailsResponseStatus}", databaseDocumentQueue.DocumentQueueId, documentDetailsResponse.Status);
                databaseDocumentQueue.MayanError = "Document Polling failed.";
                await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                return databaseDocumentQueue;
            }

            if (documentDetailsResponse.Payload.FileLatest?.Id == null)
            {
                this.Logger.LogInformation("Polling for queued document {documentQueueId} complete, file still processing", databaseDocumentQueue.DocumentQueueId);
            }
            else
            {
                this.Logger.LogInformation("Polling for queued document {documentQueueId} complete, file uploaded successfully", databaseDocumentQueue.DocumentQueueId);
                await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.SUCCESS);
            }

            return databaseDocumentQueue;
        }

        private void ValidateDatabaseDocumentQueueForUpload(PimsDocumentQueue databaseDocumentQueue, long documentQueueId)
        {
            if (databaseDocumentQueue == null)
            {
                this.Logger.LogError("Unable to find document queue with {id}", documentQueueId);
                throw new KeyNotFoundException($"Unable to find document queue with matching id: {documentQueueId}");
            }
        }

        private PimsDocumentQueue HandleRetryForErroredDocument(PimsDocumentQueue databaseDocumentQueue)
        {
            if (databaseDocumentQueue.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.PIMS_ERROR.ToString() ||
                databaseDocumentQueue.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.MAYAN_ERROR.ToString())
            {
                this.Logger.LogDebug("Document Queue {documentQueueId}, previously errored, retrying", databaseDocumentQueue.DocumentQueueId);
                databaseDocumentQueue.DocProcessRetries = ++databaseDocumentQueue.DocProcessRetries ?? 1;
                databaseDocumentQueue.DocProcessEndDt = null;
            }
            return databaseDocumentQueue;
        }

        private async Task ValidateRelatedDocumentForUpload(PimsDocument relatedDocument, PimsDocumentQueue databaseDocumentQueue)
        {
            if (relatedDocument?.DocumentTypeId == null)
            {
                databaseDocumentQueue.MayanError = "Document does not have a valid DocumentType.";
                this.Logger.LogError("Queued document {documentQueueId} does not have a related PIMS_DOCUMENT {documentId} with valid DocumentType, aborting.", databaseDocumentQueue.DocumentQueueId, relatedDocument?.DocumentId);
                await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                throw new InvalidDataException("Document does not have a valid DocumentType.");
            }
        }

        private async Task<PimsDocumentQueue> UploadDocumentAsync(PimsDocumentQueue databaseDocumentQueue, PimsDocument relatedDocument)
        {
            try
            {
                var documentType = _documentTypeRepository.GetById(relatedDocument.DocumentTypeId);

                using var memStream = new MemoryStream(databaseDocumentQueue.Document);
                var file = new FormFile(memStream, 0, databaseDocumentQueue.Document.Length, relatedDocument.FileName, relatedDocument.FileName);

                var request = new DocumentUploadRequest
                {
                    File = file,
                    DocumentStatusCode = relatedDocument.DocumentStatusTypeCode,
                    DocumentTypeId = relatedDocument.DocumentTypeId,
                    DocumentTypeMayanId = documentType.MayanId,
                    DocumentId = relatedDocument.DocumentId,
                    DocumentMetadata = databaseDocumentQueue.DocumentMetadata != null
                        ? JsonSerializer.Deserialize<List<DocumentMetadataUpdateModel>>(databaseDocumentQueue.DocumentMetadata)
                        : null,
                };

                this.Logger.LogDebug("Document Queue {documentQueueId}, beginning upload.", databaseDocumentQueue.DocumentQueueId);
                var response = await _documentService.UploadDocumentAsync(request, true);

                if (response.DocumentExternalResponse.Status != ExternalResponseStatus.Success || response?.DocumentExternalResponse?.Payload == null)
                {
                    this.Logger.LogError(
                        "Queued document upload failed {databaseDocumentQueueDocumentQueueId} {databaseDocumentQueueDocumentQueueStatusTypeCode}, {documentExternalResponseStatus}",
                        databaseDocumentQueue.DocumentQueueId,
                        databaseDocumentQueue.DocumentQueueStatusTypeCode,
                        response.DocumentExternalResponse.Status);

                    databaseDocumentQueue.MayanError = $"Failed to upload document, Mayan error: {response.DocumentExternalResponse.Message}";
                    await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.MAYAN_ERROR);
                    return databaseDocumentQueue;
                }

                response.MetadataExternalResponse
                    .Where(r => r.Status != ExternalResponseStatus.Success)
                    .ToList()
                    .ForEach(r => this.Logger.LogError("url: ${url} status: ${status} message ${message}", r.Payload.Url, r.Status, r.Message));

                if (response.DocumentExternalResponse?.Payload?.FileLatest?.Id != null)
                {
                    await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.SUCCESS);
                }
                else
                {
                    await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PROCESSING);
                }
            }
            catch (Exception ex) when (ex is BadRequestException || ex is KeyNotFoundException || ex is InvalidDataException || ex is JsonException)
            {
                this.Logger.LogError($"Error: {ex.Message}");
                databaseDocumentQueue.MayanError = ex.Message;
                await UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
            }

            return databaseDocumentQueue;
        }

        private async Task UpdateDocumentQueueStatus(PimsDocumentQueue documentQueue, DocumentQueueStatusTypes statusType)
        {
            documentQueue.DocumentQueueStatusTypeCode = statusType.ToString();
            bool removeDocument = false;

            if (statusType != DocumentQueueStatusTypes.PROCESSING && statusType != DocumentQueueStatusTypes.PENDING)
            {
                documentQueue.DocProcessEndDt = DateTime.UtcNow;
                if (statusType == DocumentQueueStatusTypes.SUCCESS)
                {
                    documentQueue.Document = null;
                    removeDocument = true;
                }
            }

            await _documentQueueRepository.Update(documentQueue, removeDocument);
        }

        private bool ValidateQueuedDocument(PimsDocumentQueue databaseDocumentQueue, PimsDocumentQueue externalDocument)
        {
            if (databaseDocumentQueue.DocumentQueueStatusTypeCode != externalDocument.DocumentQueueStatusTypeCode)
            {
                this.Logger.LogError("Requested document queue status: {documentQueueStatusTypeCode} does not match current database status: {documentQueueStatusTypeCode}", externalDocument.DocumentQueueStatusTypeCode, databaseDocumentQueue.DocumentQueueStatusTypeCode);
                return false;
            }

            if (databaseDocumentQueue.DocProcessRetries != externalDocument.DocProcessRetries)
            {
                this.Logger.LogError("Requested document retries: {documentQueueStatusTypeCode} does not match current database retries: {documentQueueStatusTypeCode}", externalDocument.DocumentQueueStatusTypeCode, databaseDocumentQueue.DocumentQueueStatusTypeCode);
                return false;
            }

            if (databaseDocumentQueue.Document == null || databaseDocumentQueue.DocumentId == null)
            {
                this.Logger.LogError("Queued document file content is empty, unable to upload.");
                return false;
            }

            return true;
        }
    }
}
