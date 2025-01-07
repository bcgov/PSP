using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Api.Models;
using Pims.Api.Models.CodeTypes;
using Pims.Api.Models.Mayan.Document;
using Pims.Api.Models.Requests.Http;
using Pims.Core.Api.Repositories.Rest;
using Pims.Core.Extensions;
using Pims.Core.Test.Http;
using Xunit;

namespace Pims.Core.Test.Repositories.Rest
{
    public class BaseRestRepositoryTest
    {
        private readonly Mock<ILogger<BaseRestRepository>> _logger;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly Mock<IOptions<JsonSerializerOptions>> _jsonOptions;
        private readonly TestBaseRestRepository _repository;

        public BaseRestRepositoryTest()
        {
            _logger = new Mock<ILogger<BaseRestRepository>>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _jsonOptions = new Mock<IOptions<JsonSerializerOptions>>();
            _repository = new TestBaseRestRepository(
                _logger.Object,
                _httpClientFactory.Object,
                _jsonOptions.Object);
        }

        [Fact]
        public async Task GetAsync_Success()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<DocumentTypeModel> { Status = ExternalResponseStatus.Success, Payload = new DocumentTypeModel(), HttpStatusCode = HttpStatusCode.OK };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(new DocumentTypeModel().Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.GetAsync<DocumentTypeModel>(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetAsync_NoContent()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Success, Message = "No content was returned from the call", HttpStatusCode = HttpStatusCode.NoContent };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.GetAsync<string>(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task GetRawAsync_Success()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Test") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.GetRawAsync(new Uri("http://test.com"), "token");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetRawAsync_NoContent()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.GetRawAsync(new Uri("http://test.com"), "token");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.NoContent);
        }

        [Fact]
        public async Task GetRawAsync_Unauthorized()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Unauthorized)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.GetRawAsync(new Uri("http://test.com"), "token");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task GetRawAsync_InternalServerError()
        {
            // Arrange
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.GetRawAsync(new Uri("http://test.com"), "token");

            // Assert
            result.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task PostAsync_Success()
        {
            var documentDetail = new DocumentDetailModel { Id = 1, Label = "Test" };

            // Arrange
            var expectedResponse = new ExternalResponse<DocumentDetailModel> { Status = ExternalResponseStatus.Success, Payload = documentDetail, HttpStatusCode = HttpStatusCode.OK };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentDetail.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PostAsync<DocumentDetailModel>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostAsync_BadRequest()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Bad Request", HttpStatusCode = HttpStatusCode.BadRequest };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Bad Request") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PostAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostAsync_Unauthorized()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Request was forbidden", HttpStatusCode = HttpStatusCode.Forbidden };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent("Unauthorized") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PostAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostAsync_InternalServerError()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Unable to contact endpoint . Http status InternalServerError", HttpStatusCode = HttpStatusCode.InternalServerError };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Internal Server Error") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PostAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostAsync_Exception()
        {
            Mock<HttpClient> httpClient = new Mock<HttpClient>();

            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Exception during Post" };
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            // Act
            var result = await _repository.PostAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PostAsync_NotFound()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "The requested resource does not exist on the server", HttpStatusCode = HttpStatusCode.NotFound };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new StringContent("Not Found") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PostAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PutAsync_Success()
        {
            var documentDetail = new DocumentDetailModel { Id = 1, Label = "Test" };

            // Arrange
            var expectedResponse = new ExternalResponse<DocumentDetailModel> { Status = ExternalResponseStatus.Success, Payload = documentDetail, HttpStatusCode = HttpStatusCode.OK };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent(documentDetail.Serialize()) }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PutAsync<DocumentDetailModel>(new Uri("http://test.com"), new StringContent(documentDetail.Serialize()), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PutAsync_BadRequest()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Bad Request", HttpStatusCode = HttpStatusCode.BadRequest };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Bad Request") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PutAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PutAsync_Unauthorized()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Request was forbidden", HttpStatusCode = HttpStatusCode.Forbidden };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent("Unauthorized") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PutAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PutAsync_InternalServerError()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Unable to contact endpoint . Http status InternalServerError", HttpStatusCode = HttpStatusCode.InternalServerError };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Internal Server Error") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.PutAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task PutAsync_Exception()
        {
            Mock<HttpClient> httpClient = new Mock<HttpClient>();

            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Exception during Put" };
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            // Act
            var result = await _repository.PutAsync<string>(new Uri("http://test.com"), new StringContent("Test"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Success, Payload = "/", HttpStatusCode = HttpStatusCode.OK };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.OK) { Content = new StringContent("Test") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.DeleteAsync(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_NoContent()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Success, Message = "No content was returned from the call", Payload = "/", HttpStatusCode = HttpStatusCode.NoContent };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.NoContent)));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.DeleteAsync(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_Unauthorized()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Request was forbidden", Payload = "/", HttpStatusCode = HttpStatusCode.Forbidden };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.Forbidden) { Content = new StringContent("Request was forbidden") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.DeleteAsync(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_BadRequest()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Message = "Internal Server Error", Payload = "/", HttpStatusCode = HttpStatusCode.BadRequest };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.BadRequest) { Content = new StringContent("Internal Server Error") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.DeleteAsync(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_InternalServerError()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Payload = "/", Message = "Unable to contact endpoint . Http status InternalServerError", HttpStatusCode = HttpStatusCode.InternalServerError };
            var httpClient = new HttpClient(new FakeHttpMessageHandler(new HttpResponseMessage(HttpStatusCode.InternalServerError) { Content = new StringContent("Internal Server Error") }));
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient);

            // Act
            var result = await _repository.DeleteAsync(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        [Fact]
        public async Task DeleteAsync_Exception()
        {
            // Arrange
            var expectedResponse = new ExternalResponse<string> { Status = ExternalResponseStatus.Error, Payload = "/", Message = "Exception during Delete" };
            Mock<HttpClient> httpClient = new Mock<HttpClient>();
            _httpClientFactory.Setup(x => x.CreateClient(It.IsAny<string>())).Returns(httpClient.Object);

            // Act
            var result = await _repository.DeleteAsync(new Uri("http://test.com"), "token");

            // Assert
            result.Should().BeEquivalentTo(expectedResponse);
        }

        // Helper class to test the abstract BaseRestRepository
        private class TestBaseRestRepository : BaseRestRepository
        {
            public TestBaseRestRepository(ILogger logger, IHttpClientFactory httpClientFactory, IOptions<JsonSerializerOptions> jsonOptions)
                : base(logger, httpClientFactory, jsonOptions)
            {
            }

            public override void AddAuthentication(HttpClient client, string authenticationToken = null)
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authenticationToken);
            }
        }
    }
}
