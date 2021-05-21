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
    public class AgencyIdComparerTest
    {
        #region Variables
        public static IEnumerable<object[]> Agencies =>
            new List<object[]>
            {
                new object[] { new Agency(), new Agency(), true },
                new object[] { new Agency("code", "name"), new Agency("code", "name"), true },
                new object[] { new Agency() { Id = 1 }, new Agency() { Id = 1 }, true },
                new object[] { new Agency() { Id = 1 }, new Agency() { Id = 2 }, false },
                new object[] { null, new Agency(), false },
                new object[] { new Agency(), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Agencies))]
        public void AgencyIdComparer_Equal(Agency val1, Agency val2, bool expectedResult)
        {
            // Arrange
            var comparer = new AgencyIdComparer();

            // Act
            var result = comparer.Equals(val1, val2);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
