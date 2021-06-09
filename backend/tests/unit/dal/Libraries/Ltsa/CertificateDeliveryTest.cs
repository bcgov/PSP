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
    public class CertificateDeliveryTest
    {
        DateTime issuedDate = DateTime.Now;
        DateTime surrenderDate = DateTime.Now;
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new CertificateDelivery("", null));
        }

        [Fact]
        public void TestConstructor()
        {
            List<OwnerAddress> ownerAddresses = new List<OwnerAddress>();
            CertificateDelivery obj = new CertificateDelivery("certificateText", "intendedRecipientLastName", "intendedRecipientGivenName", ownerAddresses);
            obj.CertificateText.Should().Be("certificateText");
            obj.IntendedRecipientLastName.Should().Be("intendedRecipientLastName");
            obj.IntendedRecipientGivenName.Should().Be("intendedRecipientGivenName");
            obj.Address.Should().BeEquivalentTo(ownerAddresses);
        }
    }
}
