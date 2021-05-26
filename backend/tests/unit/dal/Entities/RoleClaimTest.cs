using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
                new object[] { new Role(Guid.NewGuid(), "role"), null },
                new object[] { null, new Claim(Guid.NewGuid(), "claim") },
            };
        #endregion

        #region Tests
        [Fact]
        public void RoleClaim_Default_Constructor()
        {
            // Arrange
            // Act
            var roleClaim = new RoleClaim();

            // Assert
            roleClaim.RoleId.Should().BeEmpty();
            roleClaim.Role.Should().BeNull();
            roleClaim.ClaimId.Should().BeEmpty();
            roleClaim.Claim.Should().BeNull();
        }

        [Fact]
        public void RoleClaim_Constructor_01()
        {
            // Arrange
            var rid = Guid.NewGuid();
            var cid = Guid.NewGuid();

            // Act
            var roleClaim = new RoleClaim(rid, cid);

            // Assert
            roleClaim.RoleId.Should().Be(rid);
            roleClaim.Role.Should().BeNull();
            roleClaim.ClaimId.Should().Be(cid);
            roleClaim.Claim.Should().BeNull();
        }

        [Fact]
        public void RoleClaim_Constructor_02()
        {
            // Arrange
            var role = EntityHelper.CreateRole(Guid.NewGuid(), "role");
            var claim = new Claim(Guid.NewGuid(), "claim");

            // Act
            var roleClaim = new RoleClaim(role, claim);

            // Assert
            roleClaim.RoleId.Should().Be(role.Id);
            roleClaim.Role.Should().Be(role);
            roleClaim.ClaimId.Should().Be(claim.Id);
            roleClaim.Claim.Should().Be(claim);
        }

        [Theory]
        [MemberData(nameof(Constructor_02))]
        public void RoleClaim_Constructor_02_ArgumentNullException(Role role, Claim claim)
        {
            // Arrange
            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => new RoleClaim(role, claim));
        }
        #endregion
    }
}
