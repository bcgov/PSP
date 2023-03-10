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
        #endregion
        #endregion
    }
}
