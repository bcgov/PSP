using System;
using System.Collections.Generic;
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
using Pims.Api.Models.Requests.Http;
using Pims.Api.Repositories.Mayan;
using Pims.Core.Api.Exceptions;
using Xunit;

namespace Pims.Api.Repositories.Mayan
{
    public class MayanAuthRepositoryTest
    {
        private readonly Mock<ILogger<MayanAuthRepository>> _logger;
        private readonly Mock<IHttpClientFactory> _httpClientFactory;
        private readonly IOptions<JsonSerializerOptions> _jsonOptions;
        private readonly IConfiguration _configuration;
        private readonly MayanAuthRepository _repository;

        public MayanAuthRepositoryTest()
        {
            _logger = new Mock<ILogger<MayanAuthRepository>>();
            _httpClientFactory = new Mock<IHttpClientFactory>();
            _jsonOptions = Options.Create<JsonSerializerOptions>(new JsonSerializerOptions() { AllowTrailingCommas = true, PropertyNameCaseInsensitive = true });
            _configuration = _configuration = new ConfigurationBuilder().AddInMemoryCollection(new Dictionary<string, string> { { "Mayan:BaseUri", "http://mayan" } }).Build();
            _repository = new MayanAuthRepository(
                _logger.Object,
                _httpClientFactory.Object,
                _configuration,
                _jsonOptions);
        }

        [Fact]
        public async Task GetTokenAsync_Success()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(new TokenResponse
                { Token = "test-token" }))
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

            // Act
            var result = await _repository.GetTokenAsync();

            // Assert
            result.Should().Be("test-token");
        }

        [Fact]
        public async Task GetTokenAsync_Failure()
        {
            // Arrange
            var responseMessage = new HttpResponseMessage(HttpStatusCode.Forbidden)
            {
                Content = new StringContent(JsonSerializer.Serialize(new TokenResponse
                { Token = "test-token" }))
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

            // Act
            Func<Task> act = async () => await _repository.GetTokenAsync();

            // Assert
            await act.Should().ThrowAsync<AuthenticationException>().WithMessage("Request was forbidden");
        }
    }
}
