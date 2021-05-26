using FluentAssertions;
using Pims.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class CodeEntityTest
    {
        #region Variables
        public static IEnumerable<object[]> Codes =>
            new List<object[]>
            {
                new object[] { "" },
                new object[] { " " },
                new object[] { null}
            };
        #endregion

        #region Tests
        [Fact]
        public void CodeEntity_Default_Constructor()
        {
            // Arrange
            // Act
            var result = new TestClass();

            // Assert
            result.Code.Should().BeNull();
            result.Name.Should().BeNull();
        }

        [Fact]
        public void CodeEntity_Constructor_01()
        {
            // Arrange
            var uid = 1;
            string code = "code";
            var name = "name";

            // Act
            var result = new TestClass(uid, code, name);

            // Assert
            result.Id.Should().Be(uid);
            result.Code.Should().Be(code);
            result.Name.Should().Be(name);
        }


        [Theory]
        [MemberData(nameof(Codes))]
        public void CodeEntity_Constructor_01_ArgumentException(string code)
        {
            // Arrange
            var uid = 1;
            var name = "name";

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new TestClass(uid, code, name));
        }
        #endregion

        #region Test Classes
        class TestClass : CodeEntity<int>
        {
            public TestClass() : base() { }
            public TestClass(int id, string code, string name) : base(id, code, name) { }
        }
        #endregion
    }
}
