using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Comparers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services.Admin
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "roles")]
    [ExcludeFromCodeCoverage]
    public class RoleServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void Get_Roles_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateService<RoleService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get(1, 1));
        }

        [Fact]
        public void Get_Roles_ValidQuery()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole("Role 1");
            init.AddAndSaveChanges(role);
            int expectedCount = 1;

            var service = helper.CreateService<RoleService>(user);

            var result = service.Get(1,1, "Role 1");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.Role>>(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }

        [Fact]
        public void Get_Role_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole("Role 1");
            var roleId = role.Id;
            init.AddAndSaveChanges(role);

            var service = helper.CreateService<RoleService>(user);

            // Act
            var result = service.Get(roleId);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.Role>(result);
            Assert.Equal("Role 1", result.Name);
        }

        [Fact]
        public void Get_Role_ByName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole("Role 1");
            init.AddAndSaveChanges(role);

            var service = helper.CreateService<RoleService>(user);

            // Act
            var result = service.GetByName("Role 1");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.Role>(result);
            Assert.Equal("Role 1", result.Name);
        }

        [Fact]
        public void Get_Role_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            var role = EntityHelper.CreateRole("Test");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(role);

            var service = helper.CreateService<RoleService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Get(Guid.NewGuid()));
        }

        [Fact]
        public void Get_Role_ByKeycloakId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole("Role 1");
            role.KeycloakGroupId = Guid.NewGuid();
            var keycloakId = role.KeycloakGroupId;
            init.AddAndSaveChanges(role);

            var service = helper.CreateService<RoleService>(user);

            // Act
            var result = service.GetByKeycloakId(keycloakId.Value);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.Role>(result);
            Assert.Equal("Role 1", result.Name);
        }
        #endregion
        #region Update
        [Fact]
        public void Update_Role()
        {
             // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles, Permissions.SystemAdmin);
            var id = Guid.NewGuid();
            var role = EntityHelper.CreateRole(id, "Role 1");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(role);

            var service = helper.CreateService<RoleService>(user);

            var updatedRole = EntityHelper.CreateRole(id, "Role 1");
            var newName = "Updated Role";
            updatedRole.Name = newName;

            // Act
            service.Update(updatedRole);

            // Assert
            role.Name.Should().Be(newName);
        }
        #endregion
        #region Remove
        [Fact]
        public void Remove_Role_SystemAdmin()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin, Permissions.AdminRoles);
            var role = EntityHelper.CreateRole("Delete Me");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(role);

            var service = helper.CreateService<RoleService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(role);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(role).State);
        }

        [Fact]
        public void RemoveAll_Role_SystemAdmin()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin, Permissions.AdminRoles);
            var role1 = EntityHelper.CreateRole("Delete Me");
            var role2 = EntityHelper.CreateRole("And Me");
            var role3 = EntityHelper.CreateRole("Not Me");
            Guid[] exclusions = new Guid[] {role3.Id};
            helper.CreatePimsContext(user, true).AddAndSaveChanges(role1, role2);

            var service = helper.CreateService<RoleService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.RemoveAll(exclusions);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(role1).State);
            Assert.Equal(EntityState.Detached, context.Entry(role2).State);
        }
        #endregion
        #endregion
    }
}
