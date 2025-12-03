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
    public class OrganizationFilterTest
    {
        #region Tests
        [Fact]
        public void OrganizationFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new OrganizationFilter();

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(10);
            filter.Id.Should().BeNull();
            filter.ParentId.Should().Be(0);
            filter.IsDisabled.Should().BeNull();
            filter.Name.Should().BeNull();
            filter.Sort.Should().BeNull();
        }

        [Fact]
        public void OrganizationFilter_Constructor_01()
        {
            // Arrange
            var page = 2;
            var quantity = 5;

            // Act
            var filter = new OrganizationFilter(page, quantity);

            // Assert
            filter.Page.Should().Be(page);
            filter.Quantity.Should().Be(quantity);
            filter.Id.Should().BeNull();
            filter.ParentId.Should().Be(0);
            filter.IsDisabled.Should().BeNull();
            filter.Name.Should().BeNull();
            filter.Sort.Should().BeNull();
        }

        [Fact]
        public void OrganizationFilter_Constructor_02()
        {
            // Arrange
            var page = 2;
            var quantity = 5;
            var id = 3;
            var parentId = 3;
            var name = "name";
            var isDisabled = true;
            var sort = new[] { "one" };

            // Act
            var filter = new OrganizationFilter(page, quantity, id, name, parentId, isDisabled, sort);

            // Assert
            filter.Page.Should().Be(page);
            filter.Quantity.Should().Be(quantity);
            filter.Id.Should().Be(id);
            filter.ParentId.Should().Be(parentId);
            filter.IsDisabled.Should().Be(isDisabled);
            filter.Name.Should().Be(name);
            filter.Sort.Should().BeEquivalentTo(sort);
        }

        [Fact]
        public void OrganizationFilter_Constructor_03()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&id=3&parentId=6&name=test&isDisabled=true&sort=one,two");

            // Act
            var filter = new OrganizationFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.Id.Should().Be(3);
            filter.ParentId.Should().Be(6);
            filter.IsDisabled.Should().Be(true);
            filter.Name.Should().Be("test");
            filter.Sort.Should().BeEquivalentTo(new[] { "one", "two" });
        }
        #endregion
    }
}
