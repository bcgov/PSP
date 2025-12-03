using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Api.Configuration;
using Xunit;

namespace Pims.Api.Test.Exceptions
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("category", "configuration")]
    [ExcludeFromCodeCoverage]
    public class ProgramOptionsTest
    {
        #region Tests
        [Fact]
        public void DefaultConstructor()
        {
            // Arrange
            // Act
            var options = new ProgramOptions();

            // Assert
            options.Environment.Should().BeNull();
            options.Urls.Should().BeNull();
            options.HttpsPort.Should().BeNull();
            options.ToArgs().Should().BeEmpty();
        }

        [Fact]
        public void AllValues()
        {
            // Arrange
            // Act
            var options = new ProgramOptions()
            {
                Environment = "Development",
                Urls = "https://test.com",
                HttpsPort = 443,
            };

            // Assert
            options.Environment.Should().Be("Development");
            options.Urls.Should().Be("https://test.com");
            options.HttpsPort.Should().Be(443);
            options.ToArgs().Should().BeEquivalentTo(new[] { "Development", "https://test.com", "443" });
        }
        #endregion
    }
}
