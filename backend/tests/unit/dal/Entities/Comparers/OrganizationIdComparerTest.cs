using FluentAssertions;
using Pims.Core.Test;
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
    public class OrganizationIdComparerTest
    {
        #region Variables
        public static IEnumerable<object[]> Organizations =>
            new List<object[]>
            {
                new object[] { new Organization(), new Organization(), true },
                new object[] { new Organization("code", EntityHelper.CreateOrganizationType("TYPE"), EntityHelper.CreateOrganizationIdentifierType("ID"), EntityHelper.CreateAddress(1)), new Organization("code", EntityHelper.CreateOrganizationType("TYPE"), EntityHelper.CreateOrganizationIdentifierType("ID"), EntityHelper.CreateAddress(1)), true },
                new object[] { new Organization() { Id = 1 }, new Organization() { Id = 1 }, true },
                new object[] { new Organization() { Id = 1 }, new Organization() { Id = 2 }, false },
                new object[] { null, new Organization(), false },
                new object[] { new Organization(), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Organizations))]
        public void OrganizationIdComparer_Equal(Organization val1, Organization val2, bool expectedResult)
        {
            // Arrange
            var comparer = new OrganizationIdComparer();

            // Act
            var result = comparer.Equals(val1, val2);

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
