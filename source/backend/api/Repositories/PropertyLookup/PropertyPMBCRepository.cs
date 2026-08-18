using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Concepts.Property;
using Pims.Core.Helpers;

namespace Pims.Api.Repositories.PropertyLookup
{
    public class PropertyPmbcRepository : IPropertyPmbcRepository
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PropertyPmbcRepository> _logger;
        private readonly string _pmbcExternalApiUrl;

        private static string BuildGeneralLocation(string municipality, string regionalDistrict)
        {
            if (string.IsNullOrWhiteSpace(municipality))
            {
                return regionalDistrict;
            }

            if (string.IsNullOrWhiteSpace(regionalDistrict))
            {
                return municipality;
            }

            return $"{municipality}, {regionalDistrict}";
        }

        private static PropertyModel MapToPropertyModel(PmbcProperties properties, int parsedPid)
        {
            return new PropertyModel
            {
                Id = 0,
                Pid = parsedPid,
                Pin = properties.Pin,
                PlanNumber = properties.PlanNumber,
                GeneralLocation = BuildGeneralLocation(
                    properties.Municipality,
                    properties.RegionalDistrict),
            };
        }

        public PropertyPmbcRepository(
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration,
            ILogger<PropertyPmbcRepository> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _pmbcExternalApiUrl = configuration.GetValue<string>("HealthChecks:PmbcExternalApi:Url");
        }

        public async Task<PropertyModel> GetByPidAsync(string pid)
        {
            var parsedPid = PidTranslator.ConvertPID(pid);
            if (parsedPid <= 0)
            {
                return null;
            }

            var normalizedPid = parsedPid.ToString("D9");
            var feature = await QuerySingleFeatureAsync($"PID = '{normalizedPid}'", normalizedPid);
            return feature?.Properties == null ? null : MapToPropertyModel(feature.Properties, parsedPid);
        }

        private async Task<PmbcFeature> QuerySingleFeatureAsync(string cqlFilter, string lookupValue)
        {
            var url = BuildLookupUrl(cqlFilter);
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogWarning("PMBC lookup URL is not configured. Set HealthChecks:PmbcExternalApi:Url.");
                return null;
            }

            try
            {
                using var client = _httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("PMBC lookup failed searching by {LookupValue}. Status {StatusCode}", lookupValue, response.StatusCode);
                    return null;
                }

                var json = await response.Content.ReadAsStringAsync();
                var contentType = response.Content.Headers.ContentType?.MediaType;
                if (string.IsNullOrWhiteSpace(json) || json.TrimStart().StartsWith('<'))
                {
                    _logger.LogWarning(
                        "PMBC lookup returned non-JSON payload searching by {LookupValue}. ContentType: {ContentType}. PayloadPreview: {PayloadPreview}",
                        lookupValue,
                        contentType,
                        json?.Length > 300 ? json.Substring(0, 300) : json);
                    return null;
                }

                var featureCollection = JsonSerializer.Deserialize<PmbcFeatureCollection>(json);
                return featureCollection?.Features != null && featureCollection.Features.Count > 0
                    ? featureCollection.Features[0]
                    : null;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogWarning(ex, "PMBC lookup HTTP request failed for value {LookupValue}", lookupValue);
                return null;
            }
            catch (TaskCanceledException ex)
            {
                _logger.LogWarning(ex, "PMBC lookup timed out or was canceled for value {LookupValue}", lookupValue);
                return null;
            }
            catch (JsonException ex)
            {
                _logger.LogWarning(ex, "PMBC lookup returned invalid JSON for value {LookupValue}", lookupValue);
                return null;
            }
        }

        private string BuildLookupUrl(string cqlFilter)
        {
            if (string.IsNullOrWhiteSpace(_pmbcExternalApiUrl))
            {
                return null;
            }

            return QueryHelpers.AddQueryString(
                _pmbcExternalApiUrl,
                new Dictionary<string, string>
                {
                    ["cql_filter"] = cqlFilter,
                    ["count"] = "1",
                });
        }

        private sealed class PmbcFeatureCollection
        {
            [JsonPropertyName("features")]
            public List<PmbcFeature> Features { get; set; }
        }

        private sealed class PmbcFeature
        {
            [JsonPropertyName("properties")]
            public PmbcProperties Properties { get; set; }
        }

        private sealed class PmbcProperties
        {
            [JsonPropertyName("PID")]
            public string Pid { get; set; }

            [JsonPropertyName("PIN")]
            public int? Pin { get; set; }

            [JsonPropertyName("PLAN_NUMBER")]
            public string PlanNumber { get; set; }

            [JsonPropertyName("MUNICIPALITY")]
            public string Municipality { get; set; }

            [JsonPropertyName("REGIONAL_DISTRICT")]
            public string RegionalDistrict { get; set; }
        }
    }
}
