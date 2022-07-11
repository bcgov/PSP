using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using NetTopologySuite.Geometries;
using Pims.Core.Converters;
using Xunit;

namespace Pims.Dal.Test.Core.Converters
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class GeometryJsonConverterTest
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
            var converter = new GeometryJsonConverter();

            // Act
            var result = converter.CanConvert(typeof(Geometry));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(WriteData))]
        public void Write(Geometry value, string expectedResult)
        {
            // Arrange
            var options = new JsonSerializerOptions();
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            var converter = new GeometryJsonConverter();

            writer.WriteStartObject();
            writer.WritePropertyName("test");

            // Act
            converter.Write(writer, value, options);

            // Assert
            writer.WriteEndObject();
            writer.Flush();
            var result = Encoding.UTF8.GetString(stream.ToArray());
            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [MemberData(nameof(ReadData))]
        public void Read(string value, Geometry expectedResult, Type type)
        {
            // Arrange
            var options = new JsonSerializerOptions();

            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, options);
            var reader = new Utf8JsonReader(jsonUtf8Bytes);
            var converter = new GeometryJsonConverter();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.String)
                    break;
            }

            // Act
            var result = converter.Read(ref reader, type, options);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
