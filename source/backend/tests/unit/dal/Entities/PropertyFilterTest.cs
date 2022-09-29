using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NetTopologySuite.Geometries;
using Pims.Dal.Entities.Models;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class PropertyFilterTest
    {
        #region Tests
        [Fact]
        public void PropertyFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new PropertyFilter();

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(10);
            filter.PinOrPid.Should().BeNull();
        }

        [Fact]
        public void PropertyFilter_Constructor_03()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&pinOrPid=323423&sort=one,two");

            // Act
            var filter = new PropertyFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.PinOrPid.Should().Be("323423");
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }

        #region IsValid
        [Fact]
        public void PropertyFilter_IsValid()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?pinOrPid=323423");
            var filter = new PropertyFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PropertyFilter_Base_IsValid()
        {
            // Arrange
            var filter = new PropertyFilter();

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void PropertyFilter_False()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=0");
            var filter = new PropertyFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeFalse();
        }
        #endregion
        #endregion
    }
}
