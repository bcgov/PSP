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
    [Trait("category", "exception")]
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
        public void Constructor_MessageAndInnerHttpClientRequestException()
        {
            // Arrange
            var msg = "message";
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com/"),
                Content = new StringContent("test"),
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

        [Fact]
        public void Constructor_WithResponse()
        {
            // Arrange
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com/"),
                Content = new StringContent("test"),
            };

            // Act
            var exception = new HttpClientRequestException(response);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be($"HTTP Request '{response?.RequestMessage.RequestUri}' failed");
            exception.InnerException.Should().BeNull();
            exception.Response.Should().Be(response);
        }

        [Fact]
        public void Constructor_NullResponse()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new HttpClientRequestException((HttpResponseMessage)null));
        }

        [Fact]
        public void Constructor_ResponseAndMessage()
        {
            // Arrange
            var msg = "message";
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com/"),
                Content = new StringContent("test"),
            };

            // Act
            var exception = new HttpClientRequestException(response, msg);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().BeNull();
            exception.Response.Should().Be(response);
        }

        [Fact]
        public void Constructor_NullResponseAndMessage()
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new HttpClientRequestException((HttpResponseMessage)null, "message"));
        }

        [Fact]
        public void Constructor_ResponseAndInnerException()
        {
            // Arrange
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com/"),
                Content = new StringContent("test"),
            };
            var inner = new HttpClientRequestException(response);

            // Act
            var exception = new HttpClientRequestException(response, inner);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be($"HTTP Request '{response?.RequestMessage.RequestUri}' failed");
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be($"HTTP Request '{response?.RequestMessage.RequestUri}' failed");
            exception.Response.Should().Be(response);
        }

        [Fact]
        public void Constructor_ResponseAndInnerException_ArgumentNullException()
        {
            // Arrange
            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Get, "https://test.com/"),
                Content = new StringContent("test"),
            };
            var inner = new HttpClientRequestException(response);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new HttpClientRequestException((HttpResponseMessage)null, inner));
        }
        #endregion
    }
}
