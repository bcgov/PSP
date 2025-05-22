using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
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
    public class OrganizationIdComparerTest
    {
        #region Variables
        public static IEnumerable<object[]> Organizations =>
            new List<object[]>
            {
                new object[] { new PimsOrganization(), new PimsOrganization(), true },
                new object[] { new PimsOrganization("code", EntityHelper.CreateOrganizationType("TYPE"), EntityHelper.CreateOrganizationIdentifierType("ID"), EntityHelper.CreateAddress(1)), new PimsOrganization("code", EntityHelper.CreateOrganizationType("TYPE"), EntityHelper.CreateOrganizationIdentifierType("ID"), EntityHelper.CreateAddress(1)), true },
                new object[] { new PimsOrganization() { Internal_Id = 1 }, new PimsOrganization() { Internal_Id = 1 }, true },
                new object[] { new PimsOrganization() { Internal_Id = 1 }, new PimsOrganization() { Internal_Id = 2 }, false },
                new object[] { null, new PimsOrganization(), false },
                new object[] { new PimsOrganization(), null, false },
                new object[] { null, null, false },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Organizations))]
        public void OrganizationIdComparer_Equal(PimsOrganization val1, PimsOrganization val2, bool expectedResult)
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
