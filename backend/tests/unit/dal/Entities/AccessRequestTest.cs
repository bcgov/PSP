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
            accessRequest.Agencies.Should().BeEmpty();
            accessRequest.Roles.Should().BeEmpty();
            accessRequest.Status.Should().Be(AccessRequestStatus.OnHold);
        }

        [Fact]
        public void AccessRequest_User_Constructor()
        {
            // Arrange
            var user = new User();

            // Act
            var accessRequest = new AccessRequest(user);

            // Assert
            accessRequest.Agencies.Should().BeEmpty();
            accessRequest.Roles.Should().BeEmpty();
            accessRequest.Status.Should().Be(AccessRequestStatus.OnHold);
            accessRequest.User.Should().Be(user);
        }
        #endregion
    }
}
