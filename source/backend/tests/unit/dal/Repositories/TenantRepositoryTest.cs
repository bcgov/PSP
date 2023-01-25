using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "tenants")]
    [ExcludeFromCodeCoverage]
    public class TenantRepositoryTest
    {
        #region Data
        public static IEnumerable<object[]> TenantsWithId =>
            new List<object[]>
            {
                new object[] { 0, 0 },
                new object[] { 1, 2 },
            };
        #endregion

        #region Tests
        #region GetTenant
        [Fact]
        public void GetTenant()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var init = helper.CreatePimsContext(user, true);
            var tenant = EntityHelper.CreateTenant(1, "TEST", "Test Tenant");
            init.AddAndSaveChanges(tenant);

            var service = helper.CreateRepository<TenantRepository>(user);

            // Act
            var result = service.TryGetTenantByCode(tenant.Code);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsTenant>(result);
            result.Should().Be(tenant);
        }

        [Fact]
        public void GetTenant_ReturnNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var init = helper.CreatePimsContext(user, true);
            var tenant = EntityHelper.CreateTenant(1, "TEST", "Test Tenant");
            init.AddAndSaveChanges(tenant);

            var service = helper.CreateRepository<TenantRepository>(user);

            // Act
            var result = service.TryGetTenantByCode("FAKE");

            // Assert
            result.Should().BeNull();
        }
        #endregion

        #region UpdateTenant
        [Fact]
        public void UpdateTenant_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var init = helper.CreatePimsContext(user, true);
            var tenant = EntityHelper.CreateTenant(1, "TEST", "Test Tenant");
            init.AddAndSaveChanges(tenant);

            var service = helper.CreateRepository<TenantRepository>(user);

            var updateTenant = EntityHelper.CreateTenant(1, "TEST", "New Name");
            updateTenant.Description = "New Description";
            updateTenant.Settings = "{\"id\":1}";

            // Act
            var result = service.UpdateTenant(updateTenant);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.PimsTenant>(result);
            result.Should().Be(tenant);
            result.Name.Should().Be(updateTenant.Name);
            result.Description.Should().Be(updateTenant.Description);
            result.Settings.Should().Be(updateTenant.Settings);
        }

        [Fact]
        public void UpdateTenant_ArgumentNullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateRepository<TenantRepository>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.UpdateTenant(null));
        }

        [Fact]
        public void UpdateTenant_NotAuthorizedException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateRepository<TenantRepository>(user);
            var tenant = EntityHelper.CreateTenant();

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.UpdateTenant(tenant));
        }

        [Fact]
        public void UpdateTenant_KeyNotFoundException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var service = helper.CreateRepository<TenantRepository>(user);
            var tenant = EntityHelper.CreateTenant();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.UpdateTenant(tenant));
        }
        #endregion
        #endregion
    }
}
