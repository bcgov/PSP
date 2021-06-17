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
    public class ProjectNumberTest
    {
        #region Tests
        [Fact]
        public void ProjectNumber_Default_Constructor()
        {
            // Arrange
            // Act
            var number = new ProjectNumber();

            // Assert
            number.Should().Be(0);
        }

        [Fact]
        public void ProjectNumber_ToString()
        {
            // Arrange
            var number = new ProjectNumber();

            // Act
            var result = number.ToString();

            // Assert
            result.Should().Be(number.ToString());
        }
        #endregion
    }
}
