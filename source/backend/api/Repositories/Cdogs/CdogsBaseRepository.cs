using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Pims.Api.Models.Config;
using Pims.Api.Repositories.Rest;

namespace Pims.Api.Repositories.Cdogs
{
    /// <summary>
    /// CdogsBaseRepository provides common methods to interact with the Common Document Generation Service (CDOGS) api.
    /// </summary>
    public abstract class CdogsBaseRepository : BaseRestRepository
    {
        protected readonly CdogsConfig _config;
        private const string CdogsConfigSectionKey = "Cdogs";

        /// <summary>
        /// Initializes a new instance of the <see cref="CdogsBaseRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="configuration">The injected configuration provider.</param>
        protected CdogsBaseRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IConfiguration configuration)
            : base(logger, httpClientFactory)
        {
            _config = new CdogsConfig();
            configuration.Bind(CdogsConfigSectionKey, _config);
        }

        public override void AddAuthentication(HttpClient client, string authenticationToken = null)
        {
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken);
            }
        }
    }
}
