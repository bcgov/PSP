using System.Collections.Generic;
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
    public class CertificateDeliveryTest
    {
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new CertificateDelivery(string.Empty, null));
        }

        [Fact]
        public void TestConstructor()
        {
            List<OwnerAddress> ownerAddresses = new();
            CertificateDelivery obj = new("certificateText", "intendedRecipientLastName", "intendedRecipientGivenName", ownerAddresses);
            obj.CertificateText.Should().Be("certificateText");
            obj.IntendedRecipientLastName.Should().Be("intendedRecipientLastName");
            obj.IntendedRecipientGivenName.Should().Be("intendedRecipientGivenName");
            obj.Address.Should().BeEquivalentTo(ownerAddresses);
        }
    }
}
