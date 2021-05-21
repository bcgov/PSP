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
            result.Id.Should().Be(0);
            result.Name.Should().BeNull();
        }

        [Fact]
        public void LookupEntity_Constructor_01()
        {
            // Arrange
            var uid = 1;
            var name = "name";

            // Act
            var result = new TestClass(uid, name);

            // Assert
            result.Id.Should().Be(uid);
            result.Name.Should().Be(name);
        }


        [Theory]
        [MemberData(nameof(Lookups))]
        public void LookupEntity_Constructor_01_ArgumentException(string name)
        {
            // Arrange
            var uid = 1;

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => new TestClass(uid, name));
        }
        #endregion

        #region Test Classes
        class TestClass : LookupEntity<int>
        {
            public TestClass() : base() { }
            public TestClass(int id, string name) : base(id, name) { }
        }
        #endregion
    }
}
