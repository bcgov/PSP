using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Pims.Core.Exceptions;
using Pims.Core.Http.Configuration;
using Xunit;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class AuthClientOptionsTest
    {
        #region Variables
        public static IEnumerable<object[]> InvalidOptions =>
            new List<object[]>
            {
                new object[] { new AuthClientOptions() { Authority = null, Audience = "a", Client = "c" } },
                new object[] { new AuthClientOptions() { Authority = "a", Audience = null, Client = "c" } },
                new object[] { new AuthClientOptions() { Authority = "a", Audience = "a", Client = null } },
                new object[] { new AuthClientOptions() { Authority = string.Empty, Audience = "a", Client = "c" } },
                new object[] { new AuthClientOptions() { Authority = "a", Audience = string.Empty, Client = "c" } },
                new object[] { new AuthClientOptions() { Authority = "a", Audience = "a", Client = string.Empty } },
                new object[] { new AuthClientOptions() { Authority = " ", Audience = "a", Client = "c" } },
                new object[] { new AuthClientOptions() { Authority = "a", Audience = " ", Client = "c" } },
                new object[] { new AuthClientOptions() { Authority = "a", Audience = "a", Client = " " } },
            };
        #endregion

        #region Tests
        [Fact]
        public void Validate_Success()
        {
            // Arrange
            var options = new AuthClientOptions()
            {
                Audience = "Audience",
                Authority = "Authority",
                Client = "Client",
                Token = "Token",
            };

            // Act
            options.Validate();

            // Assert
            Assert.NotNull(options);
        }

        [Theory]
        [MemberData(nameof(InvalidOptions))]
        public void Validate_ConfigurationException(AuthClientOptions options)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ConfigurationException>(() => options.Validate());
        }
        #endregion
    }
}
