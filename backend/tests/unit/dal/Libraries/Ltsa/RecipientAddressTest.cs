using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Pims.Ltsa.Models;
using System.IO;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;

namespace Pims.Dal.Test.Libraries.Ltsa
{
    [Trait("category", "unit")]
    [Trait("category", "ltsa")]
    [Trait("group", "ltsa")]
    [ExcludeFromCodeCoverage]
    public class RecipientAddressTest
    {
        [Fact]
        public void TestConstructor()
        {
            RecipientAddress obj = new RecipientAddress("addressLine1", "addressLine2", "city", "province", "postalCode", "country");
            obj.AddressLine1.Should().Be("addressLine1");
            obj.AddressLine2.Should().Be("addressLine2");
            obj.City.Should().Be("city");
            obj.Province.Should().Be("province");
            obj.PostalCode.Should().Be("postalCode");
            obj.Country.Should().Be("country");
        }
    }
}
