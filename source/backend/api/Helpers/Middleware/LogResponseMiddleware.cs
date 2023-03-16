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
    /// LogResponseMiddleware class, provides a way to log responses to requests to the API.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LogResponseMiddleware
    {
        #region Variables
        private readonly RequestDelegate _next;
        private readonly ILogger<LogResponseMiddleware> _logger;
        private readonly RecyclableMemoryStreamManager _recyclableMemoryStreamManager;
        private readonly int maxStreamLength = 2000;
        #endregion

        #region Constructors

        /// <summary>
        /// Creates a new instance of an LogResponseMiddleware class, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="next"></param>
        /// <param name="logger"></param>
        public LogResponseMiddleware(RequestDelegate next, ILogger<LogResponseMiddleware> logger)
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
            await LogResponse(context);
        }

        /// <summary>
        /// Log the response to the logging library.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task LogResponse(HttpContext context)
        {
            var originalBodyStream = context.Response.Body;
            await using var responseBody = _recyclableMemoryStreamManager.GetStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            using var reader = new StreamReader(context.Response.Body);
            string body = null;
            if (reader.BaseStream.Length < maxStreamLength)
            {
                body = await reader.ReadToEndAsync();
            }
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            using (_logger.BeginScope("HTTP Response"))
            {
                if (!Log.IsEnabled(Serilog.Events.LogEventLevel.Debug))
                {
                    _logger.LogInformation($"HTTP Response {context.Request.Method} user:{context.User.GetDisplayName()} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}{System.Environment.NewLine}");
                }
                _logger.LogDebug($"HTTP Response {context.Request.Method} user:{context.User.GetDisplayName()} {context.Request.Scheme}://{context.Request.Host}{context.Request.Path}{context.Request.QueryString}");
                _logger.LogTrace("Body: {body}", body);
            }

            await responseBody.CopyToAsync(originalBodyStream);
        }
        #endregion
    }
}
