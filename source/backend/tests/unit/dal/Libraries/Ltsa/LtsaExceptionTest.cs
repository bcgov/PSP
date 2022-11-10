using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Http;
using Pims.Core.Test;
using Pims.Ltsa;
using Pims.Ltsa.Configuration;
using Pims.Ltsa.Models;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ltsa
{
    [Trait("category", "unit")]
    [Trait("category", "ltsa")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class LtsaExceptionTest
    {

        #region Tests
        #region LtsaException Constructors
        [Fact]
        public void LtsaExceptionClientModel_Success()
        {
            // Arrange
            var message = "test2";
            var status = HttpStatusCode.OK;
            var helper = new TestHelper();

            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new LtsaOptions());
            helper.Create<LtsaService>(options, user);

            var exception = new HttpClientRequestException(message, status);
            var client = new HttpClient();
            var error = new Error() { ErrorMessages = new List<string>() { "errorMessages" } };
            // Act
            var ltsaException = new LtsaException(exception, client, error);

            // Assert
            Assert.NotNull(ltsaException.Message);
            ltsaException.Message.Should().Be(message + System.Environment.NewLine);
            ltsaException.Client.Should().Be(client);
            ltsaException.StatusCode.Should().Be(status);
            ltsaException.Detail.Should().Be(null);
        }

        [Fact]
        public void LtsaException_Success()
        {
            // Arrange
            var message = "test";
            var status = HttpStatusCode.OK;

            // Act
            var exception = new LtsaException(message, status);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(status);
        }

        [Fact]
        public void LtsaExceptionMessageInnerExceptionStatus_Success()
        {
            // Arrange
            var message = "test for inner exception 2";
            var status = HttpStatusCode.OK;

            var innerException = new HttpClientRequestException(message, status);
            var ltsaException = new LtsaException(message, innerException, status);

            // Assert
            Assert.NotNull(ltsaException);
            ltsaException.Message.Should().Be(message);
            ltsaException.InnerException.Should().Be(innerException);
            ltsaException.StatusCode.Should().Be(status);
        }

        [Fact]
        public void LtsaExceptionResponse_Success()
        {
            // Arrange
            var status = HttpStatusCode.OK;
            var response = new HttpResponseMessage(status)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
            };
            var exception = new LtsaException(response);

            // Assert
            Assert.NotNull(exception);
            exception.Response.Should().Be(response);
            exception.StatusCode.Should().Be(status);
        }

        [Fact]
        public void LtsaExceptionResponseInnerException_Success()
        {
            // Arrange
            var message = "test for inner exception";
            var status = HttpStatusCode.OK;

            var innerException = new HttpClientRequestException(message, status);
            var response = new HttpResponseMessage(status)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test"),
            };
            var ltsaException = new LtsaException(response, innerException);

            // Assert
            Assert.NotNull(ltsaException);
            ltsaException.Response.Should().Be(response);
            ltsaException.InnerException.Should().Be(innerException);
        }
        #endregion
        #endregion
    }
}
