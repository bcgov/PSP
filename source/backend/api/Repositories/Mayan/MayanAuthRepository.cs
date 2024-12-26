using System;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Exceptions;

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
        /// <param name="jsonOptions">The jsonOptions.</param>
        public MayanAuthRepository(
            ILogger<MayanDocumentRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            IOptions<JsonSerializerOptions> jsonOptions)
            : base(logger, httpClientFactory, configuration, jsonOptions)
        {
            _currentToken = string.Empty;
        }

        public async Task<string> GetTokenAsync()
        {
            if (string.IsNullOrEmpty(_currentToken))
            {
                ExternalResponse<TokenResponse> tokenResponse = await TryRequestToken();

                if (tokenResponse.Status == ExternalResponseStatus.Error)
                {
                    throw new AuthenticationException(tokenResponse.Message);
                }
                _currentToken = tokenResponse.Payload.Token;
            }

            return _currentToken;
        }

        private async Task<ExternalResponse<TokenResponse>> TryRequestToken()
        {
            _logger.LogDebug("Getting authentication token...");
            Uri endpoint = new Uri($"{_config.BaseUri}/auth/token/obtain/");
            using StringContent credentials = new(JsonSerializer.Serialize(new TokenRequest
            {
                Username = _config.ConnectionUser,
                Password = _config.ConnectionPassword,
            }), Encoding.UTF8, MediaTypeNames.Application.Json);

            ExternalResponse<TokenResponse> result = await PostAsync<TokenResponse>(endpoint, credentials);
            _logger.LogDebug("Finished getting authentication token");

            return result;
        }
    }
}
