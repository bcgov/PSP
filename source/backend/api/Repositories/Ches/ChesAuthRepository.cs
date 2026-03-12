using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Common;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Exceptions;
using Polly.Registry;

namespace Pims.Api.Repositories.Ches.Auth
{

    public class ChesAuthRepository : ChesBaseRepository, IEmailAuthRepository
    {
        private JwtResponse _currentToken;
        private DateTime _lastSuccessfulRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChesAuthRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        /// <param name="jsonOptions">The jsonOptions.</param>
        /// <param name="pollyPipelineProvider">The polly retry policy.</param>
        public ChesAuthRepository(
            ILogger<ChesAuthRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IOptions<JsonSerializerOptions> jsonOptions,
            ResiliencePipelineProvider<string> pollyPipelineProvider)
            : base(logger, httpClientFactory, configuration, jsonOptions, pollyPipelineProvider)
        {
            _currentToken = null;
            _lastSuccessfulRequest = DateTime.UnixEpoch;
        }

        public async Task<string> GetTokenAsync()
        {
            if (!IsValidToken())
            {
                ExternalResponse<JwtResponse> tokenResult = await TryRequestToken();
                if (tokenResult.Status == ExternalResponseStatus.Error)
                {
                    throw new AuthenticationException(tokenResult.Message);
                }

                _lastSuccessfulRequest = DateTime.UtcNow;
                _currentToken = tokenResult.Payload;
            }

            return _currentToken.AccessToken;
        }

        private bool IsValidToken()
        {
            if (_currentToken != null)
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan delta = now - _lastSuccessfulRequest;
                if (delta.TotalSeconds >= _currentToken.ExpiresIn)
                {
                    // Revoke token
                    _logger.LogDebug("Authentication Token has expired.");
                    _currentToken = null;
                    return false;
                }
                return true;
            }

            return false;
        }

        private async Task<ExternalResponse<JwtResponse>> TryRequestToken()
        {
            _logger.LogDebug("Getting authentication token...");

            var requestForm = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", _config.ServiceClientId },
                    { "client_secret", _config.ServiceClientSecret },
                };

            using FormUrlEncodedContent content = new(requestForm);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            ExternalResponse<JwtResponse> result = await PostAsync<JwtResponse>(_config.AuthEndpoint, content);
            _logger.LogDebug("Token endpoint response: {@Result}", result);

            return result;
        }
    }
}
