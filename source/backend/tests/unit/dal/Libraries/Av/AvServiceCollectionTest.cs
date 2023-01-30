using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Av;
using Pims.Av.Configuration;
using Pims.Core.Http;
using Pims.Core.Test;
using Xunit;

namespace Pims.Dal.Test.Libraries.Av
{
    [Trait("category", "unit")]
    [Trait("category", "av")]
    [Trait("group", "notification")]
    [ExcludeFromCodeCoverage]
    public class AvServiceCollectionTest
    {
        public IConfiguration Configuration { get; }

        #region Tests
        #region AvServiceCollection
        [Fact]
        public void AvServiceCollection_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.AddSingleton(user);

            var builder = new ConfigurationBuilder();
            var options = new ClamAvOptions()
            {
                DisableScan = true,
                HostUri = "testuri",
                Port = 1234,
                MaxFileSize = 1024,
            };
            var avJson = JsonSerializer.Serialize(new { Av = options });
            IConfigurationRoot avConfig;
            using (var io = new MemoryStream(Encoding.UTF8.GetBytes(avJson)))
            {
                builder.AddJsonStream(io);
                avConfig = builder.Build();
            }

            var mockClientFactory = new Mock<IHttpClientFactory>();
            var mockIOptionsMonitor = new Mock<IOptionsMonitor<JsonSerializerOptions>>();
            var mockIlogger = new Mock<ILogger<HttpRequestClient>>();
            var mockIAvService = new Mock<ILogger<IAvService>>();

            helper.AddSingleton(mockClientFactory.Object);
            helper.AddSingleton(mockIOptionsMonitor.Object);
            helper.AddSingleton(mockIlogger.Object);
            helper.AddSingleton(mockIAvService.Object);

            // Act
            _ = helper.Services.AddClamAvService(section: avConfig.GetSection("Av"));

            var avOptions = helper.GetService<IOptions<ClamAvOptions>>();
            var avService = helper.GetService<IAvService>();

            // Assert
            Assert.NotNull(avService);
            Assert.NotNull(avOptions);

            avOptions.Value.DisableScan.Should().Be(options.DisableScan);
            avOptions.Value.HostUri.Should().Be(options.HostUri);
            avOptions.Value.Port.Should().Be(options.Port);
        }
        #endregion
        #endregion
    }
}
