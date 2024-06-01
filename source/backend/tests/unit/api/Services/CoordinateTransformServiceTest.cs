using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using NetTopologySuite.Geometries;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Repositories;
using Pims.Dal.Services;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "coordinate-transform")]
    [ExcludeFromCodeCoverage]
    public class CoordinateTransformServiceTest
    {
        private TestHelper _helper;
        private CoordinateTransformService _service;

        public CoordinateTransformServiceTest()
        {
            this._helper = new TestHelper();
            this._service = this._helper.Create<CoordinateTransformService>();
        }

        #region Tests
        [Fact]
        public void TransformCoordinates_Wgs84_BcAlbers()
        {
            // Arrange
            var expected = new Coordinate(924303.6196359333, 1088419.4036716279);

            // Act
            var location = new Coordinate(-127.18, 54.79);
            var actual = this._service.TransformCoordinates(4326, 3005, location);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void TransformCoordinates_BcAlbers_Wgs84()
        {
            // Arrange
            var expected = new Coordinate(-127.18432267731438, 54.793830114524795);

            // Act
            var location = new Coordinate(924033.50, 1088851.50);
            var actual = this._service.TransformCoordinates(3005, 4326, location);

            // Assert
            actual.Should().Be(expected);
        }

        [Fact]
        public void TransformCoordinates_NotSupported()
        {
            // Arrange
            var location = new Coordinate(924033.50, 1088851.50);

            // Act
            Action act = () => this._service.TransformCoordinates(900913, 4326, location);

            // Assert
            this._service.IsCoordinateSystemSupported(900913).Should().BeFalse();
            act.Should().Throw<InvalidOperationException>();
        }


        [Fact]
        public void TransformGeometry_Wgs84_BcAlbers()
        {
            // Arrange
            var boundary = EntityHelper.CreatePolygon(4396);

            // Act
            this._service.TransformGeometry(4326, 3005, boundary);

            // Assert
            boundary.SRID.Should().Be(3005);
            boundary.ExteriorRing.GetCoordinateN(0).Should().Be(new Coordinate(3021312.0903253276, 375417.28211033065));
        }

        [Fact]
        public void TransformGeometry_BcAlbers_Wgs84()
        {
            // Arrange
            var boundary = EntityHelper.CreatePolygon(3005);

            // Act
            this._service.TransformGeometry(3005, 4326, boundary);

            // Assert
            boundary.SRID.Should().Be(4326);
            boundary.ExteriorRing.GetCoordinateN(0).Should().Be(new Coordinate(-138.4471772534371, 44.199680362622246));
        }

        [Fact]
        public void TransformGeometry_NotSupported()
        {
            // Arrange
            // Arrange
            var boundary = EntityHelper.CreatePolygon(900913);

            // Act
            Action act = () => this._service.TransformGeometry(900913, 4326, boundary);

            // Assert
            this._service.IsCoordinateSystemSupported(900913).Should().BeFalse();
            act.Should().Throw<InvalidOperationException>();
        }
        #endregion
    }
}
