using FluentAssertions;
using Pims.Dal.Entities;
using System;
using System.Diagnostics.CodeAnalysis;
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
            var claim = new Claim();

            // Assert
            claim.Name.Should().BeNull();
            claim.Roles.Should().BeEmpty();
        }

        [Fact]
        public void Claim_Constructor_01()
        {
            // Arrange
            var uid = Guid.NewGuid();
            var name = "name";

            // Act
            var claim = new Claim(uid, name);

            // Assert
            claim.Id.Should().Be(uid);
            claim.Name.Should().Be(name);
            claim.Roles.Should().BeEmpty();
        }
        #endregion
    }
}
