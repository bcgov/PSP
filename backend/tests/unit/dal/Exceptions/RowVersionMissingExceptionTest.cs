using FluentAssertions;
using Pims.Dal.Exceptions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class RowVersionMissingExceptionTest
    {
        #region Tests
        [Fact]
        public void RowVersionMissingException_Base_Constructor()
        {
            // Arrange
            // Act
            var exception = new RowVersionMissingException();

            // Assert
            exception.Message.Should().Be("Exception of type 'Pims.Dal.Exceptions.RowVersionMissingException' was thrown.");
        }

        [Fact]
        public void RowVersionMissingException_Constructor_01()
        {
            // Arrange
            var msg = "test";

            // Act
            var exception = new RowVersionMissingException(msg);

            // Assert
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void RowVersionMissingException_Constructor_02()
        {
            // Arrange
            var msg = "test";
            var error = new Exception("inner");

            // Act
            var exception = new RowVersionMissingException(msg, error);

            // Assert
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().Be(error);
        }
        #endregion
    }
}
