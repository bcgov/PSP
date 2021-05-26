using FluentAssertions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
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
                new object[] { " ", " ", "" },
                new object[] { null, "", "" },
                new object[] { "", null, "" },
                new object[] { null, null, "" },
                new object[] { "123 St", "", "123 St" },
                new object[] { " 123 St", " ", "123 St" },
                new object[] { "123 St", null, "123 St" },
                new object[] { "123 St", "test", "123 St test" },
                new object[] { "123 St", " test", "123 St  test" },
            };
        #endregion

        #region Tests
        [Theory]
        [MemberData(nameof(Addresses))]
        public void FormatAddress(string address1, string address2, string expectedResult)
        {
            // Arrange
            var address = new Address(address1, address2, "Victoria", "BC", "postal");

            // Act
            var result = address.FormatAddress();

            // Assert
            result.Should().Be(expectedResult);
        }
        #endregion
    }
}
