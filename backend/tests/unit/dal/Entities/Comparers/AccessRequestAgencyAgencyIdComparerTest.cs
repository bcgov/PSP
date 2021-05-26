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
    public class AccessRequestAgencyAgencyIdComparerTest
    {
        #region Variables
        public static IEnumerable<object[]> AccessRequestsAgencies =>
            new List<object[]>
            {
                new object[] { new AccessRequestAgency(), new AccessRequestAgency(), true },
                new object[] { new AccessRequestAgency(1, 2), new AccessRequestAgency(1, 2), true },
                new object[] { new AccessRequestAgency(1, 2), new AccessRequestAgency(1, 3), false },
                new object[] { null, new AccessRequestAgency(1, 3), false },
                new object[] { new AccessRequestAgency(1, 2), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(AccessRequestsAgencies))]
        public void AccessRequestAgencyAgencyIdComparer_Equal(AccessRequestAgency val1, AccessRequestAgency val2, bool expectedResult)
        {
            // Arrange
            var comparer = new AccessRequestAgencyAgencyIdComparer();

            // Act
            var result = comparer.Equals(val1, val2);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
