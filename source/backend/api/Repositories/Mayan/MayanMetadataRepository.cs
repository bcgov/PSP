using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models;
using Pims.Api.Models.Mayan;
using Pims.Api.Models.Mayan.Metadata;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanMetadataRepository provides document access from a Mayan EDMS api.
    /// </summary>
    public class MayanMetadataRepository : MayanBaseRepository, IEdmsMetadataRepository
    {
        private readonly IEdmsAuthRepository _authRepository;

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
            : base(logger, httpClientFactory, configuration)
        {
            _authRepository = authRepository;
        }

        public async Task<ExternalResult<QueryResult<MetadataType>>> TryGetMetadataTypesAsync(string ordering = "", int? page = null, int? pageSize = null)
        {
            _logger.LogDebug("Retrieving metadata types...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            var queryParams = GenerateQueryParams(ordering, page, pageSize);
            string endpointString = $"{_config.BaseUri}/metadata_types/";
            Uri endpoint = new(QueryHelpers.AddQueryString(endpointString, queryParams));

            var response = await GetAsync<QueryResult<MetadataType>>(endpoint, authenticationToken).ConfigureAwait(true);

            _logger.LogDebug("Finished retrieving metadata types");
            return response;
        }

        public async Task<ExternalResult<MetadataType>> TryCreateMetadataTypeAsync(MetadataType metadataType)
        {
            _logger.LogDebug("Creating metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            JsonSerializerOptions serializerOptions = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            string serializedMetadataType = JsonSerializer.Serialize(metadataType, serializerOptions);
            using HttpContent content = new StringContent(serializedMetadataType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/metadata_types/");

            var response = await PostAsync<MetadataType>(endpoint, content, authenticationToken).ConfigureAwait(true);

            this._logger.LogDebug($"Finished creating a metadata type");
            return response;
        }

        public async Task<ExternalResult<MetadataType>> TryUpdateMetadataTypeAsync(MetadataType metadataType)
        {
            _logger.LogDebug("Updating metadata type {id}...", metadataType.Id);

            string authenticationToken = await _authRepository.GetTokenAsync();

            JsonSerializerOptions serializerOptions = new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            };
            string serializedMetadataType = JsonSerializer.Serialize(metadataType, serializerOptions);
            using HttpContent content = new StringContent(serializedMetadataType);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            Uri endpoint = new($"{this._config.BaseUri}/metadata_types/{metadataType.Id}/");

            var response = await PutAsync<MetadataType>(endpoint, content, authenticationToken).ConfigureAwait(true);

            this._logger.LogDebug($"Finished updating a metadata type", metadataType.Id);
            return response;
        }

        public async Task<ExternalResult<string>> TryDeleteMetadataTypeAsync(long metadataTypeId)
        {
            _logger.LogDebug("Deleting metadata type...");

            string authenticationToken = await _authRepository.GetTokenAsync();

            Uri endpoint = new($"{this._config.BaseUri}/metadata_types/{metadataTypeId}/");

            var response = await DeleteAsync(endpoint, authenticationToken);

            _logger.LogDebug($"Finished deleting metadata type");
            return response;
        }
    }
}
