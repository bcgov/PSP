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
    public class LookupEntityTest
    {
        #region Variables
        public static IEnumerable<object[]> Lookups =>
            new List<object[]>
            {
                new object[] { "" },
                new object[] { " " },
                new object[] { null}
            };
        #endregion

        #region Tests
        [Fact]
        public void LookupEntity_Default_Constructor()
        {
            // Arrange
            // Act
            var result = new TestClass();

            // Assert
            result.Name.Should().BeNull();
        }

        [Fact]
        public void LookupEntity_Constructor_01()
        {
            // Arrange
            var name = "name";

            // Act
            var result = new TestClass(name);

            // Assert
            result.Name.Should().Be(name);
        }


        [Theory]
        [MemberData(nameof(Lookups))]
        public void LookupEntity_Constructor_01_ArgumentException(string name)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new TestClass(name));
        }
        #endregion

        #region Test Classes
        class TestClass : LookupEntity
        {
            public TestClass() : base() { }
            public TestClass(string name) : base(name) { }
        }
        #endregion
    }
}
