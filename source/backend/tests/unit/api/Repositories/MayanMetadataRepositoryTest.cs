using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pims.Api.Test.Repositories
{
    using System.Net;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using FluentAssertions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Moq;
    using Moq.Protected;
    using Pims.Api.Models.CodeTypes;
    using Pims.Api.Models.Mayan;
    using Pims.Api.Models.Mayan.Metadata;
    using Pims.Api.Models.Requests.Http;
    using Pims.Api.Repositories.Mayan;
    using Xunit;

    public class MayanMetadataRepositoryTest
    {
        private readonly Mock<ILogger<MayanMetadataRepository>> _logger;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly Mock<IEdmsAuthRepository> _authRepository;
        private readonly Mock<IOptions<JsonSerializerOptions>> _jsonOptions;
        private readonly IConfiguration _configuration;
        private readonly MayanMetadataRepository _repository;

        public MayanMetadataRepositoryTest()
        {
            _logger = new Mock<ILogger<MayanMetadataRepository>>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _authRepository = new Mock<IEdmsAuthRepository>();
            _jsonOptions = new Mock<IOptions<JsonSerializerOptions>>();
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { { "Mayan:BaseUri", "http://mayan" } }).Build();
            _repository = new MayanMetadataRepository(
                _logger.Object,
                _httpClientFactory.Object,
                _authRepository.Object,
                _configuration,
                _jsonOptions.Object);
        }

        [Fact]
        public async Task TryGetMetadataTypesAsync_Success()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ExternalResponse<QueryResponse<MetadataTypeModel>>
                {
                    Status = ExternalResponseStatus.Success,
                    Payload = new QueryResponse<MetadataTypeModel>()
                }))
            };

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(_ => _.GetTokenAsync()).ReturnsAsync("test-token");

            // Act
            var result = await _repository.TryGetMetadataTypesAsync();

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryCreateMetadataTypeAsync_Success()
        {
            // Arrange
            var metadataType = new MetadataTypeModel();
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ExternalResponse<MetadataTypeModel>
                {
                    Status = ExternalResponseStatus.Success,
                    Payload = metadataType
                }))
            };

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(_ => _.GetTokenAsync()).ReturnsAsync("test-token");

            // Act
            var result = await _repository.TryCreateMetadataTypeAsync(metadataType);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryUpdateMetadataTypeAsync_Success()
        {
            // Arrange
            var metadataType = new MetadataTypeModel { Id = 1 };
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ExternalResponse<MetadataTypeModel>
                {
                    Status = ExternalResponseStatus.Success,
                    Payload = metadataType
                }))
            };

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(_ => _.GetTokenAsync()).ReturnsAsync("test-token");

            // Act
            var result = await _repository.TryUpdateMetadataTypeAsync(metadataType);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }

        [Fact]
        public async Task TryDeleteMetadataTypeAsync_Success()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new ExternalResponse<string>
                {
                    Status = ExternalResponseStatus.Success,
                    Payload = "Deleted"
                }))
            };

            var httpMessageHandler = new Mock<HttpMessageHandler>();
            httpMessageHandler.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(responseMessage);

            var httpClient = new HttpClient(httpMessageHandler.Object);
            _httpClientFactory.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(httpClient);
            _authRepository.Setup(_ => _.GetTokenAsync()).ReturnsAsync("test-token");

            // Act
            var result = await _repository.TryDeleteMetadataTypeAsync(1);

            // Assert
            result.Should().NotBeNull();
            result.Status.Should().Be(ExternalResponseStatus.Success);
        }
    }
}
