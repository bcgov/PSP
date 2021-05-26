using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class AccessRequestRoleTest
    {
        #region Tests
        [Fact]
        public void AccessRequestRole_Id_Constructor()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);
            var role = EntityHelper.CreateRole("role");

            // Act
            var accessRequestAgency = new AccessRequestRole(accessRequest.Id, role.Id);

            // Assert
            accessRequestAgency.AccessRequestId.Should().Be(accessRequest.Id);
            accessRequestAgency.RoleId.Should().Be(role.Id);
        }

        [Fact]
        public void AccessRequestRole_Object_Constructor()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);
            var role = EntityHelper.CreateRole("role");

            // Act
            var accessRequestAgency = new AccessRequestRole(accessRequest, role);

            // Assert
            accessRequestAgency.AccessRequestId.Should().Be(accessRequest.Id);
            accessRequestAgency.AccessRequest.Should().Be(accessRequest);
            accessRequestAgency.RoleId.Should().Be(role.Id);
            accessRequestAgency.Role.Should().Be(role);
        }

        [Fact]
        public void AccessRequestRole_AccessRequest_Constructor_ArgumentNullException()
        {
            // Arrange
            var role = EntityHelper.CreateRole("role");

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AccessRequestRole(null, role));
        }

        [Fact]
        public void AccessRequestRole_Role_Constructor_ArgumentNullException()
        {
            // Arrange
            var accessRequest = EntityHelper.CreateAccessRequest(5);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new AccessRequestRole(accessRequest, null));
        }
        #endregion
    }
}
