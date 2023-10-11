using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Spreadsheet;
using FluentAssertions;
using Moq;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Keycloak;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Pims.Keycloak.Models;
using Xunit;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
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
                Username = euser.BusinessIdentifierValue,
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { euser.BusinessIdentifierValue } } },
            };
            var groups = new string[] { removeRole.Name.ToString() };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = removeRole.Name,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Pims.Dal.Entities.PimsUser>())).Returns(euser);
            userRepository.Setup(m => m.RemoveRole(It.IsAny<Pims.Dal.Entities.PimsUser>(), removeRole.RoleId)).Returns((Pims.Dal.Entities.PimsUser eUser, long roleId) =>
            {
                var userRole = eUser.PimsUserRoles.FirstOrDefault(r => r.RoleId == roleId);
                eUser.PimsUserRoles.Remove(userRole);
                return eUser;
            });
            userRepository.Setup(m => m.UpdateAllRolesForUser(euser.Internal_Id, It.IsAny<ICollection<PimsUserRole>>())).Returns((long userId, ICollection<PimsUserRole> roles) =>
            {
                euser.PimsUserRoles = roles;
                return roles;
            });
            roleRepository.Setup(m => m.Find(removeRole.RoleId)).Returns(removeRole);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addUserRole = user.PimsUserRoles.First();
            addUserRole.RoleId = 2;
            addUserRole.Role.RoleId = 2;
            addUserRole.Role.KeycloakGroupId = Guid.NewGuid();
            addUserRole.Role.Name = "Updated";
            roleRepository.Setup(m => m.Find(addUserRole.Role.RoleId)).Returns(addUserRole.Role);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.Internal_Id.Should().Be(user.Internal_Id);
            result.BusinessIdentifierValue.Should().Be(user.BusinessIdentifierValue);
            result.Person.FirstName.Should().Be(user.Person.FirstName);
            result.Person.Surname.Should().Be(user.Person.Surname);
            result.GetOrganizations().Count.Should().Be(euser.GetOrganizations().Count);
            result.PimsUserRoles.Count.Should().Be(1);
            result.PimsUserRoles.First().Role.KeycloakGroupId.Should().Be(user.GetRoles().First().KeycloakGroupId);

            keycloakServiceMock.Verify(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value), Times.Once);
            keycloakServiceMock.Verify(m => m.ModifyUserRoleMappings(It.Is<IEnumerable<UserRoleOperation>>(a => a.Any(b => b.RoleName == "Updated" && b.Operation == "add"))), Times.Once);
            userRepository.Verify(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>()), Times.Once);
            userRepository.Verify(m => m.RemoveRole(It.IsAny<Entity.PimsUser>(), removeRole.RoleId), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_Success_KeycloakRoleNotInPims()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var removeRole = euser.PimsUserRoles.First().Role;
            removeRole.RoleId = 1;
            removeRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { Guid.NewGuid().ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { euser.BusinessIdentifierValue } } },
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = g,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Pims.Dal.Entities.PimsUser>())).Returns(euser);
            userRepository.Setup(m => m.UpdateAllRolesForUser(euser.Internal_Id, It.IsAny<ICollection<PimsUserRole>>())).Returns((long userId, ICollection<PimsUserRole> roles) =>
            {
                euser.PimsUserRoles = roles;
                return roles;
            });
            roleRepository.Setup(m => m.Find(removeRole.RoleId)).Returns(removeRole);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addUserRole = user.PimsUserRoles.First();
            addUserRole.RoleId = 2;
            addUserRole.Role.RoleId = 2;
            addUserRole.Role.KeycloakGroupId = Guid.NewGuid();
            roleRepository.Setup(m => m.Find(addUserRole.Role.RoleId)).Returns(addUserRole.Role);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.Internal_Id.Should().Be(user.Internal_Id);
            result.BusinessIdentifierValue.Should().Be(user.BusinessIdentifierValue);
            result.Person.FirstName.Should().Be(user.Person.FirstName);
            result.Person.Surname.Should().Be(user.Person.Surname);
            result.GetOrganizations().Count.Should().Be(euser.GetOrganizations().Count);
            result.PimsUserRoles.Count.Should().Be(1);
            result.PimsUserRoles.First().Role.KeycloakGroupId.Should().Be(user.GetRoles().First().KeycloakGroupId);

            keycloakServiceMock.Verify(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value), Times.Once);
            keycloakServiceMock.Verify(m => m.ModifyUserRoleMappings(It.IsAny<IEnumerable<UserRoleOperation>>()), Times.Once);
            userRepository.Verify(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>()), Times.Once);
            userRepository.Verify(m => m.RemoveRole(It.IsAny<Entity.PimsUser>(), removeRole.RoleId), Times.Once);

            // this assertion differs from the general success assertion.
            keycloakServiceMock.Verify(m => m.ModifyUserRoleMappings(new UserRoleOperation[] { new UserRoleOperation() { Username = euser.GetIdirUsername(), RoleName = groups.First(), Operation = "add" } }), Times.Never);
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
            var groups = new[] { euser.GuidIdentifierValue.Value.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = "keycloak username",
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { "serviceaccount" } } },
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = g,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(euser);
            roleRepository.Setup(m => m.Find(removeRole.RoleId)).Returns(removeRole);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Pims.Dal.Entities.PimsUser>())).Returns(euser);

            var user = EntityHelper.CreateUser(1, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.GetRoles().First();
            addRole.RoleId = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            roleRepository.Setup(m => m.Find(addRole.RoleId)).Returns(addRole);

            // Act
            var result = await service.UpdateUserAsync(user);

            // Assert
            result.BusinessIdentifierValue.Should().Be("serviceaccount");
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
            var groups = new string[] { removeRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = g,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(euser);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.GetRoles().First();
            addRole.KeycloakGroupId = Guid.NewGuid();
            roleRepository.Setup(m => m.Find(addRole.RoleId)).Returns<Entity.PimsRole>(null);

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
            var groups = new string[] { removeRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = g,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(euser);
            roleRepository.Setup(m => m.Find(removeRole.RoleId)).Returns<Entity.PimsRole>(null);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.GetRoles().First();
            addRole.RoleId = 2;
            addRole.KeycloakGroupId = Guid.NewGuid();
            roleRepository.Setup(m => m.Find(addRole.RoleId)).Returns(addRole);

            // Act
            // Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(async () => await service.UpdateUserAsync(euser));
        }
        #endregion

        #region AppendToUserAsync

        [Fact]
        public async Task AppendToUserAsync_Success_AddRole()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var existingRole = euser.PimsUserRoles.First().Role;
            existingRole.RoleId = 1;
            existingRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { existingRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { "serviceaccount" } } },
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = g,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Pims.Dal.Entities.PimsUser>())).Returns(euser);
            userRepository.Setup(m => m.RemoveRole(It.IsAny<Pims.Dal.Entities.PimsUser>(), existingRole.RoleId)).Returns((Pims.Dal.Entities.PimsUser eUser, long roleId) =>
            {
                var userRole = eUser.PimsUserRoles.FirstOrDefault(r => r.RoleId == roleId);
                eUser.PimsUserRoles.Remove(userRole);
                return eUser;
            });
            userRepository.Setup(m => m.UpdateAllRolesForUser(euser.Internal_Id, It.IsAny<ICollection<PimsUserRole>>())).Returns((long userId, ICollection<PimsUserRole> roles) =>
            {
                euser.PimsUserRoles = roles;
                return roles;
            });
            roleRepository.Setup(m => m.Find(existingRole.RoleId)).Returns(existingRole);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addUserRole = user.PimsUserRoles.First();
            addUserRole.RoleId = 2;
            addUserRole.Role.RoleId = 2;
            addUserRole.Role.KeycloakGroupId = Guid.NewGuid();
            roleRepository.Setup(m => m.Find(addUserRole.Role.RoleId)).Returns(addUserRole.Role);

            // Act
            await service.AppendToUserAsync(user);

            // Assert
            keycloakServiceMock.Verify(m => m.ModifyUserRoleMappings(It.Is<IEnumerable<UserRoleOperation>>(a => a.Any(b => b.RoleName == addUserRole.Role.Name && b.Operation == "add"))), Times.Once);
            userRepository.Verify(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>()), Times.Once);
            userRepository.Verify(m => m.RemoveRole(It.IsAny<Entity.PimsUser>(), existingRole.RoleId), Times.Once);
        }

        [Fact]
        public async Task AppendToUserAsync_Success_AddContactMethod()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var existingRole = euser.PimsUserRoles.First().Role;
            existingRole.RoleId = 1;
            existingRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { existingRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { "serviceaccount" } } },
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);
            keycloakServiceMock.Setup(m => m.GetUserGroupsAsync(euser.GuidIdentifierValue.Value))
                .ReturnsAsync(groups.Select(g => new Pims.Keycloak.Models.RoleModel()
                {
                    Name = g,
                }).ToArray());

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            var values = new List<Entity.PimsUser>();
            userRepository.Setup(m => m.UpdateOnly(Capture.In(values))).Returns(euser);
            roleRepository.Setup(m => m.Find(existingRole.RoleId)).Returns(existingRole);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            user.Person.PimsContactMethods.Clear();
            var updatedContactMethod = new Entity.PimsContactMethod() { ContactMethodValue = "update contact method", ContactMethodTypeCode = ContactMethodTypes.WorkEmail };
            user.Person.PimsContactMethods.Add(updatedContactMethod);

            // Act
            await service.AppendToUserAsync(user);

            // Assert
            userRepository.Verify(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>()), Times.Once);
            values.First().Person.PimsContactMethods.Should().Contain(cm => cm.ContactMethodValue == updatedContactMethod.ContactMethodValue);
        }

        [Fact]
        public async Task AppendToUserAsync_AddRole_KeycloakGroupIdNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var existingRole = euser.PimsUserRoles.First().Role;
            existingRole.RoleId = 1;
            existingRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { existingRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(euser);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.PimsUserRoles.First();
            addRole.RoleId = 2;
            addRole.Role.KeycloakGroupId = null;
            roleRepository.Setup(m => m.Find(addRole.RoleId)).Returns(addRole.Role);

            // Act
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AppendToUserAsync(user));
        }

        [Fact]
        public async Task AppendToUserAsync_AddRole_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var euser = EntityHelper.CreateUser("test");
            var existingRole = euser.PimsUserRoles.First().Role;
            existingRole.RoleId = 1;
            existingRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { existingRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = euser.BusinessIdentifierValue,
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);

            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(euser);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(euser);

            var user = EntityHelper.CreateUser(euser.Internal_Id, euser.GuidIdentifierValue.Value, euser.BusinessIdentifierValue, "new first name", "new last name");
            var addRole = user.PimsUserRoles.First();
            addRole.RoleId = 2;
            addRole.Role.KeycloakGroupId = Guid.NewGuid();
            roleRepository.Setup(m => m.Find(addRole.RoleId)).Returns((Entity.PimsRole)null);

            // Act
            await Assert.ThrowsAsync<KeyNotFoundException>(() => service.AppendToUserAsync(user));
        }

        [Fact]
        public async Task UpdateAccessRequestAsync_Recieved()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var service = helper.Create<PimsKeycloakService>(user);

            var eAccessRequest = EntityHelper.CreateAccessRequest(1);
            var eRole = EntityHelper.CreateRole("test-role");
            eRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { eRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = eAccessRequest.User.BusinessIdentifierValue,
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);

            var updatedAccessRequest = new Entity.PimsAccessRequest()
            {
                AccessRequestId = eAccessRequest.AccessRequestId,
                AccessRequestStatusTypeCode = AccessRequestStatusTypes.RECEIVED,
                User = eAccessRequest.User,
                Role = eRole,
                RoleId = eRole.Id,
            };

            var accessRequestRepository = helper.GetMock<IAccessRequestRepository>();
            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            var values = new List<Entity.PimsAccessRequest>();
            accessRequestRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(eAccessRequest);
            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(updatedAccessRequest.User);
            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(updatedAccessRequest.User);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(updatedAccessRequest.User);
            roleRepository.Setup(m => m.Find(It.IsAny<long>())).Returns(updatedAccessRequest.Role);
            accessRequestRepository.Setup(m => m.Update(Capture.In(values))).Returns(updatedAccessRequest);

            // Act
            var response = await service.UpdateAccessRequestAsync(updatedAccessRequest);

            var updated = values.First();
            updated.Role.Should().Be(eRole);
            updated.AccessRequestStatusTypeCode.Should().Be(AccessRequestStatusTypes.RECEIVED);
            updated.User.IsDisabled.Should().BeNull();
        }

        [Fact]
        public async Task UpdateAccessRequestAsync_Approved()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var service = helper.Create<PimsKeycloakService>(user);

            var eAccessRequest = EntityHelper.CreateAccessRequest(1);
            var eRole = EntityHelper.CreateRole("test-role");
            eRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { eRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = eAccessRequest.User.BusinessIdentifierValue,
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { "serviceaccount" } } },
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);

            var updatedAccessRequest = new Entity.PimsAccessRequest()
            {
                AccessRequestId = eAccessRequest.AccessRequestId,
                AccessRequestStatusTypeCode = AccessRequestStatusTypes.APPROVED,
                User = eAccessRequest.User,
                Role = eRole,
                RoleId = eRole.Id,
                RegionCode = eAccessRequest.RegionCode,
            };

            var accessRequestRepository = helper.GetMock<IAccessRequestRepository>();
            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            var values = new List<Entity.PimsAccessRequest>();
            accessRequestRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(eAccessRequest);
            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(updatedAccessRequest.User);
            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(updatedAccessRequest.User);
            userRepository.Setup(m => m.UpdateOnly(It.IsAny<Entity.PimsUser>())).Returns(updatedAccessRequest.User);
            roleRepository.Setup(m => m.Find(It.IsAny<long>())).Returns(updatedAccessRequest.Role);
            accessRequestRepository.Setup(m => m.Update(Capture.In(values))).Returns(updatedAccessRequest);

            // Act
            var response = await service.UpdateAccessRequestAsync(updatedAccessRequest);

            var updated = values.First();
            updated.Role.Should().Be(eRole);
            updated.RegionCode.Should().Be(eAccessRequest.RegionCode);
            updated.AccessRequestStatusTypeCode.Should().Be(AccessRequestStatusTypes.APPROVED);
            updated.User.IsDisabled.Should().BeFalse();
        }

        [Fact]
        public async Task UpdateAccessRequestAsync_Approved_RegionUpdate()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminUsers);
            var service = helper.Create<PimsKeycloakService>(user);

            var eAccessRequest = EntityHelper.CreateAccessRequest(1);
            var eRole = EntityHelper.CreateRole("test-role");
            eRole.KeycloakGroupId = Guid.NewGuid();

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var groups = new string[] { eRole.KeycloakGroupId.ToString() };
            var kuser = new Pims.Keycloak.Models.UserModel()
            {
                Username = eAccessRequest.User.BusinessIdentifierValue,
                Attributes = new Dictionary<string, string[]>() { { "idir_username", new string[1] { "serviceaccount" } } },
            };
            keycloakServiceMock.Setup(m => m.GetUserAsync(It.IsAny<Guid>())).ReturnsAsync(kuser);

            var updatedAccessRequest = new Entity.PimsAccessRequest()
            {
                AccessRequestId = eAccessRequest.AccessRequestId,
                AccessRequestStatusTypeCode = AccessRequestStatusTypes.APPROVED,
                User = eAccessRequest.User,
                Role = eRole,
                RoleId = eRole.Id,
                RegionCode = 10,
            };

            var accessRequestRepository = helper.GetMock<IAccessRequestRepository>();
            var userRepository = helper.GetMock<IUserRepository>();
            var roleRepository = helper.GetMock<IRoleRepository>();

            var values = new List<Entity.PimsUser>();
            accessRequestRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(eAccessRequest);
            userRepository.Setup(m => m.GetTrackingById(It.IsAny<long>())).Returns(updatedAccessRequest.User);
            userRepository.Setup(m => m.GetById(It.IsAny<long>())).Returns(updatedAccessRequest.User);
            userRepository.Setup(m => m.UpdateOnly(Capture.In(values))).Returns(updatedAccessRequest.User);
            roleRepository.Setup(m => m.Find(It.IsAny<long>())).Returns(updatedAccessRequest.Role);
            accessRequestRepository.Setup(m => m.Update(It.IsAny<PimsAccessRequest>())).Returns(updatedAccessRequest);

            // Act
            var response = await service.UpdateAccessRequestAsync(updatedAccessRequest);

            values.Should().HaveCount(1);
            values.FirstOrDefault().PimsRegionUsers.Should().HaveCount(1);
            values.FirstOrDefault().PimsRegionUsers.FirstOrDefault(u => u.RegionCode == updatedAccessRequest.RegionCode);
        }

        [Fact]
        public async Task UpdateAccessRequestAsync_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            // Act
            await Assert.ThrowsAsync<ArgumentNullException>(() => service.UpdateAccessRequestAsync(null));
        }

        [Fact]
        public async Task UpdateAccessRequestAsync_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var service = helper.Create<PimsKeycloakService>();

            var eAccessRequest = EntityHelper.CreateAccessRequest(1);

            var keycloakServiceMock = helper.GetMock<Pims.Keycloak.IKeycloakService>();
            var updatedAccessRequest = new Entity.PimsAccessRequest()
            {
                AccessRequestId = eAccessRequest.AccessRequestId,
                AccessRequestStatusTypeCode = AccessRequestStatusTypes.APPROVED,
                User = eAccessRequest.User,
            };

            // Act
            await Assert.ThrowsAsync<NotAuthorizedException>(() => service.UpdateAccessRequestAsync(updatedAccessRequest));
        }

        #endregion
    }
}
