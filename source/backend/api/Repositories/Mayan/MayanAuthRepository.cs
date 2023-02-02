using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanDocumentRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanAuthRepository : MayanBaseRepository, IEdmsAuthRepository
    {
        private string _currentToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanAuthRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public MayanAuthRepository(
            ILogger<MayanDocumentRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(logger, httpClientFactory, configuration)
        {
            _currentToken = string.Empty;
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_currentToken))
            {
                ExternalResult<TokenResult> tokenResult = await TryRequestToken();

                if (tokenResult.Status == ExternalResultStatus.Error)
                {
                    throw new AuthenticationException(tokenResult.Message);
                }
                _currentToken = tokenResult.Payload.Token;
            }

            return _currentToken;
        }

        private async Task<ExternalResult<TokenResult>> TryRequestToken()
        {
            _logger.LogDebug("Getting authentication token...");
            Uri endpoint = new Uri($"{_config.BaseUri}/auth/token/obtain/");
            using StringContent credentials = new(JsonSerializer.Serialize(new TokenRequest
            {
                Username = _config.ConnectionUser,
                Password = _config.ConnectionPassword,
            }), Encoding.UTF8, MediaTypeNames.Application.Json);

            ExternalResult<TokenResult> result = await PostAsync<TokenResult>(endpoint, credentials);
            _logger.LogDebug("Finished getting authentication token");

            return result;
        }
    }
}
