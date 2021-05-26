using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class ProjectNotificationFilterTest
    {
        #region Tests
        [Fact]
        public void ProjectNotificationFilter_Default_Constructor()
        {
            // Arrange
            // Act
            var filter = new ProjectNotificationFilter();

            // Assert
            filter.Page.Should().Be(1);
            filter.Quantity.Should().Be(10);
            filter.ProjectNumber.Should().BeNull();
            filter.ProjectId.Should().BeNull();
            filter.AgencyId.Should().BeNull();
            filter.Tag.Should().BeNull();
            filter.Status.Should().BeNull();
            filter.To.Should().BeNull();
            filter.Subject.Should().BeNull();
        }

        [Fact]
        public void ProjectNotificationFilter_Constructor_01()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=2&quantity=3&projectNumber=234&projectId=34&agencyId=3&tag=tag&status=3,4&to=to&subject=subject&sort=one,two");

            // Act
            var filter = new ProjectNotificationFilter(query);

            // Assert
            filter.Page.Should().Be(2);
            filter.Quantity.Should().Be(3);
            filter.ProjectNumber.Should().Be("234");
            filter.ProjectId.Should().Be(34);
            filter.AgencyId.Should().Be(3);
            filter.Tag.Should().Be("tag");
            filter.Status.Should().BeEquivalentTo(new[] { NotificationStatus.Failed, NotificationStatus.Completed });
            filter.To.Should().Be("to");
            filter.Subject.Should().Be("subject");
        }

        #region IsValid
        [Fact]
        public void ProjectNotificationFilter_IsValid()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?projectNumber=32344");
            var filter = new ProjectNotificationFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ProjectNotificationFilter_Base_IsValid()
        {
            // Arrange
            var filter = new ProjectNotificationFilter();

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void ProjectNotificationFilter_False()
        {
            // Arrange
            var query = Microsoft.AspNetCore.WebUtilities.QueryHelpers.ParseQuery("?page=0");
            var filter = new ProjectNotificationFilter(query);

            // Act
            var result = filter.IsValid();

            // Assert
            result.Should().BeFalse();
        }
        #endregion
        #endregion
    }
}
