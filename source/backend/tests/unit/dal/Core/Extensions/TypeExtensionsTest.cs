using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Pims.Core.Extensions;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Test.Core.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]
    public class TypeExtensionsTest
    {
        #region Tests

        #region GetCachedProperties
        [Fact]
        public void GetCachedProperties()
        {
            // Arrange
            var type = typeof(TestClass);

            // Act
            var result = type.GetCachedProperties();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(3);
        }
        #endregion

        #region GetCachedPropertiesAsDictionary
        [Fact]
        public void GetCachedPropertiesAsDictionary()
        {
            // Arrange
            var type = typeof(TestClass);

            // Act
            var result = type.GetCachedPropertiesAsDictionary();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(3);
        }
        #endregion

        #region GetCachedConstructors
        [Fact]
        public void GetCachedConstructors()
        {
            // Arrange
            var type = typeof(TestClass);

            // Act
            var result = type.GetCachedConstructors();

            // Assert
            result.Should().NotBeNull();
            result.Count().Should().Be(2);
        }
        #endregion

        #region IsEnumerable
        [Fact]
        public void IsEnumerable()
        {
            // Arrange
            var value = new string[0];

            // Act
            var result = value.GetType().IsEnumerable();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsEnumerable_IsString()
        {
            // Arrange
            var value = "test";

            // Act
            var result = value.GetType().IsEnumerable();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsEnumerable_NotString()
        {
            // Arrange
            var value = "test";

            // Act
            var result = value.GetType().IsEnumerable(true);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void IsEnumerable_IncludeString()
        {
            // Arrange
            var value = "test";

            // Act
            var result = value.GetType().IsEnumerable(false);

            // Assert
            result.Should().BeTrue();
        }
        #endregion

        #region GetItemType
        [Fact]
        public void GetItemType_String()
        {
            // Arrange
            var value = new[] { new string[0] };

            // Act
            var result = value.GetType().GetItemType();

            // Assert
            result.Should().BeAssignableTo(typeof(string[]));
        }

        [Fact]
        public void GetItemType_List()
        {
            // Arrange
            var value = new[] { new List<string>() };

            // Act
            var result = value.GetType().GetItemType();

            // Assert
            result.Should().BeAssignableTo(typeof(List<string>));
        }

        [Fact]
        public void GetItemType_Int()
        {
            // Arrange
            var value = 1;

            // Act
            var result = value.GetType().GetItemType();

            // Assert
            result.Should().BeAssignableTo(typeof(int));
        }
        #endregion

        #region FindMethod
        [Fact]
        public void FindMethod()
        {
            // Arrange
            var value = new TestClass();

            // Act
            var result = value.GetType().FindMethod("GetValue");

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void FindMethod_Null()
        {
            // Arrange
            var value = new TestClass();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => value.GetType().FindMethod(null));
        }

        [Fact]
        public void FindMethod_Empty()
        {
            // Arrange
            var value = new TestClass();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => value.GetType().FindMethod(string.Empty));
        }

        [Fact]
        public void FindMethod_Whitespace()
        {
            // Arrange
            var value = new TestClass();

            // Act
            // Assert
            Assert.Throws<ArgumentException>(() => value.GetType().FindMethod(" "));
        }
        #endregion

        #endregion

        #region Test Classes
        class TestClass
        {
            public int P1 { get; set; }
            public int P2 { get; }
            public int P3 { get; }
            protected int P4 { get; set; }

            public TestClass()
            {
                this.P1 = 1;
                this.P2 = 2;
                this.P3 = 3;
                this.P4 = 4;
            }

            public TestClass(int p1, int p2, int p3, int p4)
            {
                this.P1 = p1;
                this.P2 = p2;
                this.P3 = p3;
                this.P4 = p4;
            }

            public string GetValue()
            {
                return string.Empty;
            }
        }

        struct TestStruct
        {
            public int P1 { get; }
        }
        #endregion
    }
}
