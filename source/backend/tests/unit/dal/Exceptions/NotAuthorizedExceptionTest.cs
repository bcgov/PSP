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
    public class NotAuthorizedExceptionTest
    {
        #region Tests
        [Fact]
        public void NotAuthorizedException_Base_Constructor()
        {
            // Arrange
            // Act
            var exception = new NotAuthorizedException();

            // Assert
            exception.Message.Should().Be("Exception of type 'Pims.Dal.Exceptions.NotAuthorizedException' was thrown.");
        }

        [Fact]
        public void NotAuthorizedException_Constructor_01()
        {
            // Arrange
            var msg = "test";

            // Act
            var exception = new NotAuthorizedException(msg);

            // Assert
            exception.Message.Should().Be(msg);
        }

        [Fact]
        public void NotAuthorizedException_Constructor_01_NullMessage()
        {
            // Arrange
            // Act
            var exception = new NotAuthorizedException(null);

            // Assert
            exception.Message.Should().Be("User is not authorized to perform this action.");
        }

        [Fact]
        public void NotAuthorizedException_Constructor_02()
        {
            // Arrange
            var msg = "test";
            var error = new Exception("inner");

            // Act
            var exception = new NotAuthorizedException(msg, error);

            // Assert
            exception.Message.Should().Be(msg);
            exception.InnerException.Should().Be(error);
        }

        [Fact]
        public void NotAuthorizedException_Constructor_02_NullMessage()
        {
            // Arrange
            var error = new Exception("inner");

            // Act
            var exception = new NotAuthorizedException(null, error);

            // Assert
            exception.Message.Should().Be("User is not authorized to perform this action.");
            exception.InnerException.Should().Be(error);
        }
        #endregion
    }
}
