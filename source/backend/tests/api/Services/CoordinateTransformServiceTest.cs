using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NetTopologySuite.Geometries;
using Pims.Api.Services;
using Pims.Core.Test;
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
            var location = new Coordinate(-127.18, 54.79);

            // Act
            var actual = this._service.TransformCoordinates(4326, 3005, location);

            // Assert
            actual.X.Should().BeApproximately(924303.62, 0.01d);
            actual.Y.Should().BeApproximately(1088419.40, 0.01d);
        }

        [Fact]
        public void TransformCoordinates_BcAlbers_Wgs84()
        {
            // Arrange
            var location = new Coordinate(924033.50, 1088851.50);

            // Act
            var actual = this._service.TransformCoordinates(3005, 4326, location);

            // Assert
            actual.X.Should().BeApproximately(-127.18, 0.01d);
            actual.Y.Should().BeApproximately(54.79, 0.01d);
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
            boundary.ExteriorRing.GetCoordinateN(0).X.Should().BeApproximately(3021312.09, 0.01d);
            boundary.ExteriorRing.GetCoordinateN(0).Y.Should().BeApproximately(375417.28, 0.01d);

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
            boundary.ExteriorRing.GetCoordinateN(0).X.Should().BeApproximately(-138.45, 0.01d);
            boundary.ExteriorRing.GetCoordinateN(0).Y.Should().BeApproximately(44.20, 0.01d);
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
