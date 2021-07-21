using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Comparers;
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
    public class AccessRequestOrganizationOrganizationIdComparerTest
    {
        #region Variables
        public static IEnumerable<object[]> AccessRequestsOrganizations =>
            new List<object[]>
            {
                new object[] { new AccessRequestOrganization(), new AccessRequestOrganization(), true },
                new object[] { new AccessRequestOrganization(1, 2), new AccessRequestOrganization(1, 2), true },
                new object[] { new AccessRequestOrganization(1, 2), new AccessRequestOrganization(1, 3), false },
                new object[] { null, new AccessRequestOrganization(1, 3), false },
                new object[] { new AccessRequestOrganization(1, 2), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(AccessRequestsOrganizations))]
        public void AccessRequestOrganizationOrganizationIdComparer_Equal(AccessRequestOrganization val1, AccessRequestOrganization val2, bool expectedResult)
        {
            // Arrange
            var comparer = new AccessRequestOrganizationOrganizationIdComparer();

            // Act
            var result = comparer.Equals(val1, val2);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
