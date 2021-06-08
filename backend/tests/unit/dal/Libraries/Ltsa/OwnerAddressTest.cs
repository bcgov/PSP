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
    public class OwnerAddressTest
    {
        [Fact]
        public void TestConstructor()
        {
            OwnerAddress obj = new OwnerAddress("addressLine1", "addressLine2", "city", CanadianProvince.ABALBERTA, "provinceName", "country", "postalCode");
            obj.AddressLine1.Should().Be("addressLine1");
            obj.AddressLine2.Should().Be("addressLine2");
            obj.City.Should().Be("city");
            obj.Province.Should().Be(CanadianProvince.ABALBERTA);
            obj.ProvinceName.Should().Be("provinceName");
            obj.Country.Should().Be("country");
            obj.PostalCode.Should().Be("postalCode");
        }
    }
}
