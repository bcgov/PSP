using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Repositories;
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

        #endregion
    }
}
