using FluentAssertions;
using Pims.Dal.Entities;
using System.Diagnostics.CodeAnalysis;
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
            var type = new PropertyType();

            // Assert
            type.Id.Should().Be(0);
            type.Name.Should().BeNull();
            type.SortOrder.Should().Be(0);
        }

        [Fact]
        public void PropertyType_Constructor_01()
        {
            // Arrange
            // Act
            var type = new PropertyType(1, "name");

            // Assert
            type.Id.Should().Be(1);
            type.Name.Should().Be("name");
            type.SortOrder.Should().Be(0);
        }
        #endregion
    }
}
