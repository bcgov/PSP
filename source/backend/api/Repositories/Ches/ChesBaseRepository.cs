using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pims.Api.Models.Config;
using Pims.Core.Api.Repositories.Rest;
using Polly.Registry;

namespace Pims.Api.Repositories.Ches
{
    /// <summary>
    /// ChesBaseRepository provides common methods to interact with the Common Health Email Service (CHES) api.
    /// </summary>
    public abstract class ChesBaseRepository : BaseRestRepository
    {
        protected readonly ChesConfig _config;
        private const string ChesConfigSectionKey = "Ches";

        /// <summary>
        /// Initializes a new instance of the <see cref="ChesBaseRepository"/> class.
        /// </summary>
        /// <param name="logger">Injected Logger Provider.</param>
        /// <param name="httpClientFactory">Injected Httpclient factory.</param>
        /// <param name="chesConfig">The injected CHES configuration provider.</param>
        /// <param name="jsonOptions">The json options.</param>
        /// <param name="pollyPipelineProvider">The polly retry policy.</param>
        protected ChesBaseRepository(
            ILogger logger,
            IHttpClientFactory httpClientFactory,
            IOptions<ChesConfig> chesConfig,
            IOptions<JsonSerializerOptions> jsonOptions,
            ResiliencePipelineProvider<string> pollyPipelineProvider)
            : base(logger, httpClientFactory, jsonOptions, pollyPipelineProvider)
        {
            _config = chesConfig.Value;
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
