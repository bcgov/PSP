using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "roles")]
    [ExcludeFromCodeCoverage]
    public class RoleRepositoryTest
    {
        #region Tests
        #region Get
        [Fact]
        public void Get_Roles_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.GetPage(1, 1));
        }

        [Fact]
        public void Get_Roles_ValidQuery()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole(99, "Role 1");
            init.AddAndSaveChanges(role);
            int expectedCount = 1;

            var service = helper.CreateRepository<RoleRepository>(user);

            var result = service.GetPage(1, 1, "Role 1");

            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.PimsRole>>(result);
            Assert.Equal(expectedCount, result.Items.Count());
        }

        [Fact]
        public void Get_Role_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole(99, "Role 1");
            var roleKey = role.RoleUid;
            init.AddAndSaveChanges(role);

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            var result = service.GetByKey(roleKey);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.PimsRole>(result);
            Assert.Equal("Role 1", result.Name);
        }

        [Fact]
        public void Get_Role_ByName()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole(50, "Role 1");
            init.AddAndSaveChanges(role);

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            var result = service.GetByName("Role 1");

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.PimsRole>(result);
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

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.GetByKey(Guid.NewGuid()));
        }

        [Fact]
        public void Get_Role_ByKeycloakId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.AdminRoles);
            using var init = helper.InitializeDatabase(user);
            var role = EntityHelper.CreateRole(50, "Role 1");
            role.KeycloakGroupId = Guid.NewGuid();
            var keycloakId = role.KeycloakGroupId;
            init.AddAndSaveChanges(role);

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            var result = service.GetByKeycloakId(keycloakId.Value);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.PimsRole>(result);
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
            var role = EntityHelper.CreateRole(1, id, "Role 1");
            helper.CreatePimsContext(user, true).AddAndSaveChanges(role);

            var service = helper.CreateRepository<RoleRepository>(user);

            var updatedRole = EntityHelper.CreateRole(1, id, "Role 1");
            var newName = "Updated Role";
            updatedRole.Name = newName;
            updatedRole.ConcurrencyControlNumber = role.ConcurrencyControlNumber;

            // Act
            service.Update(updatedRole);

            // Assert
            role.Name.Should().Be(newName);
        }

        [Fact]
        public void Update_Roles_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var role = EntityHelper.CreateRole(Guid.NewGuid(), "Role 1");

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Update(role));
        }

        [Fact]
        public void Update_Roles_NullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
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

            var service = helper.CreateRepository<RoleRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Delete(role);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(role).State);
        }

        [Fact]
        public void Remove_Roles_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var role = EntityHelper.CreateRole(Guid.NewGuid(), "Role 1");

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Delete(role));
        }

        [Fact]
        public void Remove_Roles_NullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Delete(null));
        }

        [Fact]
        public void RemoveAll_Role_SystemAdmin()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin, Permissions.AdminRoles);
            var role1 = EntityHelper.CreateRole(1, "Delete Me");
            var role2 = EntityHelper.CreateRole(2, "And Me");
            var role3 = EntityHelper.CreateRole(3, "Not Me");
            Guid[] exclusions = new Guid[] { role3.RoleUid };
            helper.CreatePimsContext(user, true).AddAndSaveChanges(role1, role2);

            var service = helper.CreateRepository<RoleRepository>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.RemoveAll(exclusions);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(role1).State);
            Assert.Equal(EntityState.Detached, context.Entry(role2).State);
        }

        [Fact]
        public void RemoveAll_Roles_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var role = EntityHelper.CreateRole(Guid.NewGuid(), "Role 1");
            Guid[] exclusions = new Guid[] { role.RoleUid };

            var service = helper.CreateRepository<RoleRepository>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.RemoveAll(exclusions));
        }
        #endregion
        #endregion
    }
}
