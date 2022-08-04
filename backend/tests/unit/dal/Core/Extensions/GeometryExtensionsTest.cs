using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using NetTopologySuite.Geometries;
using Pims.Core.Extensions;
using Xunit;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class GeometryExtensionsTest
    {
        #region Tests
        #region ToPolygon Envelope
        [Fact]
        public void ToPolygon_Envelope()
        {
            // Arrange
            var envelope = new Envelope(1, 3, 2, 4);

            // Act
            var result = envelope.ToPolygon();

            // Assert
            result.SRID.Should().Be(4326);
            result.Coordinates.First().X.Should().Be(1.0);
            result.Coordinates.First().Y.Should().Be(2.0);
            result.Coordinates.Last().X.Should().Be(1.0);
            result.Coordinates.Last().Y.Should().Be(2.0);
        }

        [Fact]
        public void ToPolygon_NullEnvelope()
        {
            // Arrange
            // Act
            var result = ((Envelope)null).ToPolygon();

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region ToPolygon Coordinates
        [Fact]
        public void ToPolygon_OneCoordinates()
        {
            // Arrange
            var envelope = new Coordinate[] { new Coordinate(1, 1) };

            // Act
            var result = envelope.ToPolygon();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToPolygon_Null()
        {
            // Arrange
            // Act
            var result = ((Coordinate[])null).ToPolygon();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void ToPolygon_TwoCoordinates()
        {
            // Arrange
            var envelope = new Coordinate[] { new Coordinate(1, 1), new Coordinate(2, 2) };

            // Act
            var result = envelope.ToPolygon();

            // Assert
            result.SRID.Should().Be(4326);
            result.Coordinates.First().X.Should().Be(1.0);
            result.Coordinates.First().Y.Should().Be(1.0);
            result.Coordinates.Next(1).X.Should().Be(2.0);
            result.Coordinates.Next(1).Y.Should().Be(1.0);
            result.Coordinates.Next(2).X.Should().Be(2.0);
            result.Coordinates.Next(2).Y.Should().Be(2.0);
            result.Coordinates.Last().X.Should().Be(1.0);
            result.Coordinates.Last().Y.Should().Be(1.0);
        }

        [Fact]
        public void ToPolygon_Coordinates_DoNotConnect()
        {
            // Arrange
            var envelope = new Coordinate[] { new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3) };

            // Act
            var result = envelope.ToPolygon();

            // Assert
            result.SRID.Should().Be(4326);
            result.Coordinates.First().X.Should().Be(1.0);
            result.Coordinates.First().Y.Should().Be(1.0);
            result.Coordinates.Next(1).X.Should().Be(2.0);
            result.Coordinates.Next(1).Y.Should().Be(2.0);
            result.Coordinates.Next(2).X.Should().Be(3.0);
            result.Coordinates.Next(2).Y.Should().Be(3.0);
            result.Coordinates.Last().X.Should().Be(1.0);
            result.Coordinates.Last().Y.Should().Be(1.0);
        }

        [Fact]
        public void ToPolygon_Coordinates()
        {
            // Arrange
            var envelope = new Coordinate[] { new Coordinate(1, 1), new Coordinate(2, 2), new Coordinate(3, 3), new Coordinate(1, 1) };

            // Act
            var result = envelope.ToPolygon();

            // Assert
            result.SRID.Should().Be(4326);
            result.Coordinates.First().X.Should().Be(1.0);
            result.Coordinates.First().Y.Should().Be(1.0);
            result.Coordinates.Next(1).X.Should().Be(2.0);
            result.Coordinates.Next(1).Y.Should().Be(2.0);
            result.Coordinates.Next(2).X.Should().Be(3.0);
            result.Coordinates.Next(2).Y.Should().Be(3.0);
            result.Coordinates.Last().X.Should().Be(1.0);
            result.Coordinates.Last().Y.Should().Be(1.0);
        }
        #endregion
        #endregion
    }
}
