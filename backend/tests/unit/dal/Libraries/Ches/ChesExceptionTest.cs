using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Ches;
using Pims.Ches.Configuration;
using Pims.Ches.Models;
using Pims.Core.Exceptions;
using Pims.Core.Http;
using Pims.Core.Http.Models;
using Pims.Core.Test;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ches
{
    [Trait("category", "unit")]
    [Trait("category", "ches")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class ChesExceptionTest
    {

        #region Tests
        #region ChesException Constructors
        [Fact]
        public void Constructor_ExceptionAndClientAndModel()
        {
            // Arrange
            var message = "test2";
            var status = HttpStatusCode.OK;
            var helper = new TestHelper();

            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ChesOptions()
            {
                AuthUrl = "https:/test.com",
                EmailEnabled = true,
                EmailAuthorized = true,
                BccUser = true
            });
            var service = helper.Create<ChesService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test"
            };

            var exception = new HttpClientRequestException(message, status);
            var client = helper.GetService<Mock<IHttpRequestClient>>().Object;
            var model = new ErrorResponseModel();

            // Act
            var chesException = new ChesException(exception, client, model);

            // Assert
            Assert.NotNull(chesException.Message);
            chesException.Message.Should().Be(message + System.Environment.NewLine);
            chesException.Client.Should().Be(client);
            chesException.StatusCode.Should().Be(status);
            chesException.Detail.Should().Be(model.Title + System.Environment.NewLine + model.Detail + System.Environment.NewLine + model.Type + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, model.Errors.Select(e => $"\t{e.Message}")));
        }

        [Fact]
        public void Constructor_NoStatus_ExceptionAndClientAndModel()
        {
            // Arrange
            var helper = new TestHelper();

            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new ChesOptions()
            {
                AuthUrl = "https:/test.com",
                EmailEnabled = true,
                EmailAuthorized = true,
                BccUser = true
            });
            var service = helper.Create<ChesService>(options, user);

            var token = new TokenModel()
            {
                AccessToken = "test"
            };

            var response = new HttpResponseMessage()
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test")
            };

            var exception = new HttpClientRequestException(response);
            var client = helper.GetService<Mock<IHttpRequestClient>>().Object;
            var model = new ErrorResponseModel();

            // Act
            var chesException = new ChesException(exception, client, model);

            // Assert
            Assert.NotNull(chesException.Message);
            chesException.Message.Should().Be("HTTP Request 'https://test/' failed" + System.Environment.NewLine);
            chesException.Client.Should().Be(client);
            chesException.StatusCode.Should().Be(HttpStatusCode.OK);
            chesException.Detail.Should().Be(model.Title + System.Environment.NewLine + model.Detail + System.Environment.NewLine + model.Type + System.Environment.NewLine + System.String.Join(System.Environment.NewLine, model.Errors.Select(e => $"\t{e.Message}")));
        }

        [Fact]
        public void Constructor_MessageAndStatus()
        {
            // Arrange
            var message = "test";
            var status = HttpStatusCode.OK;

            // Act
            var exception = new ChesException(message, status);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(message);
            exception.StatusCode.Should().Be(status);
        }

        [Fact]
        public void Constructor_MessageAndInnerExceptionAndStatus()
        {
            // Arrange
            var message = "test for inner exception 2";
            var status = HttpStatusCode.OK;

            var innerException = new HttpClientRequestException(message, status);
            var chesException = new ChesException(message, innerException, status);

            // Assert
            Assert.NotNull(chesException);
            chesException.Message.Should().Be(message);
            chesException.InnerException.Should().Be(innerException);
            chesException.StatusCode.Should().Be(status);
        }


        [Fact]
        public void Constructor_Response()
        {
            // Arrange
            var status = HttpStatusCode.OK;
            var response = new HttpResponseMessage(status)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test")
            };
            var exception = new ChesException(response);

            // Assert
            Assert.NotNull(exception);
            exception.Response.Should().Be(response);
            exception.StatusCode.Should().Be(status);
        }

        [Fact]
        public void Constructor_ResponseAndInnerException()
        {
            // Arrange
            var message = "test for inner exception";
            var status = HttpStatusCode.OK;

            var innerException = new HttpClientRequestException(message, status);
            var response = new HttpResponseMessage(status)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "https://test")
            };
            var chesException = new ChesException(response, innerException);

            // Assert
            Assert.NotNull(chesException);
            chesException.Response.Should().Be(response);
            chesException.InnerException.Should().Be(innerException);
        }
        #endregion
        #endregion
    }
}
