using FluentAssertions;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Keycloak;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Libraries.Keycloak
{
    [Trait("category", "unit")]
    [Trait("category", "keycloak")]
    [Trait("group", "keycloak")]
    [Trait("group", "user")]
    public partial class PimsKeycloakUserServiceTest
    {
        #region UpdateUserAsync
        /// <summary>
        /// Validate that the user properties are updated, the keycloak groups are added and removed.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateUserAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var removeRole = euser.PimsUserRoles.First().Role;
            removeRole.RoleId = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.GuidIdentifierValue.Value,
                Username = euser.BusinessIdentifierValue,
                Groups = new string[] { removeRole.KeycloakGroupId.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());
            keycloakServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()));

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.Get(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Pims.Dal.Entities.PimsUser>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.RemoveRole(It.IsAny<Pims.Dal.Entities.PimsUser>(), removeRole.RoleId)).Returns((Pims.Dal.Entities.PimsUser eUser, long roleId) =>
            {
                var userRole = eUser.PimsUserRoles.FirstOrDefault(r => r.RoleId == roleId);
                eUser.PimsUserRoles.Remove(userRole);
                return eUser;
            });
            pimsServiceMock.Setup(m => m.Role.Find(removeRole.RoleId)).Returns(removeRole);

            var user = EntityHelper.CreateUser(euser.Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addUserRole = user.PimsUserRoles.First();
            addUserRole.RoleId = 2;
            addUserRole.Role.RoleId = 2;
            addUserRole.Role.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addUserRole.Role.RoleId)).Returns(addUserRole.Role);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.PimsOrganization>());
            pimsServiceMock.Setup(m => m.User.GetOrganizations(It.IsAny<Guid>())).Returns(euser.GetOrganizations().Select(a => a.Id));

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.Id.Should().Be(user.Id);
            result.BusinessIdentifierValue.Should().Be(user.BusinessIdentifierValue);
            result.Person.FirstName.Should().Be(user.Person.FirstName);
            result.Person.Surname.Should().Be(user.Person.Surname);
            result.GetOrganizations().Count.Should().Be(euser.GetOrganizations().Count);
            result.PimsUserRoles.Count.Should().Be(1);
            result.PimsUserRoles.First().Role.KeycloakGroupId.Should().Be(user.GetRoles().First().KeycloakGroupId);

            keycloakServiceMock.Verify(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value), Times.Once);
            keycloakServiceMock.Verify(m => m.RemoveGroupFromUserAsync(euser.GuidIdentifierValue.Value, new Guid(kuser.Groups.First())), Times.Once);
            keycloakServiceMock.Verify(m => m.AddGroupToUserAsync(euser.GuidIdentifierValue.Value, addUserRole.Role.KeycloakGroupId.Value), Times.Once);
            pimsServiceMock.Verify(m => m.User.UpdateOnly(It.IsAny<Entity.PimsUser>()), Times.Once);
            pimsServiceMock.Verify(m => m.User.GetOrganizations(It.IsAny<Guid>()), Times.Once);
            pimsServiceMock.Verify(m => m.User.RemoveRole(It.IsAny<Entity.PimsUser>(), removeRole.RoleId), Times.Once);
            keycloakServiceMock.Verify(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()), Times.Once);
        }

        /// <summary>
        /// Validate when user does not exist in keycloak.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateUserAsync_MissingKeycloakUser()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync((Pims.Keycloak.Models.UserModel)null);

            // Act
            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateUserAsync(euser));
        }

        /// <summary>
        /// Validate that the keycloak user matches PIMS.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateUserAsync_KeycloakUserDoesNotMatch()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var removeRole = euser.GetRoles().First();
            removeRole.RoleId = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.GuidIdentifierValue.Value,
                Username = "keycloak username",
                Groups = new[] { euser.GuidIdentifierValue.Value.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());
            keycloakServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()));

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.Get(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Role.Find(removeRole.RoleId)).Returns(removeRole);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Pims.Dal.Entities.PimsUser>())).Returns(euser);

            var user = EntityHelper.CreateUser(1, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.GetRoles().First();
            addRole.RoleId = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.RoleId)).Returns(addRole);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.PimsOrganization>());
            pimsServiceMock.Setup(m => m.User.GetOrganizations(It.IsAny<Guid>())).Returns(euser.GetOrganizations().Select(a => a.Id));

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.BusinessIdentifierValue.Should().Be("keycloak username");
        }

        /// <summary>
        /// Validate that the role added to the user exists in PIMS.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateUserAsync_AddRoleDoesNotExistInPims()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var removeRole = euser.GetRoles().First();
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.GuidIdentifierValue.Value,
                Username = euser.BusinessIdentifierValue,
                Groups = new string[] { removeRole.KeycloakGroupId.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.Get(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.PimsOrganization>());
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.PimsOrganization>());

            var user = EntityHelper.CreateUser(euser.Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.GetRoles().First();
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.RoleId)).Returns<Entity.PimsRole>(null);

            // Act
            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateUserAsync(euser));
        }

        /// <summary>
        /// Validate that the role removed from the user exists in PIMS.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdateUserAsync_RemoveRoleDoesNotExistInPims()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var removeRole = euser.GetRoles().First();
            removeRole.RoleId = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.GuidIdentifierValue.Value,
                Username = euser.BusinessIdentifierValue,
                Groups = new string[] { removeRole.KeycloakGroupId.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());
            keycloakServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()));

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Role.Find(removeRole.RoleId)).Returns<Entity.PimsRole>(null);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.PimsOrganization>());

            var user = EntityHelper.CreateUser(euser.Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.GetRoles().First();
            addRole.RoleId = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.RoleId)).Returns(addRole);

            // Act
            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateUserAsync(euser));
        }
        #endregion
    }
}
