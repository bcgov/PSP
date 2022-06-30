using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Config;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanMetadataRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanMetadataRepository : IEdmsMetadataRepository
    {
        private const string MayanConfigSectionKey = "Mayan";
        private readonly ILogger _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IEdmsAuthRepository _authRepository;
        private readonly MayanConfig _config;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanMetadataRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        public MayanMetadataRepository(
            ILogger<MayanMetadataRepository> logger,
            IHttpClientFactory httpClientFactory,
            IEdmsAuthRepository authRepository,
            IConfiguration configuration)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
            _authRepository = authRepository;
            _config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, this._config);
        }

        public async Task<ExternalResult<QueryResult<MetadataType>>> GetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<QueryResult<MetadataType>> retrieveResult = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Retrieving metadata types...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(MediaTypeNames.Application.Json));

            Dictionary<string, string> queryParams = new();

            if (!string.IsNullOrEmpty(ordering))
            {
                queryParams["ordering"] = ordering;
            }

            if (page.HasValue)
            {
                queryParams["page"] = page.ToString();
            }

            if (pageSize.HasValue)
            {
                queryParams["page_size"] = pageSize.ToString();
            }

            try
            {
                string endpointString = $"{this._config.BaseUri}/metadata_types/";
                Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));
                HttpResponseMessage response = await client.GetAsync(endpoint).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.OK:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        QueryResult<MetadataType> metadataTypesResult = JsonSerializer.Deserialize<QueryResult<MetadataType>>(payload);
                        if (metadataTypesResult != null)
                        {
                            retrieveResult.Status = ExternalResultStatus.Success;
                            retrieveResult.Payload = metadataTypesResult;
                        }
                        else
                        {
                            retrieveResult.Status = ExternalResultStatus.Error;
                            retrieveResult.Message = "The response is empty";
                        }

                        break;
                    case HttpStatusCode.NoContent:
                        retrieveResult.Status = ExternalResultStatus.Success;
                        retrieveResult.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        retrieveResult.Status = ExternalResultStatus.Error;
                        retrieveResult.Message = "Forbidden";
                        break;
                    default:
                        retrieveResult.Status = ExternalResultStatus.Error;
                        retrieveResult.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                retrieveResult.Status = ExternalResultStatus.Error;
                retrieveResult.Message = "Exception retrieving metadata types";
                this._logger.LogError("Unexpected exception retrieving metadata types {e}", e);
            }

            this._logger.LogDebug("Finished retrieving metadata types");
            return retrieveResult;
        }

        public async Task<ExternalResult<MetadataType>> CreateMetadataTypeAsync(MetadataType metadataType)
        {
            string authenticationToken = await _authRepository.GetTokenAsync();

            ExternalResult<MetadataType> createMetadataResult = new()
            {
                Status = ExternalResultStatus.Error,
            };

            _logger.LogDebug("Creating metadata type...");
            using HttpClient client = _httpClientFactory.CreateClient("Pims.Api.Logging");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);

            try
            {
                JsonSerializerOptions serializerOptions = new()
                {
                    IgnoreNullValues = true
                };
                string serializedMetadataType = JsonSerializer.Serialize(metadataType, serializerOptions);

                using HttpContent content = new StringContent(serializedMetadataType);
                content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                Uri endpoint = new($"{this._config.BaseUri}/metadata_types/");

                HttpResponseMessage response = await client.PostAsync(endpoint, content).ConfigureAwait(true);
                string payload = await response.Content.ReadAsStringAsync().ConfigureAwait(true);
                this._logger.LogTrace("Response: {response}", response);
                switch (response.StatusCode)
                {
                    case HttpStatusCode.Created:
                        this._logger.LogTrace("Response payload: {payload}", payload);
                        createMetadataResult.Status = ExternalResultStatus.Success;
                        createMetadataResult.Payload = JsonSerializer.Deserialize<MetadataType>(payload);
                        break;
                    case HttpStatusCode.NoContent:
                        createMetadataResult.Status = ExternalResultStatus.Success;
                        createMetadataResult.Message = "No content found";
                        break;
                    case HttpStatusCode.Forbidden:
                        createMetadataResult.Status = ExternalResultStatus.Error;
                        createMetadataResult.Message = "Forbidden";
                        break;
                    case HttpStatusCode.BadRequest:
                        createMetadataResult.Status = ExternalResultStatus.Error;
                        createMetadataResult.Message = payload;
                        break;
                    default:
                        createMetadataResult.Status = ExternalResultStatus.Error;
                        createMetadataResult.Message = $"Unable to contact endpoint {endpoint}. Http status {response.StatusCode}";
                        break;
                }
            }
            catch (Exception e)
            {
                createMetadataResult.Status = ExternalResultStatus.Error;
                createMetadataResult.Message = "Exception creating a metadata type";
                this._logger.LogError("Unexpected exception creating a metadata_type {e}", e);
            }

            this._logger.LogDebug($"Finished creating a metadata type");
            return createMetadataResult;
        }
    }
}
