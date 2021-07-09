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
    public class AccessRequestRoleRoleIdComparerTest
    {
        #region Variables
        static Guid roleKey = Guid.NewGuid();
        public static IEnumerable<object[]> AccessRequestRoles =>
            new List<object[]>
            {
                new object[] { new AccessRequestRole(), new AccessRequestRole(), true },
                new object[] { new AccessRequestRole(1, 1), new AccessRequestRole(1, 1), true },
                new object[] { new AccessRequestRole(1, 2), new AccessRequestRole(1, 2), true },
                new object[] { new AccessRequestRole(1, 1), new AccessRequestRole(1, 3), false },
                new object[] { null, new AccessRequestRole(1, 0), false },
                new object[] { new AccessRequestRole(1, 0), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(AccessRequestRoles))]
        public void AccessRequestRoleRoleIdComparer_Equal(AccessRequestRole val1, AccessRequestRole val2, bool expectedResult)
        {
            // Arrange
            var comparer = new AccessRequestRoleRoleIdComparer();

            // Act
            var result = comparer.Equals(val1, val2);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
