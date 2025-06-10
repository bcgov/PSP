using System.Diagnostics.CodeAnalysis;
using System.IO;
using FluentAssertions;
using Pims.Ltsa.Models;
using Xunit;

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
            RecipientAddress address = new();
            StateTitleCertificateOrderParameters obj = new("titleNumber", "pendingApplicationNumber", "ltoClientNumber", "recipientName", LandTitleDistrictCode.KA, address);
            obj.TitleNumber.Should().Be("titleNumber");
            obj.PendingApplicationNumber.Should().Be("pendingApplicationNumber");
            obj.LtoClientNumber.Should().Be("ltoClientNumber");
            obj.RecipientName.Should().Be("recipientName");
            obj.LandTitleDistrictCode.Should().Be(LandTitleDistrictCode.KA);
            obj.RecipientAddress.Should().Be(address);
        }
    }
}
