using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Comparers;
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
    public class RoleIdComparerTest
    {
        #region Variables
        static Guid roleId = Guid.NewGuid();
        public static IEnumerable<object[]> Roles =>
            new List<object[]>
            {
                new object[] { new Role(), new Role(), true },
                new object[] { new Role(Guid.Empty, "name"), new Role(Guid.Empty, "name"), true },
                new object[] { new Role() { Id = roleId }, new Role() { Id = roleId }, true },
                new object[] { new Role() { Id = Guid.NewGuid() }, new Role() { Id = Guid.NewGuid() }, false },
                new object[] { null, new Role(), false },
                new object[] { new Role(), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Roles))]
        public void RoleIdComparer_Equal(Role val1, Role val2, bool expectedResult)
        {
            // Arrange
            var comparer = new RoleIdComparer();

            // Act
            var result = comparer.Equals(val1, val2);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
