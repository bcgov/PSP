using FluentAssertions;
using MapsterMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Ches;
using Pims.Core.Http;
using Pims.Core.Http.Configuration;
using Pims.Core.Test;
using Pims.Notifications;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Libraries.Notifications
{
    [Trait("category", "unit")]
    [Trait("category", "notifications")]
    [Trait("group", "notifications")]
    [ExcludeFromCodeCoverage]
    public class ServiceCollectionExtensionsTest
    {
        #region Methods
        [Fact]
        public void AddNotificationsService_Success()
        {
            // Arrange
            var services = new ServiceCollection();

            var mockOptions = new Mock<IOptions<Pims.Notifications.Configuration.NotificationOptions>>();
            var options = new Pims.Notifications.Configuration.NotificationOptions();
            mockOptions.Setup(m => m.Value).Returns(options);
            services.AddScoped((s) => mockOptions.Object);

            var mockChesService = new Mock<IChesService>();
            services.AddScoped((s) => mockChesService.Object);

            var mockLogger = new Mock<ILogger<NotificationService>>();
            services.AddScoped((s) => mockLogger.Object);

            // Act
            var result = services.AddNotificationsService();
            var provider = result.BuildServiceProvider();
            var service = provider.GetService<INotificationService>();
            var dalService = provider.GetService<Dal.Services.INotificationService>();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(5);
            provider.Should().NotBeNull();
            service.Should().NotBeNull();
            dalService.Should().NotBeNull();
        }

        [Fact]
        public void AddNotificationsService_WithConfig_Success()
        {
            // Arrange
            var services = new ServiceCollection();

            var mockOptions = new Mock<IOptions<Pims.Notifications.Configuration.NotificationOptions>>();
            var options = new Pims.Notifications.Configuration.NotificationOptions();
            mockOptions.Setup(m => m.Value).Returns(options);
            services.AddScoped((s) => mockOptions.Object);

            var mockChesService = new Mock<IChesService>();
            services.AddScoped((s) => mockChesService.Object);

            var mockLogger = new Mock<ILogger<NotificationService>>();
            services.AddScoped((s) => mockLogger.Object);

            var mockConfig = new Mock<IConfigurationSection>();

            // Act
            var result = services.AddNotificationsService(mockConfig.Object);
            var provider = result.BuildServiceProvider();
            var service = provider.GetService<INotificationService>();
            var dalService = provider.GetService<Dal.Services.INotificationService>();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(12);
            provider.Should().NotBeNull();
            service.Should().NotBeNull();
            dalService.Should().NotBeNull();
        }
        #endregion
    }
}
