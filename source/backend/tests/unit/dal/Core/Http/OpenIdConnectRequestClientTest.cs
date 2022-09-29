using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Http;
using Pims.Core.Http.Configuration;
using Pims.Core.Test;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class OpenIdConnectRequestClientTest
    {
        #region Tests
        [Fact]
        public void OpenIdConnectRequestClient_Constructor()
        {
            // Arrange
            var helper = new TestHelper();
            var response = new HttpResponseMessage();
            var clientFactory = helper.CreateHttpClientFactory(response);
            var tokenHandler = new JwtSecurityTokenHandler();
            var authClientOptions = new AuthClientOptions();
            var mockAuthClientOptions = new Mock<IOptionsMonitor<AuthClientOptions>>();
            mockAuthClientOptions.Setup(m => m.CurrentValue).Returns(authClientOptions);
            var openIdConnectOptions = new OpenIdConnectOptions();
            var mockOpenIdConnectOptions = new Mock<IOptionsMonitor<OpenIdConnectOptions>>();
            mockOpenIdConnectOptions.Setup(m => m.CurrentValue).Returns(openIdConnectOptions);
            var mockJsonSerializeOptions = new Mock<IOptionsMonitor<JsonSerializerOptions>>();
            var mockLogger = new Mock<ILogger<OpenIdConnectRequestClient>>();

            // Act
            var client = new OpenIdConnectRequestClient(clientFactory, tokenHandler, mockAuthClientOptions.Object, mockOpenIdConnectOptions.Object, mockJsonSerializeOptions.Object, mockLogger.Object);

            // Assert
            Assert.NotNull(client);
            client.AuthClientOptions.Should().Be(authClientOptions);
            client.OpenIdConnectOptions.Should().Be(openIdConnectOptions);
            client.Client.Should().NotBeNull();
        }
        #endregion
    }
}
