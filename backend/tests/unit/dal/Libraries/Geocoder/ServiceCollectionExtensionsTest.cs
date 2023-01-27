using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Pims.Core.Http;
using Pims.Geocoder;
using Xunit;

namespace Pims.Dal.Test.Libraries.Geocoder
{
    [Trait("category", "unit")]
    [Trait("category", "geocoder")]
    [Trait("group", "geocoder")]
    [ExcludeFromCodeCoverage]
    public class ServiceCollectionExtensionsTest
    {
        #region Methods
        [Fact]
        public void AddGeocoderService_Success()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfig = new Mock<IConfigurationSection>();
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();
            services.AddScoped((s) => mockHttpClientFactory.Object);
            var mockLogger = new Mock<ILogger<HttpRequestClient>>();
            services.AddScoped((s) => mockLogger.Object);

            // Act
            var result = services.AddGeocoderService(mockConfig.Object);
            var provider = result.BuildServiceProvider();
            var service = provider.GetService<IGeocoderService>();
            var client = provider.GetService<IHttpRequestClient>();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(11);
            provider.Should().NotBeNull();
            service.Should().NotBeNull();
            client.Should().NotBeNull();
        }
        #endregion
    }
}
