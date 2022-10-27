using System;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using FluentAssertions;
using Pims.Api.Helpers.Exceptions;
using Xunit;

namespace Pims.Api.Test.Exceptions
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("category", "exception")]
    [ExcludeFromCodeCoverage]
    public class ApiHttpRequestExceptionTest
    {
        #region Tests
        #region ApiHttpRequestException Constructor 01
        [Fact]
        public void ApiHttpRequestException_Constructor_01()
        {
            // Arrange
            // Act
            var exception = new ApiHttpRequestException("test", HttpStatusCode.Accepted);

            // Assert
            exception.Message.Should().Be("test");
            exception.StatusCode.Should().Be(HttpStatusCode.Accepted);
        }

        [Fact]
        public void ApiHttpRequestException_Constructor_01_DefaultStatusCode()
        {
            // Arrange
            // Act
            var exception = new ApiHttpRequestException("test");

            // Assert
            exception.Message.Should().Be("test");
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public void ApiHttpRequestException_Constructor_01_NullMessage()
        {
            // Arrange
            // Act
            var exception = new ApiHttpRequestException((string)null);

            // Assert
            exception.Message.Should().Be("Exception of type 'Pims.Api.Helpers.Exceptions.ApiHttpRequestException' was thrown.");
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }
        #endregion

        #region ApiHttpRequestException Constructor 02
        [Fact]
        public void ApiHttpRequestException_Constructor_02()
        {
            // Arrange
            var inner = new Exception("inner test");

            // Act
            var exception = new ApiHttpRequestException("test", inner, HttpStatusCode.Accepted);

            // Assert
            exception.Message.Should().Be("test");
            exception.StatusCode.Should().Be(HttpStatusCode.Accepted);
            exception.InnerException.Should().Be(inner);
        }

        [Fact]
        public void ApiHttpRequestException_Constructor_02_DefaultStatusCode()
        {
            // Arrange
            var inner = new Exception("inner test");

            // Act
            var exception = new ApiHttpRequestException("test", inner);

            // Assert
            exception.Message.Should().Be("test");
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            exception.InnerException.Should().Be(inner);
        }

        [Fact]
        public void ApiHttpRequestException_Constructor_02_NullMessage()
        {
            // Arrange
            var inner = new Exception("inner test");

            // Act
            var exception = new ApiHttpRequestException((string)null, inner);

            // Assert
            exception.Message.Should().Be("Exception of type 'Pims.Api.Helpers.Exceptions.ApiHttpRequestException' was thrown.");
            exception.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
            exception.InnerException.Should().Be(inner);
        }
        #endregion
        #endregion
    }
}
