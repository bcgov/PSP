using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Dal.Exceptions;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class ConfigurationExceptionTest
    {
        #region Tests
        [Fact]
        public void ConfigurationException_Base_Constructor()
        {
            // Arrange
            // Act
            var exception = new ConfigurationException();

            // Assert
            exception.Message.Should().Be("Exception of type 'Pims.Dal.Exceptions.ConfigurationException' was thrown.");
        }

        [Fact]
        public void ConfigurationException_Constructor_01()
        {
            // Arrange
            var msg = "test";

            // Act
            var exception = new ConfigurationException(msg);

            // Assert
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void ConfigurationException_Constructor_02()
        {
            // Arrange
            var msg = "test";
            var error = new Exception("inner");

            // Act
            var exception = new ConfigurationException(msg, error);

            // Assert
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().Be(error);
        }
        #endregion
    }
}
