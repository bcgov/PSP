using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Helpers.Middleware;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Dal.Exceptions;
using Xunit;

namespace Pims.Api.Test.Middleware
{
    public class ErrorHandlingMiddlewareTest
    {
        private readonly Mock<IWebHostEnvironment> _env = new();
        private readonly Mock<ILogger<ErrorHandlingMiddleware>> _logger = new();
        private readonly IOptions<JsonOptions> _options = Options.Create(new JsonOptions());

        public ErrorHandlingMiddlewareTest()
        {
            // IsDevelopment() is an extension method that reads EnvironmentName
            _env.SetupGet(e => e.EnvironmentName).Returns(Environments.Production);
        }

        private ErrorHandlingMiddleware CreateMiddleware(RequestDelegate next) =>
            new(next, _env.Object, _logger.Object, _options);

        private static DefaultHttpContext CreateContext()
        {
            var context = new DefaultHttpContext();
            context.Response.Body = new MemoryStream();
            return context;
        }

        private static async Task<string> ReadBodyAsync(HttpContext context)
        {
            context.Response.Body.Position = 0;
            using var reader = new StreamReader(context.Response.Body);
            return await reader.ReadToEndAsync();
        }

        [Fact]
        public async Task Invoke_NoException_PassesThrough()
        {
            var context = CreateContext();
            var called = false;
            var middleware = CreateMiddleware(ctx => { called = true; return Task.CompletedTask; });

            await middleware.Invoke(context);

            called.Should().BeTrue();
            context.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
        }

        public static IEnumerable<object[]> ExceptionCases => new List<object[]>
    {
        new object[] { new KeyNotFoundException(), HttpStatusCode.NotFound },
        new object[] { new DbUpdateConcurrencyException(), HttpStatusCode.BadRequest },
        new object[] { new DbUpdateException(), HttpStatusCode.BadRequest },
        new object[] { new NotAuthorizedException(), HttpStatusCode.Forbidden },
        new object[] { new BusinessRuleViolationException("rule"), HttpStatusCode.BadRequest },
        new object[] { new InvalidOperationException("bad"), HttpStatusCode.BadRequest },
        new object[] { new Exception("boom"), HttpStatusCode.InternalServerError },
        new object[] { new LtsaException("LTSA service is not available"), HttpStatusCode.InternalServerError },
        new object[] { new AuthenticationException("auth failed"), HttpStatusCode.Unauthorized },
        new object[] { new UserOverrideException("User override is required. Approve?"), HttpStatusCode.Conflict },
        new object[] { new ConcurrencyControlNumberMissingException(), HttpStatusCode.BadRequest },
    };

        [Theory]
        [MemberData(nameof(ExceptionCases))]
        public async Task Invoke_Exception_MapsToStatusCode(Exception ex, HttpStatusCode expected)
        {
            var context = CreateContext();
            var middleware = CreateMiddleware(_ => throw ex);

            await middleware.Invoke(context);

            context.Response.StatusCode.Should().Be((int)expected);
            context.Response.ContentType.Should().Be("application/json");

            var body = await ReadBodyAsync(context);
            body.Should().NotBeNullOrWhiteSpace();
            using var json = JsonDocument.Parse(body); // asserts it is valid JSON
        }

        [Fact]
        public async Task Invoke_BusinessRuleViolation_ReturnsExceptionMessage()
        {
            var context = CreateContext();
            var middleware = CreateMiddleware(_ => throw new BusinessRuleViolationException("Custom rule message"));

            await middleware.Invoke(context);

            var body = await ReadBodyAsync(context);
            body.Should().Contain("Custom rule message");
        }

        [Fact]
        public async Task Invoke_UnhandledException_LogsError()
        {
            var context = CreateContext();
            var middleware = CreateMiddleware(_ => throw new Exception("boom"));

            await middleware.Invoke(context);

            _logger.Verify(l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.IsAny<It.IsAnyType>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }
    }
}
