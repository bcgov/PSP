using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Http;
using Pims.Core.Http.Configuration;
using Pims.Core.Test;
using Pims.Dal.Keycloak;
using Pims.Dal.Repositories;
using Pims.Keycloak;
using Pims.Keycloak.Configuration;
using Xunit;

namespace Pims.Dal.Test.Libraries.Keycloak
{
    [Trait("category", "unit")]
    [Trait("category", "keycloak")]
    [Trait("group", "keycloak")]
    [ExcludeFromCodeCoverage]
    public class ServiceCollectionExtensionsTest
    {
        #region Methods
        [Fact]
        public void AddKeycloakService_Success()
        {
            // Arrange
            var services = new ServiceCollection();
            var mockConfig = new Mock<IOptions<KeycloakOptions>>();
            var options = new KeycloakOptions()
            {
                Authority = "test",
                Audience = "test",
                Client = "test",
                ServiceAccount = new KeycloakServiceAccountOptions
                {
                    Client = "test",
                    Secret = "test",
                    Api = "api",
                    Environment = "test",
                    Integration = "4379",
                },
            };
            mockConfig.Setup(m => m.Value).Returns(options);
            services.AddScoped((s) => mockConfig.Object);
            var mockOpenIdConnectRequestClient = new Mock<IOpenIdConnectRequestClient>();
            var clientOptions = new AuthClientOptions();
            mockOpenIdConnectRequestClient.Setup(m => m.AuthClientOptions).Returns(clientOptions);
            services.AddScoped((s) => mockOpenIdConnectRequestClient.Object);

            // Act
            var result = services.AddKeycloakService();
            var provider = result.BuildServiceProvider();
            var service = provider.GetService<IKeycloakService>();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(3);
            provider.Should().NotBeNull();
            service.Should().NotBeNull();
        }

        [Fact]
        public void AddPimsKeycloakService_Success()
        {
            // Arrange
            var services = new ServiceCollection();

            var mockMapper = new Mock<IMapper>();
            services.AddScoped((s) => mockMapper.Object);
            var mockAccessRepository = new Mock<IAccessRequestRepository>();
            services.AddScoped((s) => mockAccessRepository.Object);
            var mockUserRepository = new Mock<IUserRepository>();
            services.AddScoped((s) => mockUserRepository.Object);
            var mockRoleRepository = new Mock<IRoleRepository>();
            services.AddScoped((s) => mockRoleRepository.Object);
            var mockLogger = new Mock<ILogger<IPimsKeycloakService>>();
            services.AddScoped((s) => mockLogger.Object);
            var user = PrincipalHelper.CreateForPermission();
            services.AddScoped((s) => user);

            var mockConfig = new Mock<IOptions<KeycloakOptions>>();
            var options = new KeycloakOptions()
            {
                Authority = "test",
                Audience = "test",
                Client = "test",
                ServiceAccount = new KeycloakServiceAccountOptions
                {
                    Client = "test",
                    Secret = "test",
                    Api = "api",
                    Environment = "test",
                    Integration = "4379",
                },
            };
            mockConfig.Setup(m => m.Value).Returns(options);
            services.AddScoped((s) => mockConfig.Object);
            var mockOpenIdConnectRequestClient = new Mock<IOpenIdConnectRequestClient>();
            var clientOptions = new AuthClientOptions();
            mockOpenIdConnectRequestClient.Setup(m => m.AuthClientOptions).Returns(clientOptions);
            services.AddScoped((s) => mockOpenIdConnectRequestClient.Object);

            // Act
            var result = services.AddPimsKeycloakService();
            var provider = result.BuildServiceProvider();
            var service = provider.GetService<IPimsKeycloakService>();

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(10);
            provider.Should().NotBeNull();
            service.Should().NotBeNull();
        }

        #endregion
    }
}
