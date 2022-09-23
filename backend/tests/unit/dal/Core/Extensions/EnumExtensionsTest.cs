using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Extensions;
using Xunit;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class EnumExtensionsTest
    {
        #region Data
        public readonly static IEnumerable<object[]> EnumInData = new List<object[]>()
        {
            new object[] { TestEnum.Utf8, new[] { TestEnum.Utf8, TestEnum.Hex }, true },
            new object[] { TestEnum.Utf8, new[] { TestEnum.Utf8 }, true },
            new object[] { TestEnum.Utf8, new[] { TestEnum.Binary, TestEnum.Hex }, false },
            new object[] { TestEnum.Utf8, new[] { TestEnum.Binary }, false },
            new object[] { TestEnum.Utf8, new TestEnum[0], false },
        };
        #endregion

        #region Tests
        #region ToLower
        [Fact]
        public void Enum_ToLower()
        {
            // Arrange
            var encoding = TestEnum.Utf8;

            // Act
            var result = encoding.ToLower();

            // Assert
            result.Should().Be("utf8");
        }
        #endregion

        #region In
        [Theory]
        [MemberData(nameof(EnumInData))]
        public void Enum_In(TestEnum value, TestEnum[] inValues, bool expectedResult)
        {
            // Arrange
            // Act
            var result = value.In(inValues);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
        #endregion

        public enum TestEnum
        {
            Utf8,
            Binary,
            Hex,
        }
    }
}
