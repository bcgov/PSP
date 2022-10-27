using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class PropertyTypeTest
    {
        #region Tests
        [Fact]
        public void PropertyType_Default_Constructor()
        {
            // Arrange
            // Act
            var type = new PimsPropertyType();

            // Assert
            type.Id.Should().BeNull();
            type.Description.Should().BeNull();
            type.DisplayOrder.Should().BeNull();
        }

        [Fact]
        public void PropertyType_Constructor_01()
        {
            // Arrange
            // Act
            var type = new PimsPropertyType("name");

            // Assert
            type.Id.Should().Be("name");
            type.Description.Should().BeNull();
            type.DisplayOrder.Should().BeNull();
        }
        #endregion
    }
}
