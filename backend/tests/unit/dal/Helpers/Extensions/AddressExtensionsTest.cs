using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Xunit;

namespace Pims.Dal.Test.Helpers.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("category", "extensions")]
    [Trait("group", "building")]
    [ExcludeFromCodeCoverage]
    public class AddressExtensionsTest
    {
        #region Variables
        public static IEnumerable<object[]> Addresses =>
            new List<object[]>
            {
                new object[] { EntityHelper.CreateAddress(1, "123 St ", string.Empty, null, null), "123 St" },
                new object[] { EntityHelper.CreateAddress(1, " 123 St", " ", null, null), "123 St" },
                new object[] { EntityHelper.CreateAddress(1, "123 St", (string)null, null, null), "123 St" },
                new object[] { EntityHelper.CreateAddress(1, "123 St", "test", null, null), "123 St test" },
                new object[] { EntityHelper.CreateAddress(1, "123 St", " test", null, null), "123 St  test" },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Addresses))]
        public void FormatAddress(PimsAddress address, string expectedResult)
        {
            // Arrange
            // Act
            var result = address.FormatAddress();

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
