using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Pims.Api.Handlers
{
    public class LoggingHandler : DelegatingHandler
    {
        private readonly ILogger _logger;

        public LoggingHandler(ILogger<LoggingHandler> logger)
        {
            _logger = logger;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            _logger.LogTrace("Request:");
            _logger.LogTrace("{request}", request.ToString());
            if (request.Content != null)
            {
                _logger.LogTrace("{cancellationToken}", await request.Content.ReadAsStringAsync(cancellationToken));
            }

            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            _logger.LogTrace("Response:");
            _logger.LogTrace("{response}", response.ToString());
            if (response.Content != null)
            {
                _logger.LogTrace("{cancellationToken}", await response.Content.ReadAsStringAsync(cancellationToken));
            }

            return response;
        }
    }
}
