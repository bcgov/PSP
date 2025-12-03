using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Comparers;
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
        public static IEnumerable<object[]> Roles =>
            new List<object[]>
            {
                new object[] { new PimsRole(), new PimsRole(), true },
                new object[] { new PimsRole(Guid.Empty, "name"), new PimsRole(Guid.Empty, "name"), true },
                new object[] { new PimsRole() { Id = 1 }, new PimsRole() { Id = 1 }, true },
                new object[] { new PimsRole() { Id = 1 }, new PimsRole() { Id = 2 }, false },
                new object[] { null, new PimsRole(), false },
                new object[] { new PimsRole(), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Roles))]
        public void RoleIdComparer_Equal(PimsRole val1, PimsRole val2, bool expectedResult)
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
