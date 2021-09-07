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
            var removeRole = euser.Roles.First();
            removeRole.Id = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.KeycloakUserId.Value,
                Username = euser.BusinessIdentifier,
                Groups = new string[] { removeRole.KeycloakGroupId.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.KeycloakUserId.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());
            keycloakServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()));

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.Get(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Pims.Dal.Entities.User>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Role.Find(removeRole.Id)).Returns(removeRole);

            var user = EntityHelper.CreateUser(euser.Id, euser.KeycloakUserId.Value, euser.BusinessIdentifier, "new first name", "new last name");
            var addRole = user.Roles.First();
            addRole.Id = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.Id)).Returns(addRole);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.Organization>());
            pimsServiceMock.Setup(m => m.User.GetOrganizations(It.IsAny<Guid>())).Returns(euser.Organizations.Select(a => a.Id));

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.Id.Should().Be(user.Id);
            result.BusinessIdentifier.Should().Be(user.BusinessIdentifier);
            result.Person.FirstName.Should().Be(user.Person.FirstName);
            result.Person.Surname.Should().Be(user.Person.Surname);
            result.Organizations.Count.Should().Be(euser.Organizations.Count);
            result.RolesManyToMany.Count.Should().Be(1);
            result.RolesManyToMany.First().Role.KeycloakGroupId.Should().Be(user.Roles.First().KeycloakGroupId);

            keycloakServiceMock.Verify(m => m.GetUserGroupsAsync(euser.KeycloakUserId.Value), Times.Once);
            keycloakServiceMock.Verify(m => m.RemoveGroupFromUserAsync(euser.KeycloakUserId.Value, new Guid(kuser.Groups.First())), Times.Once);
            keycloakServiceMock.Verify(m => m.AddGroupToUserAsync(euser.KeycloakUserId.Value, addRole.KeycloakGroupId.Value), Times.Once);
            pimsServiceMock.Verify(m => m.User.UpdateOnly(It.IsAny<Entity.User>()), Times.Once);
            pimsServiceMock.Verify(m => m.User.GetOrganizations(It.IsAny<Guid>()), Times.Once);
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
            var removeRole = euser.Roles.First();
            removeRole.Id = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.KeycloakUserId.Value,
                Username = "keycloak username",
                Groups = new[] { euser.KeycloakUserId.Value.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.KeycloakUserId.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());
            keycloakServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()));

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.Get(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Role.Find(removeRole.Id)).Returns(removeRole);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Pims.Dal.Entities.User>())).Returns(euser);

            var user = EntityHelper.CreateUser(1, euser.KeycloakUserId.Value, euser.BusinessIdentifier, "new first name", "new last name");
            var addRole = user.Roles.First();
            addRole.Id = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.Id)).Returns(addRole);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.Organization>());
            pimsServiceMock.Setup(m => m.User.GetOrganizations(It.IsAny<Guid>())).Returns(euser.Organizations.Select(a => a.Id));

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.BusinessIdentifier.Should().Be("keycloak username");
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
            var removeRole = euser.Roles.First();
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.KeycloakUserId.Value,
                Username = euser.BusinessIdentifier,
                Groups = new string[] { removeRole.KeycloakGroupId.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.KeycloakUserId.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.Get(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Entity.User>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.Organization>());
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.Organization>());

            var user = EntityHelper.CreateUser(euser.Id, euser.KeycloakUserId.Value, euser.BusinessIdentifier, "new first name", "new last name");
            var addRole = user.Roles.First();
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.Id)).Returns<Entity.Role>(null);

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
            var removeRole = euser.Roles.First();
            removeRole.Id = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Id = euser.KeycloakUserId.Value,
                Username = euser.BusinessIdentifier,
                Groups = new string[] { removeRole.KeycloakGroupId.ToString() }
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.KeycloakUserId.Value))
                .ReturnsAsync(kuser.Groups.Select(g => new Pims.Keycloak.Models.GroupModel()
                {
                    Id = new Guid(g)
                }).ToArray());
            keycloakServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<Pims.Keycloak.Models.UserModel>()));

            var pimsServiceMock = helper.GetMock<IPimsService>();
            pimsServiceMock.Setup(m => m.User.GetTracking(It.IsAny<long>())).Returns(euser);
            pimsServiceMock.Setup(m => m.User.UpdateOnly(It.IsAny<Entity.User>())).Returns(euser);
            pimsServiceMock.Setup(m => m.Role.Find(removeRole.Id)).Returns<Entity.Role>(null);
            pimsServiceMock.Setup(m => m.Organization.GetChildren(It.IsAny<long>())).Returns(Array.Empty<Entity.Organization>());

            var user = EntityHelper.CreateUser(euser.Id, euser.KeycloakUserId.Value, euser.BusinessIdentifier, "new first name", "new last name");
            var addRole = user.Roles.First();
            addRole.Id = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            pimsServiceMock.Setup(m => m.Role.Find(addRole.Id)).Returns(addRole);

            // Act
            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateUserAsync(euser));
        }
        #endregion
    }
}
