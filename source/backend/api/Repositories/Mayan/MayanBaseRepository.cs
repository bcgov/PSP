using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Config;
using Pims.Api.Repositories.Rest;

namespace Pims.Api.Repositories.Mayan
{
    /// <summary>
    /// MayanBaseRepository provides common methods to interact with the Mayan EDMS api.
    /// </summary>
    public abstract class MayanBaseRepository : BaseRestRepository
    {
        protected readonly MayanConfig _config;
        private const string MayanConfigSectionKey = "Mayan";

        /// <summary>
        /// Initializes a new instance of the <see cref="MayanBaseRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        protected MayanBaseRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(logger, httpClientFactory)
        {
            _config = new MayanConfig();
            configuration.Bind(MayanConfigSectionKey, _config);
        }

        public override void AddAuthentication(HttpClient client, string authenticationToken = null)
        {
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Token", authenticationToken);
            }
        }

        protected Dictionary<string, string> GenerateQueryParams(string ordering = "", int? page = null, int? pageSize = null)
        {
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

            return queryParams;
        }
    }
}
