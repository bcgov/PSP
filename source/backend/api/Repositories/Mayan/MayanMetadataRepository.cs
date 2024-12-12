using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Metadata;
using Pims.Api.Models.Requests.Http;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanMetadataRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanMetadataRepository : MayanBaseRepository, IEdmsMetadataRepository
    {
        private static readonly JsonSerializerOptions SerializerOptions = new() { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull };
        private readonly IEdmsAuthRepository _authRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanMetadataRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="authRepository">Injected repository that handles authentication.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        /// <param name="jsonOptions">The json options.</param>
        public MayanMetadataRepository(
            ILogger<MayanMetadataRepository> logger,
            IHttpClientFactory httpClientFactory,
            IEdmsAuthRepository authRepository,
            IConfiguration configuration,
            IOptions<JsonSerializerOptions> jsonOptions)
            : base(logger, httpClientFactory, configuration, jsonOptions)
        {
            _authRepository = authRepository;
        }

        public async Task<ExternalResponse<QueryResponse<MetadataTypeModel>>> TryGetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving metadata types...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);
            string endpointString = $"{_config.BaseUri}/metadata_types/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));

            var response = await GetAsync<QueryResponse<MetadataTypeModel>>(endpoint, authenticationToken).ConfigureAwait(true);
            _logger.LogDebug("Finished retrieving metadata types");

            return response;
        }

        public async Task<ExternalResponse<MetadataTypeModel>> TryCreateMetadataTypeAsync(MetadataTypeModel metadataType)
        {
            _logger.LogDebug("Creating metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();
            string serializedMetadataType = JsonSerializer.Serialize(metadataType, SerializerOptions);
            using HttpContent content = new StringContent(serializedMetadataType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{_config.BaseUri}/metadata_types/");

            var response = await PostAsync<MetadataTypeModel>(endpoint, content, authenticationToken).ConfigureAwait(true);
            _logger.LogDebug($"Finished creating a metadata type");

            return response;
        }

        public async Task<ExternalResponse<MetadataTypeModel>> TryUpdateMetadataTypeAsync(MetadataTypeModel metadataType)
        {
            _logger.LogDebug("Updating metadata type {id}...", metadataType.Id);

            string authenticationToken = await _authRepository.GetTokenAsync();
            string serializedMetadataType = JsonSerializer.Serialize(metadataType, SerializerOptions);
            using HttpContent content = new StringContent(serializedMetadataType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{_config.BaseUri}/metadata_types/{metadataType.Id}/");

            var response = await PutAsync<MetadataTypeModel>(endpoint, content, authenticationToken).ConfigureAwait(true);
            _logger.LogDebug("Finished updating a metadata type {id}...", metadataType.Id);

            return response;
        }

        public async Task<ExternalResponse<string>> TryDeleteMetadataTypeAsync(long metadataTypeId)
        {
            _logger.LogDebug("Deleting metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();
            Uri endpoint = new($"{_config.BaseUri}/metadata_types/{metadataTypeId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);
            _logger.LogDebug("Finished deleting metadata type");

            return response;
        }
    }
}
