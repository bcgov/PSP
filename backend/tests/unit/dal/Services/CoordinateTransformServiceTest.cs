using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NetTopologySuite.Geometries;
using Pims.Core.Test;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "coordinate-transform")]
    [ExcludeFromCodeCoverage]
    public class CoordinateTransformServiceTest
    {
        #region Tests
        [Fact]
        public void Transform_Wgs84_BcAlbers()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<CoordinateTransformService>();
            var expected = new Coordinate(924303.6196359333, 1088419.4036716279);

            // Act
            var location = new Coordinate(-127.18, 54.79);
            var actual = service.TransformCoordinates(4326, 3005, location);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Transform_BcAlbers_Wgs84()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<CoordinateTransformService>();
            var expected = new Coordinate(-127.18432267731438, 54.793830114524795);

            // Act
            var location = new Coordinate(924033.50, 1088851.50);
            var actual = service.TransformCoordinates(3005, 4326, location);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void Transform_Not_Supported()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<CoordinateTransformService>();

            // Act
            // Assert
            var location = new Coordinate(924033.50, 1088851.50);
            service.IsCoordinateSystemSupported(900913).Should().BeFalse();
            Assert.Throws<InvalidOperationException>(() => service.TransformCoordinates(900913, 4326, location));
        }
        #endregion
    }
}
