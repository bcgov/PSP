using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.Concepts.Notification;
using Pims.Api.Models.Models.Concepts.Notification;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Extensions;
using Pims.Core.Http;
using Pims.Scheduler.Http.Configuration;
using Polly.Registry;

namespace Pims.Scheduler.Repositories
{
    public class PimsNotificationUserRepository : PimsBaseRepository, IPimsNotificationUserRepository
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };

        private readonly IOpenIdConnectRequestClient _authRepository;
        private readonly IOptionsMonitor<PimsOptions> _configuration;

        public PimsNotificationUserRepository(
            ILogger<PimsDocumentQueueRepository> logger,
            IHttpClientFactory httpClientFactory,
            IOpenIdConnectRequestClient authRepository,
            IOptionsMonitor<PimsOptions> configuration,
            IOptions<JsonSerializerOptions> jsonOptions,
            ResiliencePipelineProvider<string> pollyPipelineProvider)
            : base(logger, httpClientFactory, jsonOptions, pollyPipelineProvider)
        {
            _authRepository = authRepository;
            _configuration = configuration;
        }

        public async Task<ExternalResponse<List<NotificationUserOutputModel>>> SearchUserNotificationsAsync(NotificationUserSearchFilterModel filter)
        {
            _logger.LogDebug("Getting filtered list of  {filter}", filter);

            string authenticationToken = await _authRepository.RequestAccessToken();
            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/user-notifications/search");
            string serializedFilter = JsonSerializer.Serialize(filter, SerializerOptions);
            using var content = new StringContent(serializedFilter, Encoding.UTF8, "application/json");

            var response = await PostAsync<List<NotificationUserOutputModel>>(endpoint, content, authenticationToken);
            _logger.LogDebug("Retrieved list of user notifications based on {filter}, {response} ", filter.Serialize(), response.Serialize());

            return response;
        }

        public async Task<ExternalResponse<NotificationUserOutputModel>> PushUserNotificationsAsync(NotificationUserOutputModel userNotification)
        {
            _logger.LogDebug("Pushing notification {userNotification}", userNotification);

            string authenticationToken = await _authRepository.RequestAccessToken();
            Uri endpoint = new($"{_configuration.CurrentValue.Uri}/user-notifications/{userNotification.NotificationUserOutputId}/push");

            var response = await PutAsync<NotificationUserOutputModel>(endpoint, null, authenticationToken);
            _logger.LogDebug("Received response for pushing notification with Id: {NotificationUserOutputId} with code: {HttpStatusCode} ", userNotification.NotificationUserOutputId, response.HttpStatusCode);

            return response;
        }
    }
}
