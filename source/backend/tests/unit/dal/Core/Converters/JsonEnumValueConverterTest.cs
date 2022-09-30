using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;
using FluentAssertions;
using NetTopologySuite.Geometries;
using Pims.Core.Converters;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Core.Converters
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class JsonEnumValueConverterTest
    {
        #region Data
        public readonly static IEnumerable<object[]> WriteData = new List<object[]>()
        {
            new object[] { new Point(new Coordinate(1, 1)), "{\"test\":[1,1]}" },
            new object[] { new Polygon(new LinearRing(new Coordinate[] { new Coordinate(1, 1), new Coordinate(1, 2), new Coordinate(1, 3), new Coordinate(1, 1) })), "{\"test\":[[1,1],[1,2],[1,3],[1,1]]}" },
        };

        public readonly static IEnumerable<object[]> ReadData = new List<object[]>()
        {
            new object[] { "1,1", new Point(new Coordinate(1, 1)), typeof(Point) },
            new object[] { "[[1,1],[1,2],[1,3],[1,1]]", null, typeof(Polygon) },
        };
        #endregion

        #region Tests
        [Fact]
        public void CanConvert()
        {
            // Arrange
            var converter = new JsonEnumValueConverter();

            // Act
            var result = converter.CanConvert(typeof(Permissions));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CreateConverter()
        {
            // Arrange
            var factory = new JsonEnumValueConverter(JsonNamingPolicy.CamelCase);
            var jsonOptions = new JsonSerializerOptions();

            // Act
            var converter = factory.CreateConverter(typeof(Permissions), jsonOptions);

            // Assert
            converter.Should().NotBeNull();
            converter.Should().BeAssignableTo<JsonConverter<Permissions>>();
            converter.GetType().Name.Should().Be("EnumConverter`1");
        }
        #endregion
    }
}
