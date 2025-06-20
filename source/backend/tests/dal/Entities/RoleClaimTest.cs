using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class RoleClaimTest
    {
        #region Variables
        public static IEnumerable<object[]> Constructor_02 =>
            new List<object[]>
            {
                new object[] { new PimsRole(Guid.NewGuid(), "role"), null },
                new object[] { null, new PimsClaim(Guid.NewGuid(), "claim") },
            };
        #endregion

        #region Tests
        [Fact]
        public void RoleClaim_Default_Constructor()
        {
            // Arrange
            // Act
            var roleClaim = new PimsRoleClaim();

            // Assert
            roleClaim.RoleId.Should().Be(0);
            roleClaim.Role.Should().BeNull();
            roleClaim.ClaimId.Should().Be(0);
            roleClaim.Claim.Should().BeNull();
        }

        [Fact]
        public void RoleClaim_Constructor_01()
        {
            // Arrange
            // Act
            var roleClaim = new PimsRoleClaim(1, 2);

            // Assert
            roleClaim.RoleId.Should().Be(1);
            roleClaim.Role.Should().BeNull();
            roleClaim.ClaimId.Should().Be(2);
            roleClaim.Claim.Should().BeNull();
        }

        [Fact]
        public void RoleClaim_Constructor_02()
        {
            // Arrange
            var role = EntityHelper.CreateRole(Guid.NewGuid(), "role");
            var claim = new PimsClaim(Guid.NewGuid(), "claim");

            // Act
            var roleClaim = new PimsRoleClaim(role, claim);

            // Assert
            roleClaim.RoleId.Should().Be(role.Id);
            roleClaim.Role.Should().Be(role);
            roleClaim.ClaimId.Should().Be(claim.ClaimId);
            roleClaim.Claim.Should().Be(claim);
        }

        [Theory]
        [MemberData(nameof(Constructor_02))]
        public void RoleClaim_Constructor_02_ArgumentNullException(PimsRole role, PimsClaim claim)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new PimsRoleClaim(role, claim));
        }
        #endregion
    }
}
