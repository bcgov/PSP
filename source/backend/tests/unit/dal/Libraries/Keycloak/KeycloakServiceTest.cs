using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Exceptions;
using Pims.Core.Http;
using Pims.Core.Test;
using Pims.Keycloak;
using Pims.Keycloak.Configuration;
using Xunit;

namespace Pims.Dal.Test.Libraries.Keycloak
{
    [Trait("category", "unit")]
    [Trait("category", "keycloak")]
    [Trait("group", "keycloak")]
    [ExcludeFromCodeCoverage]
    public partial class KeycloakServiceTest
    {
        #region Tests
        #region CreateKeycloakService
        [Fact]
        public void CreateKeycloakService_NoAuthority()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions());

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            // Assert
            var result = Assert.Throws<ConfigurationException>(() => helper.Create<KeycloakService>(options, user));
            result.Message.Should().Be("The configuration for Keycloak:Authority is invalid or missing.");
        }

        [Fact]
        public void CreateKeycloakService_NoAudience()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            // Assert
            var result = Assert.Throws<ConfigurationException>(() => helper.Create<KeycloakService>(options, user));
            result.Message.Should().Be("The configuration for Keycloak:Audience is invalid or missing.");
        }

        [Fact]
        public void CreateKeycloakService_NoClient()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
                Audience = "pims",
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            // Assert
            var result = Assert.Throws<ConfigurationException>(() => helper.Create<KeycloakService>(options, user));
            result.Message.Should().Be("The configuration for Keycloak:Client is invalid or missing.");
        }

        [Fact]
        public void CreateKeycloakService_NoServiceAccount()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
                Audience = "pims",
                Client = "pims",
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            // Assert
            var result = Assert.Throws<ConfigurationException>(() => helper.Create<KeycloakService>(options, user));
            result.Message.Should().Be("The configuration for Keycloak:ServiceAccount is invalid or missing.");
        }

        [Fact]
        public void CreateKeycloakService_NoServiceAccountClient()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
                Audience = "pims",
                Client = "pims",
                ServiceAccount = new KeycloakServiceAccountOptions(),
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            // Assert
            var result = Assert.Throws<ConfigurationException>(() => helper.Create<KeycloakService>(options, user));
            result.Message.Should().Be("The configuration for Keycloak:ServiceAccount:Client is invalid or missing.");
        }

        [Fact]
        public void CreateKeycloakService_NoServiceAccountSecret()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
                Audience = "pims",
                Client = "pims",
                ServiceAccount = new KeycloakServiceAccountOptions()
                {
                    Client = "pims-service-account",
                },
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            // Assert
            var result = Assert.Throws<ConfigurationException>(() => helper.Create<KeycloakService>(options, user));
            result.Message.Should().Be("The configuration for Keycloak:ServiceAccount:Secret is invalid or missing.");
        }

        [Fact]
        public void CreateKeycloakService()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
                Audience = "pims",
                Client = "pims",
                ServiceAccount = new KeycloakServiceAccountOptions()
                {
                    Client = "pims-service-account",
                    Secret = "[USE SECRETS]",
                    Api = "https://api.loginproxy.gov.bc.ca/api/v1",
                    Integration = "4379",
                    Environment = "test",
                },
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            var service = helper.Create<KeycloakService>(options, user);

            // Assert
            openIdConnect.Object.AuthClientOptions.Audience.Should().Be(options.Value.Audience);
            openIdConnect.Object.AuthClientOptions.Authority.Should().Be(options.Value.Authority);
            openIdConnect.Object.AuthClientOptions.Client.Should().Be(options.Value.ServiceAccount.Client);
            openIdConnect.Object.AuthClientOptions.Secret.Should().Be(options.Value.ServiceAccount.Secret);
        }

        [Fact]
        public void CreateKeycloakService_ServiceAccount()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var options = Options.Create(new KeycloakOptions()
            {
                Authority = "https://keycloak",
                Audience = "pims",
                Client = "pims",
                ServiceAccount = new KeycloakServiceAccountOptions()
                {
                    Client = "pims-service-account",
                    Secret = "[USE SECRETS]",
                    Api = "https://api.loginproxy.gov.bc.ca/api/v1",
                    Integration = "4379",
                    Environment = "test",
                    Audience = "pims-service-account",
                    Authority = "https://loginproxy.gov.bc.ca/auth/realms/standard",
                },
            });

            var openIdConnect = new Mock<IOpenIdConnectRequestClient>();
            openIdConnect.Setup(m => m.AuthClientOptions).Returns(new Pims.Core.Http.Configuration.AuthClientOptions());
            openIdConnect.Setup(m => m.DeleteAsync(It.IsAny<string>(), It.IsAny<HttpContent>())).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));
            helper.AddSingleton(openIdConnect.Object);

            // Act
            var service = helper.Create<KeycloakService>(options, user);

            // Assert
            openIdConnect.Object.AuthClientOptions.Audience.Should().Be(options.Value.ServiceAccount.Audience);
            openIdConnect.Object.AuthClientOptions.Authority.Should().Be(options.Value.ServiceAccount.Authority);
            openIdConnect.Object.AuthClientOptions.Client.Should().Be(options.Value.ServiceAccount.Client);
            openIdConnect.Object.AuthClientOptions.Secret.Should().Be(options.Value.ServiceAccount.Secret);
        }
        #endregion

        #endregion
    }
}
