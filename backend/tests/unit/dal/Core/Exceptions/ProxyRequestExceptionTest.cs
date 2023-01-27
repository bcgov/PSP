using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Pims.Core.Exceptions;
using Xunit;

namespace Pims.Api.Test.Core.Exceptions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class ProxyRequestExceptionTest
    {
        #region Tests
        [Fact]
        public void Constructor_Message()
        {
            // Arrange
            var msg = "message";

            // Act
            var exception = new ProxyRequestException(msg);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Constructor_Message_WithStatusCode()
        {
            // Arrange
            var msg = "message";
            var status = HttpStatusCode.Accepted;

            // Act
            var exception = new ProxyRequestException(msg, status);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.StatusCode.Should().Be(status);
        }

        [Fact]
        public void Constructor_MessageAndInnerException()
        {
            // Arrange
            var msg = "message";
            var inner = new Exception(msg);

            // Act
            var exception = new ProxyRequestException(msg, inner);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be(msg);
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void Constructor_MessageAndInnerException_WithStatusCode()
        {
            // Arrange
            var msg = "message";
            var inner = new Exception(msg);
            var status = HttpStatusCode.Accepted;

            // Act
            var exception = new ProxyRequestException(msg, inner, status);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be(msg);
            exception.StatusCode.Should().Be(status);
        }

        [Fact]
        public void Constructor_HttpResponseMessage()
        {
            // Arrange
            var response = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com"),
            };

            // Act
            var exception = new ProxyRequestException(response);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be($"HTTP Request '{response?.RequestMessage.RequestUri}' failed");
            exception.Response.Should().Be(response);
            exception.StatusCode.Should().Be(response.StatusCode);
        }

        [Fact]
        public void Constructor_HttpResponseMessage_WithInnerException()
        {
            // Arrange
            var msg = "message";
            var inner = new Exception(msg);
            var response = new HttpResponseMessage(HttpStatusCode.Accepted)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com"),
            };

            // Act
            var exception = new ProxyRequestException(response, inner);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be($"HTTP Request '{response?.RequestMessage.RequestUri}' failed");
            exception.Response.Should().Be(response);
            exception.StatusCode.Should().Be(response.StatusCode);
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be(msg);
        }
        #endregion
    }
}
