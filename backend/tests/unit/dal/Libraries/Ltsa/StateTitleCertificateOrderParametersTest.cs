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
    public class StateTitleCertificateOrderTest
    {
        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new StateTitleCertificateOrderParameters(titleNumber: null));
        }
        [Fact]
        public void TestConstructor()
        {
            RecipientAddress address = new RecipientAddress();
            StateTitleCertificateOrderParameters obj = new StateTitleCertificateOrderParameters("titleNumber", "pendingApplicationNumber", "ltoClientNumber", "recipientName", LandTitleDistrictCode.KA, address);
            obj.TitleNumber.Should().Be("titleNumber");
            obj.PendingApplicationNumber.Should().Be("pendingApplicationNumber");
            obj.LtoClientNumber.Should().Be("ltoClientNumber");
            obj.RecipientName.Should().Be("recipientName");
            obj.LandTitleDistrictCode.Should().Be(LandTitleDistrictCode.KA);
            obj.RecipientAddress.Should().Be(address);
        }
    }
}
