using Pims.Core.Test;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("area", "admin")]
    [Trait("group", "lease")]
    [ExcludeFromCodeCoverage]
    public class LeaseServiceTest
    {
        #region Data
        public static IEnumerable<object[]> LeaseFilterData =>
            new List<object[]>
            {
                new object[] { new LeaseFilter() { TenantName = "tenant" }, 1 },
                new object[] { new LeaseFilter() { TenantName = "fake" }, 0 },
                new object[] { new LeaseFilter() { LFileNo = "123" }, 1 },
                new object[] { new LeaseFilter() { LFileNo = "fake" }, 0 },
                new object[] { new LeaseFilter() { PidOrPin = "456" }, 1 },
                new object[] { new LeaseFilter() { PidOrPin = "789" }, 0 },
                new object[] { new LeaseFilter(), 1 },
            };
        #endregion

        #region Tests
        [Fact]
        public void Lease_Count()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var elease = EntityHelper.CreateLease(1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var result = service.Count();

            // Assert
            Assert.Equal(1, result);
        }

        #region Get
        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Paged(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", tenantLastName: "tenant");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var result = service.Get(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Entity.Lease[]>(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void Get_Leases_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var service = helper.CreateService<LeaseService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get(null));
        }

        [Theory]
        [MemberData(nameof(LeaseFilterData))]
        public void Get_Leases_Filter(LeaseFilter filter, int expectedCount)
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.PropertyView);
            var elease = EntityHelper.CreateLease(456, lFileNo: "123", tenantLastName: "tenant");

            helper.CreatePimsContext(user, true).AddAndSaveChanges(elease);

            var service = helper.CreateService<LeaseService>(user);

            // Act
            var result = service.GetPage(filter);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<Paged<Entity.Lease>>(result);
            Assert.Equal(expectedCount, result.Items.Count);
        }
        #endregion
        #endregion
    }
}
