using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
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
    public class ProjectExtensionsTest
    {
        #region Tests
        #region UpdateProjectNumbers ProjectProperty Entity
        [Fact]
        public void UpdateProjectNumbers_Land()
        {
            // Arrange
            var parcel = new Parcel
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
                Parcel = parcel
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"3\",\"4\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_Land_NoProjects()
        {
            // Arrange
            var parcel = new Parcel();
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
                Parcel = parcel
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"3\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_NullLand()
        {
            // Arrange
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
            };

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => projectProperty.UpdateProjectNumbers("3"));
        }

        [Fact]
        public void UpdateProjectNumbers_AddLand()
        {
            // Arrange
            var parcel = new Parcel
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
                Parcel = parcel
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("5");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"3\",\"4\",\"5\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_Building()
        {
            // Arrange
            var building = new Building
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
                Building = building
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"3\",\"4\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_Building_NoProjects()
        {
            // Arrange
            var building = new Building();
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
                Building = building
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"3\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_AddBuilding()
        {
            // Arrange
            var building = new Building
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
                Building = building
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("5");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"3\",\"4\",\"5\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_NullBuilding()
        {
            // Arrange
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
            };

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => projectProperty.UpdateProjectNumbers("3"));
        }

        [Fact]
        public void UpdateProjectNumbers_Subdivision()
        {
            // Arrange
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Subdivision,
            };

            // Act
            var result = projectProperty.UpdateProjectNumbers("5");

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region UpdateProjectNumbers ProjectProperty Model
        [Fact]
        public void UpdateProjectNumbers_Model_Land_Empty()
        {
            // Arrange
            var property = new Dal.Entities.Views.Property();
            var projectProperty = new Dal.Entities.Models.ProjectProperty(property);

            // Act
            var result = projectProperty.UpdateProjectNumbers("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"3\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_Model_Land()
        {
            // Arrange
            var property = new Dal.Entities.Views.Property
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new Dal.Entities.Models.ProjectProperty(property);

            // Act
            var result = projectProperty.UpdateProjectNumbers("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"3\",\"4\"]");
        }

        [Fact]
        public void UpdateProjectNumbers_Model_AddLand()
        {
            // Arrange
            var property = new Dal.Entities.Views.Property
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new Dal.Entities.Models.ProjectProperty(property);

            // Act
            var result = projectProperty.UpdateProjectNumbers("5");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"3\",\"4\",\"5\"]");
        }
        #endregion

        #region RemoveProjectNumber ProjectProperty Entity
        [Fact]
        public void RemoveProjectNumber_Land()
        {
            // Arrange
            var parcel = new Parcel
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
                Parcel = parcel
            };

            // Act
            var result = projectProperty.RemoveProjectNumber("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"4\"]");
        }

        [Fact]
        public void RemoveProjectNumber_Land_NoProjects()
        {
            // Arrange
            var parcel = new Parcel();
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
                Parcel = parcel
            };

            // Act
            var result = projectProperty.RemoveProjectNumber("3");

            // Assert
            result.ProjectNumbers.Should().Be("[]");
        }

        [Fact]
        public void RemoveProjectNumber_NullLand()
        {
            // Arrange
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Land,
            };

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => projectProperty.RemoveProjectNumber("3"));
        }

        [Fact]
        public void RemoveProjectNumber_Building()
        {
            // Arrange
            var building = new Building
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
                Building = building
            };

            // Act
            var result = projectProperty.RemoveProjectNumber("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"4\"]");
        }

        [Fact]
        public void RemoveProjectNumber_Building_NoProjects()
        {
            // Arrange
            var building = new Building();
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
                Building = building
            };

            // Act
            var result = projectProperty.RemoveProjectNumber("3");

            // Assert
            result.ProjectNumbers.Should().Be("[]");
        }

        [Fact]
        public void RemoveProjectNumber_NullBuilding()
        {
            // Arrange
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Building,
            };

            // Act
            // Assert
            Assert.Throws<InvalidOperationException>(() => projectProperty.RemoveProjectNumber("3"));
        }

        [Fact]
        public void RemoveProjectNumber_Subdivision()
        {
            // Arrange
            var projectProperty = new ProjectProperty()
            {
                PropertyType = PropertyTypes.Subdivision,
            };

            // Act
            var result = projectProperty.RemoveProjectNumber("5");

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region RemoveProjectNumber Property
        [Fact]
        public void RemoveProjectNumber_Property()
        {
            // Arrange
            var parcel = new Parcel
            {
                ProjectNumbers = "[\"1\",\"2\",\"3\",\"4\"]"
            };

            // Act
            var result = parcel.RemoveProjectNumber("3");

            // Assert
            result.ProjectNumbers.Should().Be("[\"1\",\"2\",\"4\"]");
        }

        [Fact]
        public void RemoveProjectNumber_Null()
        {
            // Arrange
            var parcel = new Parcel();

            // Act
            var result = parcel.RemoveProjectNumber("3");

            // Assert
            result.ProjectNumbers.Should().Be("[]");
        }
        #endregion
        #endregion
    }
}
