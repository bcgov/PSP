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
    public class AccessRequestOrganizationOrganizationIdComparerTest
    {
        #region Variables
        public static IEnumerable<object[]> AccessRequestsOrganizations =>
            new List<object[]>
            {
                new object[] { new PimsAccessRequestOrganization(), new PimsAccessRequestOrganization(), true },
                new object[] { new PimsAccessRequestOrganization(1, 2), new PimsAccessRequestOrganization(1, 2), true },
                new object[] { new PimsAccessRequestOrganization(1, 2), new PimsAccessRequestOrganization(1, 3), false },
                new object[] { null, new PimsAccessRequestOrganization(1, 3), false },
                new object[] { new PimsAccessRequestOrganization(1, 2), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(AccessRequestsOrganizations))]
        public void AccessRequestOrganizationOrganizationIdComparer_Equal(PimsAccessRequestOrganization val1, PimsAccessRequestOrganization val2, bool expectedResult)
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
