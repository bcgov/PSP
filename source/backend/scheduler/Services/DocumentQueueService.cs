using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Services;
using Pims.Core.Extensions;
using Pims.Dal.Entities.Models;
using Pims.Scheduler.Http.Configuration;
using Pims.Scheduler.Models;
using Pims.Scheduler.Repositories;

namespace Pims.Scheduler.Services
{
    public class DocumentQueueService : BaseService, IDocumentQueueService
    {
        private readonly ILogger _logger;
        private readonly IPimsDocumentQueueRepository _pimsDocumentQueueRepository;
        private readonly IOptionsMonitor<UploadQueuedDocumentsJobOptions> _uploadQueuedDocumentsJobOptions;
        private readonly IOptionsMonitor<QueryProcessingDocumentsJobOptions> _queryProcessingDocumentsJobOptions;

        public DocumentQueueService(
            ILogger<DocumentQueueService> logger,
            IOptionsMonitor<UploadQueuedDocumentsJobOptions> uploadQueuedDocumentsJobOptions,
            IOptionsMonitor<QueryProcessingDocumentsJobOptions> queryProcessingDocumentsJobOptions,
            IPimsDocumentQueueRepository pimsDocumentQueueRepository)
            : base(null, logger)
        {
            _logger = logger;
            _pimsDocumentQueueRepository = pimsDocumentQueueRepository;
            _uploadQueuedDocumentsJobOptions = uploadQueuedDocumentsJobOptions;
            _queryProcessingDocumentsJobOptions = queryProcessingDocumentsJobOptions;
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public async Task<ScheduledTaskResponseModel> UploadQueuedDocuments()
        {
            var filter = new DocumentQueueFilter() { Quantity = _uploadQueuedDocumentsJobOptions?.CurrentValue?.BatchSize ?? 50, DocumentQueueStatusTypeCodes = new string[] { DocumentQueueStatusTypes.PENDING.ToString() } };
            var searchResponse = await SearchQueuedDocuments(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse?.ScheduledTaskResponseModel;
            }

            IEnumerable<Task<DocumentQueueResponseModel>> responses = searchResponse?.SearchResults?.Payload?.Select(qd =>
            {
                _logger.LogInformation("Uploading Queued document {documentQueueId}", qd.Id);
                _logger.LogDebug("document contents {document}", qd.Serialize());

                return _pimsDocumentQueueRepository.UploadQueuedDocument(qd).ContinueWith(response => HandleDocumentQueueResponse("UploadQueuedDocument", qd, response));
            });
            var results = await Task.WhenAll(responses);
            return new ScheduledTaskResponseModel() { Status = GetMergedStatus(results), DocumentQueueResponses = results };
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public async Task<ScheduledTaskResponseModel> RetryQueuedDocuments()
        {
            var filter = new DocumentQueueFilter()
            {
                Quantity = _uploadQueuedDocumentsJobOptions?.CurrentValue?.BatchSize ?? 50,
                DocumentQueueStatusTypeCodes = new string[] { DocumentQueueStatusTypes.PIMS_ERROR.ToString(), DocumentQueueStatusTypes.MAYAN_ERROR.ToString() },
                MaxDocProcessRetries = 3,
            };
            var searchResponse = await SearchQueuedDocuments(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse?.ScheduledTaskResponseModel;
            }
            IEnumerable<Task<DocumentQueueResponseModel>> responses = searchResponse?.SearchResults?.Payload?.Select(qd =>
            {
                _logger.LogInformation("Uploading Queued document {documentQueueId}", qd.Id);
                _logger.LogDebug("document contents {document}", qd.Serialize());
                return _pimsDocumentQueueRepository.UploadQueuedDocument(qd).ContinueWith(response => HandleDocumentQueueResponse("UploadQueuedDocument", qd, response));
            });
            var results = await Task.WhenAll(responses);
            return new ScheduledTaskResponseModel() { Status = GetMergedStatus(results), DocumentQueueResponses = results };
        }

        [DisableConcurrentExecution(timeoutInSeconds: 10 * 30)]
        public async Task<ScheduledTaskResponseModel> QueryProcessingDocuments()
        {
            var filter = new Dal.Entities.Models.DocumentQueueFilter()
            {
                Quantity = _queryProcessingDocumentsJobOptions?.CurrentValue?.BatchSize ?? 50,
                DocumentQueueStatusTypeCodes = new string[] { DocumentQueueStatusTypes.PROCESSING.ToString() },
            };
            var searchResponse = await SearchQueuedDocuments(filter);
            if (searchResponse?.ScheduledTaskResponseModel != null)
            {
                return searchResponse?.ScheduledTaskResponseModel;
            }

            IEnumerable<Task<DocumentQueueResponseModel>> responses = searchResponse?.SearchResults?.Payload.Select(qd =>
            {
                _logger.LogInformation("Querying for queued document {documentQueueId}", qd.Id);
                _logger.LogDebug("document contents {document}", qd.Serialize());
                if (qd.DocumentProcessStartTimestamp.HasValue && DateTime.UtcNow.Subtract(qd.DocumentProcessStartTimestamp.Value).TotalMinutes > _queryProcessingDocumentsJobOptions?.CurrentValue?.MaxProcessingMinutes)
                {
                    _logger.LogError("Document processing for document {documentQueueId} has exceeded maximum processing time of {maxProcessingMinutes}", qd.Id, _queryProcessingDocumentsJobOptions?.CurrentValue?.MaxProcessingMinutes);
                    qd.DocumentQueueStatusType.Id = DocumentQueueStatusTypes.MAYAN_ERROR.ToString();
                    qd.DocumentProcessEndTimestamp = DateTime.UtcNow;
                    _ = _pimsDocumentQueueRepository.UpdateQueuedDocument(qd.Id, qd).ContinueWith(response =>
                    {
                        _logger.LogInformation("Received response from PIMS document update for queued document {documentQueueId} status {Status} message: {Message}", qd.Id, response?.Result?.Status, response?.Result?.Message);
                    });
                    return Task.FromResult(new DocumentQueueResponseModel() { DocumentQueueStatus = DocumentQueueStatusTypes.PIMS_ERROR, Message = $"Document processing for document {qd.Id} has exceeded maximum processing time of {_queryProcessingDocumentsJobOptions?.CurrentValue?.MaxProcessingMinutes}" });
                }
                else
                {
                    return _pimsDocumentQueueRepository.PollQueuedDocument(qd).ContinueWith(response => HandleDocumentQueueResponse("PollQueuedDocument", qd, response));
                }
            });
            var results = await Task.WhenAll(responses);
            return new ScheduledTaskResponseModel() { Status = GetMergedStatus(results), DocumentQueueResponses = results };
        }

        private static TaskResponseStatusTypes GetMergedStatus(IEnumerable<DocumentQueueResponseModel> responses)
        {
            if (responses.All(r => r.DocumentQueueStatus == DocumentQueueStatusTypes.SUCCESS))
            {
                return TaskResponseStatusTypes.SUCCESS;
            }
            else if (responses.All(r => r.DocumentQueueStatus == DocumentQueueStatusTypes.MAYAN_ERROR || r.DocumentQueueStatus == DocumentQueueStatusTypes.PIMS_ERROR))
            {
                return TaskResponseStatusTypes.ERROR;
            }
            return TaskResponseStatusTypes.PARTIAL;
        }

        private async Task<SearchQueuedDocumentsResponseModel> SearchQueuedDocuments(DocumentQueueFilter filter)
        {
            ScheduledTaskResponseModel scheduledTaskResponseModel = null;
            var queuedDocuments = await _pimsDocumentQueueRepository.SearchQueuedDocumentsAsync(filter);

            if (queuedDocuments?.Status != ExternalResponseStatus.Success)
            {
                _logger.LogError("Received error status from pims document queue search service, aborting. {status} {message}", queuedDocuments.Status, queuedDocuments.Message);
                scheduledTaskResponseModel = new ScheduledTaskResponseModel() { Status = TaskResponseStatusTypes.ERROR, Message = "Received error status from pims document queue service, aborting." };
            }
            if (queuedDocuments?.Payload?.Count == 0)
            {
                _logger.LogInformation("No documents to process, skipping execution.");
                scheduledTaskResponseModel = new ScheduledTaskResponseModel() { Status = TaskResponseStatusTypes.SKIPPED, Message = "No documents to process, skipping execution." };
            }
            return new SearchQueuedDocumentsResponseModel() { ScheduledTaskResponseModel = scheduledTaskResponseModel, SearchResults = queuedDocuments };
        }

        private DocumentQueueResponseModel HandleDocumentQueueResponse(string httpMethodName, DocumentQueueModel qd, Task<ExternalResponse<DocumentQueueModel>> response)
        {
            var responseObject = response?.Result;
            if (responseObject?.Status == ExternalResponseStatus.Success && (responseObject?.Payload?.DocumentQueueStatusType?.Id == DocumentQueueStatusTypes.PROCESSING.ToString() || responseObject?.Payload?.DocumentQueueStatusType?.Id == DocumentQueueStatusTypes.SUCCESS.ToString()))
            {
                _logger.LogInformation("Received response from {httpMethodName} for queued document {documentQueueId} status {Status} message: {Message}", httpMethodName, qd.Id, response?.Result?.Status, response?.Result?.Message);
                return new DocumentQueueResponseModel() { DocumentQueueStatus = DocumentQueueStatusTypes.SUCCESS };
            }
            else if (responseObject?.Payload?.DocumentQueueStatusType?.Id != DocumentQueueStatusTypes.PIMS_ERROR.ToString() && responseObject?.Payload?.DocumentQueueStatusType?.Id != DocumentQueueStatusTypes.MAYAN_ERROR.ToString())
            {
                // If the poll failed, but the document is not in an error state, update the document to an error state.
                _logger.LogError("Received error response from {httpMethodName} for queued document {documentQueueId} status {Status} message: {Message}", httpMethodName, qd.Id, response?.Result?.Status, response?.Result?.Message);
                qd.DocumentQueueStatusType.Id = DocumentQueueStatusTypes.PIMS_ERROR.ToString();
                qd.RowVersion = responseObject?.Payload?.RowVersion ?? qd.RowVersion;
                _ = _pimsDocumentQueueRepository.UpdateQueuedDocument(qd.Id, qd);
                return new DocumentQueueResponseModel() { DocumentQueueStatus = DocumentQueueStatusTypes.PIMS_ERROR, Message = $"Received error response from {httpMethodName} for queued document {qd.Id} status {response?.Result?.Status} message: {response?.Result?.Message}" };
            }
            return new DocumentQueueResponseModel() { DocumentQueueStatus = DocumentQueueStatusTypes.PIMS_ERROR };
        }
    }
}
