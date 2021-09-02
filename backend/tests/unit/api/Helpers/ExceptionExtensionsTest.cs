using Pims.Api.Helpers.Exceptions;
using Pims.Api.Helpers.Extensions;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Helpers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class ExceptionExtensionsTest
    {
        #region Tests
        [Fact]
        public void ThrowBadRequestIfNull_WithNull()
        {
            // Arrange
            Entity.Property property = null;

            // Act
            // Assert
            Assert.Throws<BadRequestException>(() => property.ThrowBadRequestIfNull("test"));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void ThrowBadRequestIfNull_ArgumentException(string message)
        {
            // Arrange
            Entity.Property property = null;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => property.ThrowBadRequestIfNull(message));
        }

        [Fact]
        public void ThrowBadRequestIfNull_WithObject()
        {
            // Arrange
            var property = new Entity.Property();

            // Act
            property.ThrowBadRequestIfNull("test");

            // Assert
            Assert.NotNull(property);
        }
        #endregion
    }
}
