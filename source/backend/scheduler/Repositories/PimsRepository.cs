using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.Concepts.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Http;
using Pims.Dal.Entities.Models;
using Pims.Scheduler.Http.Configuration;

namespace Pims.Scheduler.Repositories.Pims
{
    /// <summary>
    /// PimsDocumentQueueRepository provides document access from the PIMS document queue api.
    /// </summary>
    public class PimsDocumentQueueRepository : PimsBaseRepository, IPimsDocumentQueueRepository
    {
        private readonly IOpenIdConnectRequestClient _authRepository;
        private readonly IOptionsMonitor<PimsOptions> _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="PimsDocumentQueueRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public PimsDocumentQueueRepository(
            ILogger<PimsDocumentQueueRepository> logger,
            IHttpClientFactory httpClientFactory,
            IOpenIdConnectRequestClient authRepository,
            IOptionsMonitor<PimsOptions> configuration)
            : base(logger, httpClientFactory)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<ExternalResponse<List<DocumentQueueModel>>> SearchQueuedDocumentsAsync(DocumentQueueFilter filter)
        {
            _logger.LogDebug("Getting filtered list of queued documents by {filter}", filter);

            string authenticationToken = await _authRepository.RequestAccessToken();

            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/documents/queue/search");

            var response = await GetAsync<List<DocumentQueueModel>>(endpoint, authenticationToken);
            _logger.LogDebug($"Retrieved list of queued documents based on {filter} ", filter);

            return response;
        }
    }
}
