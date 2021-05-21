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
    public class AdministrativeAreaTest
    {
        #region Tests
        [Fact]
        public void AdministrativeArea_Default_Constructor()
        {
            // Arrange
            // Act
            var administrativeArea = new AdministrativeArea();

            // Assert
            administrativeArea.Name.Should().BeNull();
            administrativeArea.Abbreviation.Should().BeNull();
            administrativeArea.BoundaryType.Should().BeNull();
            administrativeArea.GroupName.Should().BeNull();
        }

        [Fact]
        public void AdministrativeArea_Name_Constructor()
        {
            // Arrange
            var name = "name";

            // Act
            var administrativeArea = new AdministrativeArea(name);

            // Assert
            administrativeArea.Name.Should().Be(name);
        }
        #endregion
    }
}
