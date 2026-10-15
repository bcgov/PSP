using MapsterMapper;
using Pims.Api.Models.Concepts.Lease;
using Pims.Api.Models.Concepts.Role;
using Pims.Api.Models.Concepts.User;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Api.Test.Mapping
{
    public class MapsterBehaviorTests
    {
        private static IMapper CreateMapper()
        {
            _ = typeof(Pims.Api.Startup).Assembly.GetTypes();
            _ = typeof(UserRoleMap).Assembly.GetTypes();
            return new TestHelper().GetService<IMapper>();
        }

        [Fact]
        public void UserRole_MapsNestedUserAndRole()
        {
            var source = new PimsUserRole
            {
                UserRoleId = 11, UserId = 12, RoleId = 13,
                User = new PimsUser { UserId = 12 },
                Role = new PimsRole { RoleId = 13, Name = "Acquisition read-only" },
            };
            source.User.PimsUserRoles.Add(source);

            var result = CreateMapper().Map<UserRoleModel>(source);

            Assert.Equal(11, result.Id);
            Assert.Equal(12, result.User.Id);
            Assert.Equal(13, result.Role.Id);
            Assert.Equal("Acquisition read-only", result.Role.Name);
        }

        [Fact]
        public void RegionUser_MapsEntityToModel()
        {
            var source = new PimsRegionUser
            {
                RegionUserId = 21, UserId = 22, RegionCode = 1,
                User = new PimsUser { UserId = 22 },
                RegionCodeNavigation = new PimsRegion { RegionCode = 1, RegionName = "South Coast Region" },
            };
            source.User.PimsRegionUsers.Add(source);

            var result = CreateMapper().Map<RegionUserModel>(source);

            Assert.Equal(21, result.Id);
            Assert.Equal(22, result.User.Id);
            Assert.Equal((short)1, result.Region.Id);
        }

        [Fact]
        public void RegionUser_MapsModelToEntity()
        {
            var source = new RegionUserModel
            {
                Id = 21, UserId = 22, RegionCode = 1,
                User = new UserModel { Id = 22 },
            };
            source.User.UserRegions = new[] { source };

            var result = CreateMapper().Map<PimsRegionUser>(source);

            Assert.Equal(21, result.RegionUserId);
            Assert.Equal(22, result.User.UserId);
            Assert.Equal((short)1, result.RegionCode);
        }

        [Fact]
        public void RoleClaim_MapsEntityToModel()
        {
            var source = new PimsRoleClaim
            {
                RoleClaimId = 31, RoleId = 32, ClaimId = 33,
                Role = new PimsRole { RoleId = 32, Name = "Acquisition read-only" },
                Claim = new PimsClaim { ClaimId = 33, Name = "admin-users" },
            };
            source.Role.PimsRoleClaims.Add(source);

            var result = CreateMapper().Map<RoleClaimModel>(source);

            Assert.Equal(31, result.Id);
            Assert.Equal(32, result.Role.Id);
            Assert.Equal(33, result.Claim.Id);
            Assert.Equal("admin-users", result.Claim.Name);
        }

        [Fact]
        public void RoleClaim_MapsModelToEntity()
        {
            var source = new RoleClaimModel
            {
                Id = 31, RoleId = 32, ClaimId = 33,
                Role = new RoleModel { Id = 32, Name = "Acquisition read-only" },
                Claim = new Pims.Api.Models.Concepts.Claim.ClaimModel { Id = 33, Name = "admin-users" },
            };
            source.Role.RoleClaims = new[] { source };

            var result = CreateMapper().Map<PimsRoleClaim>(source);

            Assert.Equal(31, result.RoleClaimId);
            Assert.Equal(32, result.Role.RoleId);
            Assert.Equal(33, result.Claim.ClaimId);
        }

        [Fact]
        public void LeaseGraph_PreservesParentReference()
        {
            var source = new PimsLease { LeaseId = 41 };
            source.PimsPropertyLeases.Add(new PimsPropertyLease { Lease = source, LeaseId = 41 });

            var result = CreateMapper().Map<LeaseModel>(source);

            Assert.Equal(41, result.Id);
            Assert.Same(result, Assert.Single(result.FileProperties).File);
        }

        [Theory]
        [InlineData(null, "Existing description")]
        [InlineData("Replacement", "Replacement")]
        public void LeaseUpdate_RespectsNullPolicy(string description, string expected)
        {
            var source = new LeaseModel
            {
                Id = 41,
                Description = description,
                // The production mapping calls ToImmutableList on this collection.
                FileProperties = System.Array.Empty<PropertyLeaseModel>(),
            };
            var destination = new PimsLease { LeaseId = 41, LeaseDescription = "Existing description" };

            CreateMapper().Map(source, destination);

            Assert.Equal(expected, destination.LeaseDescription);
        }
    }
}
