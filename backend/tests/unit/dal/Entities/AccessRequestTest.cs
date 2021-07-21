using FluentAssertions;
using Pims.Dal.Entities;
using System.Diagnostics.CodeAnalysis;
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
            var accessRequest = new AccessRequest();

            // Assert
            accessRequest.User.Should().BeNull();
            accessRequest.Role.Should().BeNull();
            accessRequest.Status.Should().BeNull();
            accessRequest.Organizations.Should().BeEmpty();
        }

        [Fact]
        public void AccessRequest_User_Constructor()
        {
            // Arrange
            var user = new User();
            var role = new Role();
            var status = new AccessRequestStatusType();

            // Act
            var accessRequest = new AccessRequest(user, role, status);

            // Assert
            accessRequest.User.Should().Be(user);
            accessRequest.Role.Should().Be(role);
            accessRequest.Status.Should().Be(status);
            accessRequest.Organizations.Should().BeEmpty();
        }
        #endregion
    }
}
