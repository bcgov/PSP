using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using FluentAssertions;
using Pims.Core.Extensions;
using Xunit;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class StringExtensionsTest
    {
        #region Tests
        #region GetFirstLetterOfEachWord
        [Fact]
        public void GetFirstLetterOfEachWord_KeepLower()
        {
            // Arrange
            var value = "Test a Story";

            // Act
            var result = value.GetFirstLetterOfEachWord(false);

            // Assert
            result.Should().Be("TaS");
        }

        [Fact]
        public void GetFirstLetterOfEachWord_DefaultUpper()
        {
            // Arrange
            var value = "Test a Story";

            // Act
            var result = value.GetFirstLetterOfEachWord();

            // Assert
            result.Should().Be("TAS");
        }

        [Fact]
        public void GetFirstLetterOfEachWord_Upper()
        {
            // Arrange
            var value = "Test a Story";

            // Act
            var result = value.GetFirstLetterOfEachWord(true);

            // Assert
            result.Should().Be("TAS");
        }
        #endregion

        #region IsProduction
        [Fact]
        public void IsProduction()
        {
            // Arrange
            var value = "Production";

            // Act
            var result = value.IsProduction();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsProduction_Case()
        {
            // Arrange
            var value = "production";

            // Act
            var result = value.IsProduction();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsProduction_False()
        {
            // Arrange
            var value = "Production2";

            // Act
            var result = value.IsProduction();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsProduction_Null()
        {
            // Arrange
            var value = (string)null;

            // Act
            var result = value.IsProduction();

            // Assert
            result.Should().BeFalse();
        }
        #endregion

        #region FormatAsPostal
        [Fact]
        public void FormatAsPostal_Null()
        {
            // Arrange
            var value = (string)null;

            // Act
            var result = value.FormatAsPostal();

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void FormatAsPostal_Empty()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var result = value.FormatAsPostal();

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void FormatAsPostal_LessThan6()
        {
            // Arrange
            var value = "12345";

            // Act
            var result = value.FormatAsPostal();

            // Assert
            result.Should().Be("12345");
        }

        [Fact]
        public void FormatAsPostal_MoreThan6()
        {
            // Arrange
            var value = "1234567";

            // Act
            var result = value.FormatAsPostal();

            // Assert
            result.Should().Be("1234567");
        }

        [Fact]
        public void FormatAsPostal_Has6()
        {
            // Arrange
            var value = "123456";

            // Act
            var result = value.FormatAsPostal();

            // Assert
            result.Should().Be("123 456");
        }

        [Fact]
        public void FormatAsPostal_ContainsSpace()
        {
            // Arrange
            var value = "123 45";

            // Act
            var result = value.FormatAsPostal();

            // Assert
            result.Should().Be("123 45");
        }
        #endregion

        #region LowercaseFirstCharacter
        [Fact]
        public void LowercaseFirstCharacter()
        {
            // Arrange
            var value = "Test";

            // Act
            var result = value.LowercaseFirstCharacter();

            // Assert
            result.Should().Be("test");
        }

        [Fact]
        public void LowercaseFirstCharacter_Empty()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var result = value.LowercaseFirstCharacter();

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void LowercaseFirstCharacter_Whitespace()
        {
            // Arrange
            var value = " ";

            // Act
            var result = value.LowercaseFirstCharacter();

            // Assert
            result.Should().Be(" ");
        }

        [Fact]
        public void LowercaseFirstCharacter_Null()
        {
            // Arrange
            var value = (string)null;

            // Act
            var result = value.LowercaseFirstCharacter();

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region GetHttpMethod
        [Fact]
        public void GetHttpMethod_Null()
        {
            // Arrange
            var value = (string)null;

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void GetHttpMethod_Empty()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void GetHttpMethod_Whitespace()
        {
            // Arrange
            var value = " ";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void GetHttpMethod_Default()
        {
            // Arrange
            var value = "test";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void GetHttpMethod_Post()
        {
            // Arrange
            var value = "post";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Post);
        }

        [Fact]
        public void GetHttpMethod_GetUpper()
        {
            // Arrange
            var value = "Get";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Get);
        }

        [Fact]
        public void GetHttpMethod_Get()
        {
            // Arrange
            var value = "get";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Get);
        }

        [Fact]
        public void GetHttpMethod_DeleteUpper()
        {
            // Arrange
            var value = "Delete";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Delete);
        }

        [Fact]
        public void GetHttpMethod_Delete()
        {
            // Arrange
            var value = "delete";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Delete);
        }

        [Fact]
        public void GetHttpMethod_PutUpper()
        {
            // Arrange
            var value = "Put";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Put);
        }

        [Fact]
        public void GetHttpMethod_Put()
        {
            // Arrange
            var value = "put";

            // Act
            var result = value.GetHttpMethod();

            // Assert
            result.Should().Be(HttpMethod.Put);
        }
        #endregion

        #region ConvertToUTF8
        [Fact]
        public void ConvertToUTF8()
        {
            // Arrange
            var value = "a value";

            // Act
            var result = value.ConvertToUTF8();

            // Assert
            result.Should().Be(value);
        }

        [Fact]
        public void ConvertToUTF8_ReplaceLineBreaks()
        {
            // Arrange
            var value = "a value\r\ntest line two";

            // Act
            var result = value.ConvertToUTF8(true);

            // Assert
            result.Should().Be("a value test line two");
        }
        #endregion

        #region Truncate
        [Fact]
        public void Truncate()
        {
            // Arrange
            var value = "a value";

            // Act
            var result = value.Truncate(1);

            // Assert
            result.Should().Be("a");
        }

        [Fact]
        public void Truncate_Null()
        {
            // Arrange
            var value = (string)null;

            // Act
            var result = value.Truncate(1);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Truncate_Empty()
        {
            // Arrange
            var value = string.Empty;

            // Act
            var result = value.Truncate(1);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Truncate_Whitespace()
        {
            // Arrange
            var value = " ";

            // Act
            var result = value.Truncate(1);

            // Assert
            result.Should().Be(" ");
        }
        #endregion
        #endregion
    }
}
