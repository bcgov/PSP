using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Ches;
using Pims.Ches.Configuration;
using Pims.Core.Http;
using Pims.Core.Test;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ches
{
    [Trait("category", "unit")]
    [Trait("category", "ches")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class ChesServiceCollectionTest
    {
        public IConfiguration Configuration { get; }

        #region Tests
        #region ChesServiceCollection
        [Fact]
        public void ChesServiceCollection_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.AddSingleton(user);

            var builder = new ConfigurationBuilder();
            var options = new ChesOptions()
            {
                AuthUrl = "a mocked value",
                EmailEnabled = true,
                EmailAuthorized = true
            };
            var chesJson = JsonSerializer.Serialize(new { Ches = options });
            IConfigurationRoot chesConfig;
            using (var io = new MemoryStream(Encoding.UTF8.GetBytes(chesJson)))
            {
                builder.AddJsonStream(io);
                chesConfig = builder.Build();
            }

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockIOptionsMonitor = new Mock<IOptionsMonitor<JsonSerializerOptions>>();
            var mockIlogger = new Mock<ILogger<HttpRequestClient>>();
            var mockIChesService = new Mock<ILogger<IChesService>>();

            helper.AddSingleton(mockClientFactory.Object);
            helper.AddSingleton(mockIOptionsMonitor.Object);
            helper.AddSingleton(mockIlogger.Object);
            helper.AddSingleton(mockIChesService.Object);

            // Act
            _ = helper.Services.AddChesService(section: chesConfig.GetSection("Ches"));

            var chesOptions = helper.GetService<IOptions<ChesOptions>>();
            var chesService = helper.GetService<IChesService>();
            var httpRequestClient = helper.GetService<IHttpRequestClient>();
            var jwtSecurityTokenHandler = helper.GetService<JwtSecurityTokenHandler>();

            // Assert
            Assert.NotNull(chesService);
            Assert.NotNull(httpRequestClient);
            Assert.NotNull(jwtSecurityTokenHandler);
            Assert.NotNull(chesOptions);

            chesOptions.Value.AuthUrl.Should().Be(options.AuthUrl);
            chesOptions.Value.EmailEnabled.Should().Be(options.EmailEnabled);
            chesOptions.Value.EmailAuthorized.Should().Be(options.EmailAuthorized);

        }
        #endregion
        #endregion
    }
}
