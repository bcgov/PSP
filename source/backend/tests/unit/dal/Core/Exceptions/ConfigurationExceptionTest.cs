using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Exceptions;
using Xunit;

namespace Pims.Api.Test.Core.Exceptions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class ConfigurationExceptionTest
    {
        #region Tests
        [Fact]
        public void Constructor_Base()
        {
            // Arrange
            // Act
            var exception = new ConfigurationException();

            // Assert
            Assert.NotNull(exception);
        }

        [Fact]
        public void Constructor_Message()
        {
            // Arrange
            var msg = "message";

            // Act
            var exception = new ConfigurationException(msg);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void Constructor_MessageAndInnerException()
        {
            // Arrange
            var msg = "message";
            var inner = new Exception(msg);

            // Act
            var exception = new ConfigurationException(msg, inner);

            // Assert
            Assert.NotNull(exception);
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().NotBeNull();
            exception.InnerException.Message.Should().Be(msg);
        }
        #endregion
    }
}
