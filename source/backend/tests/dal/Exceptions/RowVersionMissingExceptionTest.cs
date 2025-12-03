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
    public class ConcurrencyControlNumberMissingExceptionTest
    {
        #region Tests
        [Fact]
        public void ConcurrencyControlNumberMissingException_Base_Constructor()
        {
            // Arrange
            // Act
            var exception = new ConcurrencyControlNumberMissingException();

            // Assert
            exception.Message.Should().Be("Exception of type 'Pims.Dal.Exceptions.ConcurrencyControlNumberMissingException' was thrown.");
        }

        [Fact]
        public void ConcurrencyControlNumberMissingException_Constructor_01()
        {
            // Arrange
            var msg = "test";

            // Act
            var exception = new ConcurrencyControlNumberMissingException(msg);

            // Assert
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void ConcurrencyControlNumberMissingException_Constructor_02()
        {
            // Arrange
            var msg = "test";
            var error = new Exception("inner");

            // Act
            var exception = new ConcurrencyControlNumberMissingException(msg, error);

            // Assert
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().Be(error);
        }
        #endregion
    }
}
