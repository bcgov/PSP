using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestTest
    {
        #region Tests
        [Fact]
        public void AccessRequest_DefaultConstructor()
        {
            // Arrange
            // Act
            var accessRequest = new PimsAccessRequest();

            // Assert
            accessRequest.User.Should().BeNull();
            accessRequest.Role.Should().BeNull();
            accessRequest.AccessRequestStatusTypeCode.Should().BeNull();
            accessRequest.GetOrganizations().Should().BeEmpty();
        }

        [Fact]
        public void AccessRequest_User_Constructor()
        {
            // Arrange
            var user = new PimsUser();
            var role = new PimsRole();
            var status = new PimsAccessRequestStatusType();

            // Act
            var accessRequest = new PimsAccessRequest(user, role, status);

            // Assert
            accessRequest.User.Should().Be(user);
            accessRequest.Role.Should().Be(role);
            accessRequest.AccessRequestStatusTypeCodeNavigation.Should().Be(status);
            accessRequest.GetOrganizations().Should().BeEmpty();
        }
        #endregion
    }
}
