using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json;
using Pims.Core.Converters;
using Pims.Core.Json;
using Xunit;

namespace Pims.Dal.Test.Core.Converters
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class EnumValueJsonConverterTest
    {
        #region Data
        public enum TestEnum
        {
            [EnumValue("utf-8")]
            Utf8,
            [EnumValue("binary")]
            Binary,
            [EnumValue("hex")]
            Hex,
            [EnumValue("base64")]
            Base64,
        }

        public static IEnumerable<object[]> WriteData = new List<object[]>()
        {
            new object[] { TestEnum.Utf8, "{\"test\":\"utf-8\"}" },
            new object[] { TestEnum.Base64, "{\"test\":\"base64\"}" },
        };

        public static IEnumerable<object[]> ReadData = new List<object[]>()
        {
            new object[] { "utf-8", TestEnum.Utf8 },
            new object[] { "utf8", TestEnum.Utf8 },
            new object[] { "Utf8", TestEnum.Utf8 },
            new object[] { "UTF8", TestEnum.Utf8 },
            new object[] { "base64", TestEnum.Base64 },
            new object[] { "Base64", TestEnum.Base64 },
            new object[] { "BASE64", TestEnum.Base64 },
        };
        #endregion

        #region Tests
        [Fact]
        public void CanConvert()
        {
            // Arrange
            var converter = new EnumValueJsonConverter<TestEnum>();

            // Act
            var result = converter.CanConvert(typeof(TestEnum));

            // Assert
            Assert.True(result);
        }

        [Theory]
        [MemberData(nameof(WriteData))]
        public void Write(TestEnum value, string expectedResult)
        {
            // Arrange
            var options = new JsonSerializerOptions();
            using var stream = new MemoryStream();
            using var writer = new Utf8JsonWriter(stream);
            var converter = new EnumValueJsonConverter<TestEnum>();

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
        public void Read(string value, TestEnum expectedResult)
        {
            // Arrange
            var options = new JsonSerializerOptions();

            var jsonUtf8Bytes = System.Text.Json.JsonSerializer.SerializeToUtf8Bytes(value, options);
            var reader = new Utf8JsonReader(jsonUtf8Bytes);
            var converter = new EnumValueJsonConverter<TestEnum>();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.String)
                    break;
            }

            // Act
            var result = converter.Read(ref reader, typeof(bool), options);

            // Assert
            Assert.Equal(expectedResult, result);
        }
        #endregion
    }
}
