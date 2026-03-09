using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.Ches;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Requests.Http;
using Polly.Registry;

namespace Pims.Api.Repositories.Ches
{
    /// <summary>
    /// ChesRepository provides email access from the CHES API.
    /// </summary>
    public class ChesRepository : ChesBaseRepository, IEmailRepository
    {
        private readonly HttpClient _client;
        private readonly IEmailAuthRepository _authRepository;
        private readonly JsonSerializerOptions _serializeOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChesRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="config">The injected configuration provider.</param>
        /// <param name="jsonOptions">The jsonOptions.</param>
        /// <param name="pollyPipelineProvider">The polly retry policy.</param>
        public ChesRepository(
            ILogger<ChesRepository> logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration config,
            IEmailAuthRepository authRepository,
            IOptions<JsonSerializerOptions> jsonOptions,
            ResiliencePipelineProvider<string> pollyPipelineProvider)
            : base(logger, httpClientFactory, config, jsonOptions, pollyPipelineProvider)
        {
            _client = httpClientFactory.CreateClient();
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));
            _authRepository = authRepository;

            _serializeOptions = new JsonSerializerOptions(jsonOptions.Value)
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
        }

        public async Task<ExternalResponse<EmailResponse>> SendEmailAsync(EmailRequest request)
        {
            _logger.LogDebug("Sending Email ...");
            ExternalResponse<EmailResponse> result = new ExternalResponse<EmailResponse>()
            {
                Status = ExternalResponseStatus.Error,
            };

            try
            {
                var token = await _authRepository.GetTokenAsync();

                Uri endpoint = new(_config.ChesHost, "/api/v1/email");
                var jsonContent = JsonSerializer.Serialize(request, _serializeOptions);

                using var content = new StringContent(jsonContent);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                using var httpRequest = new HttpRequestMessage(HttpMethod.Post, endpoint);
                httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
                httpRequest.Content = content;

                var response = await _client.SendAsync(httpRequest);
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    result.Status = ExternalResponseStatus.Success;
                    result.Payload = JsonSerializer.Deserialize<EmailResponse>(responseBody);
                    if (result.Payload == null)
                    {
                        result.Status = ExternalResponseStatus.Error;
                        result.Message = "CHES email send succeeded but response payload was null.";
                        _logger.LogError("CHES email send succeeded but response payload was null.");
                    }
                }
                else
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError("CHES email send failed: {Status} {Reason} {Body}", response.StatusCode, response.ReasonPhrase, errorBody);
                    result.Message = $"CHES email send failed: {response.StatusCode} {response.ReasonPhrase}. Response body: {errorBody}";
                }
            }
            catch (Exception ex)
            {
                result.Status = ExternalResponseStatus.Error;
                result.Message = $"Exception sending CHES email: {ex.Message}";
                result.Payload = null;
                result.HttpStatusCode = System.Net.HttpStatusCode.InternalServerError;
                _logger.LogError(ex, "Exception sending CHES email.");
            }
            _logger.LogDebug($"Finished sending email");
            return result;
        }
    }
}
