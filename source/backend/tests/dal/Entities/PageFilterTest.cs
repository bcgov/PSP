using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Dal.Entities.Models;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class PageFilterTest
    {
        #region Tests
        [Fact]
        public void PageFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new TestFilter();

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(10);
        }

        [Fact]
        public void PageFilter_Constructor_01()
        {
            // Arrange
            var page = 2;
            var quantity = 4;

            // Act
            var filter = new TestFilter(page, quantity);

            // Assert
            filter.Page.Should().Be(page);
            filter.Quantity.Should().Be(quantity);
            filter.Sort.Should().BeNull();
        }

        [Fact]
        public void PageFilter_Constructor_01_WithSort()
        {
            // Arrange
            var page = 2;
            var quantity = 4;
            var sort = new[] { "one", "two" };

            // Act
            var filter = new TestFilter(page, quantity, sort);

            // Assert
            filter.Page.Should().Be(page);
            filter.Quantity.Should().Be(quantity);
            filter.Sort.Should().BeEquivalentTo(sort);
        }

        [Fact]
        public void PageFilter_Constructor_03()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&sort=one,two");

            // Act
            var filter = new TestFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        [Fact]
        public void PageFilter_Constructor_03_Count()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&count=3&sort=one,two");

            // Act
            var filter = new TestFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        [Fact]
        public void PageFilter_Constructor_03_DefaultQuantity()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&sort=one,two");

            // Act
            var filter = new TestFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(10);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        [Fact]
        public void PageFilter_Constructor_03_StartIndex()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?startIndex=0&quantity=3&sort=one,two");

            // Act
            var filter = new TestFilter(query);

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(3);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        [Fact]
        public void PageFilter_Constructor_03_StartIndexLessThanQuantity()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?startIndex=2&quantity=3&sort=one,two");

            // Act
            var filter = new TestFilter(query);

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(3);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        [Fact]
        public void PageFilter_Constructor_03_StartIndexMoreThanQuantity()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?startIndex=10&quantity=3&sort=one,two");

            // Act
            var filter = new TestFilter(query);

            // Assert
            filter.Page.Should().Be(3);
            filter.Quantity.Should().Be(3);
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        #region IsValid
        [Fact]
        public void PageFilter_IsValid()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=3");
            var filter = new TestFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PageFilter_Base_IsValid()
        {
            // Arrange
            var filter = new TestFilter();

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PageFilter_False()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=0");
            var filter = new TestFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeFalse();
        }
        #endregion
        #endregion

        #region Test Class
        class TestFilter : PageFilter
        {
            public TestFilter() : base() { }
            public TestFilter(int page, int quantity, string[] sort = null) : base(page, quantity, sort) { }
            public TestFilter(Dictionary<string, Microsoft.Extensions.Primitives.StringValues> query) : base(query) { }
        }
        #endregion
    }
}
