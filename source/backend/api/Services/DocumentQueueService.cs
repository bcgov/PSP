using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.InkML;
using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Document.Upload;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Exceptions;
using Pims.Core.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Http.Configuration;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Repositories;
using Serilog.Filters;

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
        /// Searches for documents in the document queue based on the specified filter.
        /// </summary>
        /// <param name="filter">The filter criteria to apply when searching the document queue.</param>
        /// <returns>An enumerable collection of <see cref="PimsDocumentQueue"/> that match the filter criteria.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        public IEnumerable<PimsDocumentQueue> SearchDocumentQueue(DocumentQueueFilter filter)
        {
            this.Logger.LogInformation("Retrieving queued PIMS documents using filter {filter}", filter);
            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            return _documentQueueRepository.GetAllByFilter(filter);
        }

        /// <summary>
        /// Updates the specified document queue.
        /// </summary>
        /// <param name="documentQueue">The document queue object to update.</param>
        /// <returns>The updated document queue object.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        public PimsDocumentQueue Update(PimsDocumentQueue documentQueue)
        {
            this.Logger.LogInformation("Updating queued document {documentQueueId}", documentQueue.DocumentQueueId);
            this.Logger.LogDebug("Incoming queued document {document}", documentQueue.Serialize());

            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            _documentQueueRepository.Update(documentQueue);
            _documentQueueRepository.CommitTransaction();
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
            if (documentQueue.DocumentId == null)
            {
                this.Logger.LogError("polled queued document does not have a document Id {documentQueueId}", documentQueue.DocumentQueueId);
                throw new InvalidDataException("DocumentId is required to poll for a document.");
            }

            var databaseDocumentQueue = _documentQueueRepository.TryGetById(documentQueue.DocumentQueueId);
            if (databaseDocumentQueue == null)
            {
                this.Logger.LogError("Unable to find document queue with {id}", documentQueue.DocumentQueueId);
                throw new KeyNotFoundException($"Unable to find document queue with matching id: {documentQueue.DocumentQueueId}");
            }

            var relatedDocument = _documentRepository.TryGet(documentQueue.DocumentId.Value);

            if (relatedDocument?.MayanId == null || relatedDocument?.MayanId < 0)
            {
                this.Logger.LogError("Queued Document {documentQueueId} has no mayan id and is invalid.", documentQueue.DocumentQueueId);
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                return databaseDocumentQueue;
            }

            ExternalResponse<DocumentDetailModel> documentDetailsResponse = await _documentService.GetStorageDocumentDetail(relatedDocument.MayanId.Value);

            if (documentDetailsResponse.Status != ExternalResponseStatus.Success || documentDetailsResponse?.Payload == null)
            {
                this.Logger.LogError("Polling for queued document {documentQueueId} failed with status {documentDetailsResponseStatus}", documentQueue.DocumentQueueId, documentDetailsResponse.Status);
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                return databaseDocumentQueue;
            }

            if (documentDetailsResponse.Payload.FileLatest?.Id == null)
            {
                this.Logger.LogInformation("Polling for queued document {documentQueueId} complete, file still processing", documentQueue.DocumentQueueId);
            }
            else
            {
                this.Logger.LogInformation("Polling for queued document {documentQueueId} complete, file uploaded successfully", documentQueue.DocumentQueueId);
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.SUCCESS);
            }

            return databaseDocumentQueue;
        }


        /// <summary>
        /// Uploads the specified document queue.
        /// </summary>
        /// <param name="documentQueue">The document queue object containing the document to upload.</param>
        /// <returns>A task that represents the asynchronous operation. The task result contains the updated document queue object, or null if the upload failed.</returns>
        /// <exception cref="UnauthorizedAccessException">Thrown when the user is not authorized to perform this operation.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the document queue does not have a valid document ID or related document.</exception>
        public async Task<PimsDocumentQueue> Upload(PimsDocumentQueue documentQueue)
        {
            this.Logger.LogInformation("Uploading queued document {documentQueueId}", documentQueue.DocumentQueueId);
            this.Logger.LogDebug("Uploading queued document {document}", documentQueue.Serialize());

            this.User.ThrowIfNotAuthorizedOrServiceAccount(Permissions.SystemAdmin, this._keycloakOptions);

            var databaseDocumentQueue = _documentQueueRepository.TryGetById(documentQueue.DocumentQueueId);
            if(databaseDocumentQueue == null)
            {
                this.Logger.LogError("Unable to find document queue with {id}", documentQueue.DocumentQueueId);
                throw new KeyNotFoundException($"Unable to find document queue with matching id: {documentQueue.DocumentQueueId}");
            }
            databaseDocumentQueue.DocProcessStartDt = DateTime.UtcNow;

            // if the document queued for upload is already in an error state, update the retries.
            if (databaseDocumentQueue.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.PIMS_ERROR.ToString() || databaseDocumentQueue.DocumentQueueStatusTypeCode == DocumentQueueStatusTypes.MAYAN_ERROR.ToString())
            {
                databaseDocumentQueue.DocProcessRetries += 1;
                databaseDocumentQueue.DocProcessEndDt = null;
            }

            bool isValid = ValidateQueuedDocument(databaseDocumentQueue, documentQueue);
            if (!isValid)
            {
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                return databaseDocumentQueue;
            }
            UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PROCESSING);

            PimsDocument relatedDocument = null;
            relatedDocument = _documentRepository.TryGetDocumentRelationships(databaseDocumentQueue.DocumentId.Value);
            if (relatedDocument?.DocumentTypeId == null)
            {
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
                this.Logger.LogError("Queued document {documentQueueId} does not have a related PIMS_DOCUMENT {documentId} with valid DocumentType, aborting.", databaseDocumentQueue.DocumentQueueId, relatedDocument?.DocumentId);
                return databaseDocumentQueue;
            }
            else if (relatedDocument?.MayanId != null && relatedDocument?.MayanId > 0)
            {
                this.Logger.LogInformation("Queued document {documentQueueId} already has a mayan id {mayanid}, no further processing required.", databaseDocumentQueue.DocumentQueueId, relatedDocument.MayanId);
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.SUCCESS);
                return databaseDocumentQueue; // The document poll job should pick this up and fix the document queue status.
            }

            try
            {
                PimsDocumentTyp documentTyp = _documentTypeRepository.GetById(relatedDocument.DocumentTypeId); // throws KeyNotFoundException if not found.

                IFormFile file = null;
                using MemoryStream memStream = new(databaseDocumentQueue.Document);
                file = new FormFile(memStream, 0, databaseDocumentQueue.Document.Length, relatedDocument.FileName, relatedDocument.FileName);

                DocumentUploadRequest request = new DocumentUploadRequest()
                {
                    File = file,
                    DocumentStatusCode = relatedDocument.DocumentStatusTypeCode,
                    DocumentTypeId = relatedDocument.DocumentTypeId,
                    DocumentTypeMayanId = documentTyp.MayanId,
                    DocumentId = relatedDocument.DocumentId,
                    DocumentMetadata = databaseDocumentQueue.DocumentMetadata != null ? JsonSerializer.Deserialize<List<DocumentMetadataUpdateModel>>(databaseDocumentQueue.DocumentMetadata) : null,
                };
                DocumentUploadResponse response = await _documentService.UploadDocumentAsync(request);

                if (response.DocumentExternalResponse.Status != ExternalResponseStatus.Success || response?.DocumentExternalResponse?.Payload == null)
                {
                    this.Logger.LogError(
                        "Queued document upload failed {databaseDocumentQueueDocumentQueueId} {databaseDocumentQueueDocumentQueueStatusTypeCode}, {documentExternalResponseStatus}",
                        databaseDocumentQueue.DocumentQueueId,
                        databaseDocumentQueue.DocumentQueueStatusTypeCode,
                        response.DocumentExternalResponse.Status);

                    UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.MAYAN_ERROR);
                    return databaseDocumentQueue;
                }
                response.MetadataExternalResponse.Where(r => r.Status != ExternalResponseStatus.Success).ForEach(r => this.Logger.LogError("url: ${url} status: ${status} message ${message}", r.Payload.Url, r.Status, r.Message)); // Log any metadata errors, but don't fail the upload.

                // Mayan may have already returned a file id from the original upload. If not, this job will remain in the processing state (to be periodically checked for completion in another job).
                if (response.DocumentExternalResponse?.Payload?.FileLatest?.Id != null)
                {
                    UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.SUCCESS);
                }
            }
            catch (Exception ex) when (ex is BadRequestException || ex is KeyNotFoundException || ex is InvalidDataException || ex is JsonException)
            {
                this.Logger.LogError($"Error: {ex.Message}");
                UpdateDocumentQueueStatus(databaseDocumentQueue, DocumentQueueStatusTypes.PIMS_ERROR);
            }
            return databaseDocumentQueue;
        }

        /// <summary>
        /// Updates the status of the specified document queue.
        /// </summary>
        /// <param name="documentQueue">The document queue object to update.</param>
        /// <param name="statusType">The new status type to set for the document queue.</param>
        /// <remarks>
        /// This method updates the document queue's status and commits the transaction.
        /// If the status is a final state, it also updates the processing end date.
        /// </remarks>
        private void UpdateDocumentQueueStatus(PimsDocumentQueue documentQueue, DocumentQueueStatusTypes statusType)
        {
            documentQueue.DocumentQueueStatusTypeCode = statusType.ToString();
            bool removeDocument = false;

            // Any final states should update the processing end date.
            if (statusType != DocumentQueueStatusTypes.PROCESSING && statusType != DocumentQueueStatusTypes.PENDING)
            {
                documentQueue.DocProcessEndDt = DateTime.UtcNow;
                if (statusType == DocumentQueueStatusTypes.SUCCESS)
                {
                    documentQueue.Document = null;
                    removeDocument = true;
                }
            }
            _documentQueueRepository.Update(documentQueue, removeDocument);
            _documentQueueRepository.CommitTransaction();
        }


        /// <summary>
        /// Validates the queued document against the database document queue.
        /// </summary>
        /// <param name="databaseDocumentQueue">The document queue object from the database.</param>
        /// <param name="externalDocument">The document queue object to validate against the database.</param>
        /// <returns>True if the queued document is valid; otherwise, false.</returns>
        /// <remarks>
        /// This method checks if the status type, process retries, and document content are valid.
        /// It also ensures that at least one file document ID is associated with the document.
        /// </remarks>
        private bool ValidateQueuedDocument(PimsDocumentQueue databaseDocumentQueue, PimsDocumentQueue externalDocument)
        {
            if (databaseDocumentQueue.DocumentQueueStatusTypeCode != externalDocument.DocumentQueueStatusTypeCode)
            {
                this.Logger.LogError("Requested document queue status: {documentQueueStatusTypeCode} does not match current database status: {documentQueueStatusTypeCode}", externalDocument.DocumentQueueStatusTypeCode, databaseDocumentQueue.DocumentQueueStatusTypeCode);
                return false;
            }
            else if (databaseDocumentQueue.DocProcessRetries != externalDocument.DocProcessRetries)
            {
                this.Logger.LogError("Requested document retries: {documentQueueStatusTypeCode} does not match current database retries: {documentQueueStatusTypeCode}", externalDocument.DocumentQueueStatusTypeCode, databaseDocumentQueue.DocumentQueueStatusTypeCode);
                return false;
            }
            else if (databaseDocumentQueue.Document == null || databaseDocumentQueue.DocumentId == null)
            {
                this.Logger.LogError("Queued document file content is empty, unable to upload.");
                return false;
            }
            return true;
        }
    }
}
