using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.IO;
using Pims.Core.Extensions;
using Serilog;

namespace Pims.Api.Helpers.Middleware
{
    /// <summary>
    /// LogRequestMiddleware class, provides a way to log requests inbound to the API.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LogRequestMiddleware
    {
        #region Variables
        private readonly RequestDelegate _next;
        private readonly ILogger<LogRequestMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly int maxStreamLength = 2000;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an LogRequestMiddleware class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public LogRequestMiddleware(RequestDelegate next, ILogger<LogRequestMiddleware> logger)
        {
            _next = next;
            _logger = logger;
            _recyclableMemoryStreamManager = new RecyclableMemoryStreamManager();
        }
        #endregion

        #region Methods

        /// <summary>
        /// Add a log message for the request.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            context.Request.EnableBuffering();
            await using var requestStream = _recyclableMemoryStreamManager.GetStream();
            await context.Request.Body.CopyToAsync(requestStream);

            string body = null;
            requestStream.Position = 0;
            using (var streamReader = new StreamReader(requestStream))
            {
                if (requestStream.Length < maxStreamLength)
                {
                    body = streamReader.ReadToEnd();
                }
            }

            using (_logger.BeginScope("HTTP Request"))
            {
                if (!Log.IsEnabled(Serilog.Events.LogEventLevel.Debug))
                {
                    _logger.LogInformation($"HTTP Request {context.Request.Method} user:{context.User.GetDisplayName()} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}");
                }

                _logger.LogDebug("HTTP Request {context.Request.Method} user:{context.User.GetDisplayName()} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}", context.Request.Method, context.User.GetDisplayName(), context.Request.Scheme, context.Request.Host, context.Request.Path, context.Request.QueryString);
                _logger.LogTrace("Body: {{body}}", body);
            }

            context.Request.Body.Position = 0;

            await _next(context);
        }

        #endregion
    }
}
