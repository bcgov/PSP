using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Extensions;
using Pims.Core.Http;
using Pims.Dal.Entities.Models;
using Pims.Scheduler.Http.Configuration;

namespace Pims.Scheduler.Repositories
{
    /// <summary>
    /// PimsDocumentQueueRepository provides document access from the PIMS document queue api.
    /// </summary>
    public class PimsDocumentQueueRepository : PimsBaseRepository, IPimsDocumentQueueRepository
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        private readonly IOpenIdConnectRequestClient _authRepository;
        private readonly IOptionsMonitor<PimsOptions> _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PimsDocumentQueueRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        /// <param name="jsonOptions">The injected json options.</param>
        public PimsDocumentQueueRepository(
            ILogger<PimsDocumentQueueRepository> logger,
            IHttpClientFactory httpClientFactory,
            IOpenIdConnectRequestClient authRepository,
            IOptionsMonitor<PimsOptions> configuration,
            IOptions<JsonSerializerOptions> jsonOptions)
            : base(logger, httpClientFactory, jsonOptions)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Polls the upload status in mayan of a queued document using the provided document model.
        /// </summary>
        /// <param name="document">The document to poll.</param>
        /// <returns>A task that represents the asynchronous operation. The result is an external response containing the document queue model and status information.</returns>
        public async Task<ExternalResponse<DocumentQueueModel>> PollQueuedDocument(DocumentQueueModel document)
        {
            _logger.LogDebug("polling queued document with id {documentId}", document.Id);

            string authenticationToken = await _authRepository.RequestAccessToken();

            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/documents/queue/{document.Id}/poll");

            string serializedFilter = JsonSerializer.Serialize(document, SerializerOptions);
            using var content = new StringContent(serializedFilter, Encoding.UTF8, "application/json");

            var response = await PostAsync<DocumentQueueModel>(endpoint, content, authenticationToken);
            _logger.LogDebug("queued document poll for document with id {documentId} complete with status: {response}", document.Id, response.Serialize());

            return response;
        }

        /// <summary>
        /// Uploads a queued document to the specified endpoint.
        /// </summary>
        /// <param name="document">The document queue model containing the document details.</param>
        /// <returns>A task that represents the asynchronous operation and returns an external response containing the status of the upload.</returns>
        public async Task<ExternalResponse<DocumentQueueModel>> UploadQueuedDocument(DocumentQueueModel document)
        {
            _logger.LogDebug("uploading queued document with id {documentId}", document.Id);

            string authenticationToken = await _authRepository.RequestAccessToken();

            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/documents/queue/{document.Id}/upload");

            string serializedFilter = JsonSerializer.Serialize(document, SerializerOptions);
            using var content = new StringContent(serializedFilter, Encoding.UTF8, "application/json");

            var response = await PostAsync<DocumentQueueModel>(endpoint, content, authenticationToken);
            _logger.LogDebug("queued document upload for document with id {documentId} complete with status: {response}", document.Id, response.Serialize());

            return response;
        }

        /// <summary>
        /// Updates an existing queued document.
        /// </summary>
        /// <param name="documentQueueId">The ID of the document to update.</param>
        /// <param name="document">The updated document details.</param>
        /// <returns>The result of the update operation.</returns>
        public async Task<ExternalResponse<DocumentQueueModel>> UpdateQueuedDocument(long documentQueueId, DocumentQueueModel document)
        {
            _logger.LogDebug("updating queued document with id {documentId}", documentQueueId);

            string authenticationToken = await _authRepository.RequestAccessToken();

            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/documents/queue/{documentQueueId}");

            string serializedFilter = JsonSerializer.Serialize(document, SerializerOptions);
            using var content = new StringContent(serializedFilter, Encoding.UTF8, "application/json");

            var response = await PutAsync<DocumentQueueModel>(endpoint, content, authenticationToken);
            _logger.LogDebug("queued document update for document with id {documentId} complete with {response}", documentQueueId, response.Serialize());

            return response;
        }

        /// <summary>
        /// Updates an existing queued document.
        /// </summary>
        /// <param name="documentQueueId">The ID of the document to update.</param>
        /// <returns>The result of the update operation.</returns>
        public async Task<ExternalResponse<DocumentQueueModel>> GetById(long documentQueueId)
        {
            _logger.LogDebug("getting queued document with id {documentId}", documentQueueId);

            string authenticationToken = await _authRepository.RequestAccessToken();

            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/documents/queue/{documentQueueId}");

            var response = await GetAsync<DocumentQueueModel>(endpoint, authenticationToken);
            _logger.LogDebug("queued document retrieval for document with id {documentId} complete with {response}", documentQueueId, response.Serialize());

            return response;
        }

        /// <summary>
        /// Searches for queued documents based on the provided filter.
        /// </summary>
        /// <param name="filter">The filter to apply to the search.</param>
        /// <returns>A task that represents the asynchronous operation, returning a list of document queue models.</returns>
        public async Task<ExternalResponse<List<DocumentQueueModel>>> SearchQueuedDocumentsAsync(DocumentQueueFilter filter)
        {
            _logger.LogDebug("Getting filtered list of queued documents by {filter}", filter);

            string authenticationToken = await _authRepository.RequestAccessToken();

            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/documents/queue/search");

            string serializedFilter = JsonSerializer.Serialize(filter, SerializerOptions);
            using var content = new StringContent(serializedFilter, Encoding.UTF8, "application/json");

            var response = await PostAsync<List<DocumentQueueModel>>(endpoint, content, authenticationToken);
            _logger.LogDebug("Retrieved list of queued documents based on {filter}, {response} ", filter.Serialize(), response.Serialize());

            return response;
        }
    }
}
