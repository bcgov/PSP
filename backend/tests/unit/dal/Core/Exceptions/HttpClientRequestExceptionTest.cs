using FluentAssertions;
using Pims.Core.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Pims.Api.Test.Core.Exceptions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class HttpClientRequestExceptionTest
    {
        #region Tests
        [Fact]
        public void Constructor_Message()
        {
            // Arrange
            var msg = "message";

            // Act
            var exception = new HttpClientRequestException(msg);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
        [Fact]
        public void Constructor_MessageAndStatusCode()
        {
            // Arrange
            var msg = "message";
            var status = HttpStatusCode.Accepted;

            // Act
            var exception = new HttpClientRequestException(msg, status);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

        [Fact]
        public void Constructor_MessageAndInnerException()
        {
            // Arrange
            var msg = "message";
            var inner = new Exception(msg);

            // Act
            var exception = new HttpClientRequestException(msg, inner);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be(msg);
            exception.Response.Should().BeNull();
        }

        [Fact]
        public void Constructor_MessageAndInnerException_WithResponse()
        {
            // Arrange
            var msg = "message";
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com/"),
                Content = new StringContent("test")
            };
            var inner = new HttpClientRequestException(response);

            // Act
            var exception = new HttpClientRequestException(msg, inner);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be($"HTTP Request '{response?.RequestMessage.RequestUri}' failed");
            exception.Response.Should().Be(response);
        }
        #endregion
    }
}
