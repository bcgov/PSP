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
        public void Transform_Wgs84_BcAlbers()
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
        public void Transform_BcAlbers_Wgs84()
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
        public void Transform_Not_Supported()
        {
            // Arrange
            var location = new Coordinate(924033.50, 1088851.50);

            // Act
            // Assert
            this._service.IsCoordinateSystemSupported(900913).Should().BeFalse();
            Assert.Throws<InvalidOperationException>(() => this._service.TransformCoordinates(900913, 4326, location));
        }
        #endregion
    }
}
