using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Core.Api.Repositories.Rest;

namespace Pims.Scheduler.Repositories
{
    public abstract class PimsBaseRepository : BaseRestRepository
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="PimsBaseRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="jsonOptions">Injected app-wide json options.</param>
        protected PimsBaseRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IOptions<JsonSerializerOptions> jsonOptions)
            : base(logger, httpClientFactory, jsonOptions)
        {
        }

        public override void AddAuthentication(HttpClient client, string authenticationToken = null)
        {
            if (!string.IsNullOrEmpty(authenticationToken))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken.Split(" ")[1]);
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
