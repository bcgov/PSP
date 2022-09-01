using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Dal.Entities;
using Xunit;

namespace Pims.Dal.Test.Entities
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "entities")]
    [Trait("group", "entity")]
    [ExcludeFromCodeCoverage]
    public class ClaimTest
    {
        #region Tests
        [Fact]
        public void Claim_Default_Constructor()
        {
            // Arrange
            // Act
            var claim = new PimsClaim();

            // Assert
            claim.Name.Should().BeNull();
            claim.GetRoles().Should().BeEmpty();
        }

        [Fact]
        public void Claim_Constructor_01()
        {
            // Arrange
            var uid = Guid.NewGuid();
            var name = "name";

            // Act
            var claim = new PimsClaim(uid, name);

            // Assert
            claim.ClaimId.Should().Be(0);
            claim.ClaimUid.Should().Be(uid);
            claim.Name.Should().Be(name);
            claim.GetRoles().Should().BeEmpty();
        }
        #endregion
    }
}
