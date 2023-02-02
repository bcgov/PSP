using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Cdogs;

namespace Pims.Api.Repositories.Cdogs
{
    /// <summary>
    /// CdogsAuthRepository provides authentication to the Cdogs external api.
    /// </summary>
    public class CdogsAuthRepository : CdogsBaseRepository, IDocumentGenerationAuthRepository
    {
        private JwtResponse _currentToken;
        private DateTime _lastSucessfullRequest;

        /// <summary>
        /// Initializes a new instance of the <see cref="CdogsAuthRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public CdogsAuthRepository(
            ILogger<CdogsAuthRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(logger, httpClientFactory, configuration)
        {
            _currentToken = null;
            _lastSucessfullRequest = DateTime.UnixEpoch;
        }

        public async Task<string> GetTokenAsync()
        {
            if (!IsValidToken())
            {
                ExternalResult<JwtResponse> tokenResult = await TryRequestToken();

                if (tokenResult.Status == ExternalResultStatus.Error)
                {
                    throw new AuthenticationException(tokenResult.Message);
                }

                _lastSucessfullRequest = DateTime.UtcNow;
                _currentToken = tokenResult.Payload;
            }

            return _currentToken.AccessToken;
        }

        private bool IsValidToken()
        {
            if (_currentToken != null)
            {
                DateTime now = DateTime.UtcNow;
                TimeSpan delta = now - _lastSucessfullRequest;
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

        private async Task<ExternalResult<JwtResponse>> TryRequestToken()
        {
            _logger.LogDebug("Getting authentication token...");

            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();

            var requestForm = new Dictionary<string, string>
                {
                    { "grant_type", "client_credentials" },
                    { "client_id", _config.ServiceClientId },
                    { "client_secret", _config.ServiceClientSecret },
                };

            using FormUrlEncodedContent content = new(requestForm);
            content.Headers.Clear();
            content.Headers.Add("Content-Type", "application/x-www-form-urlencoded");

            ExternalResult<JwtResponse> result = await PostAsync<JwtResponse>(_config.AuthEndpoint, content);
            _logger.LogDebug("Finished getting authentication token");

            return result;
        }
    }
}
