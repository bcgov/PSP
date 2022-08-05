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
    public class ChargeTest
    {
        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new Charge(chargeOwnershipGroups: new List<ChargeOwnershipGroup>(), transactionType: "transactionType", chargeNumber: null));
        }

        [Fact]
        public void TestConstructor_Null_TransactionType()
        {
            Assert.Throws<InvalidDataException>(() => new Charge(chargeOwnershipGroups: new List<ChargeOwnershipGroup>(), chargeNumber: "chargeNumber", transactionType: null));
        }

        [Fact]
        public void TestConstructor_Null_ChargeOwnershipGroups()
        {
            Assert.Throws<InvalidDataException>(() => new Charge(transactionType: "transactionType", chargeNumber: "chargeNumber", chargeOwnershipGroups: null));
        }

        [Fact]
        public void TestConstructor()
        {
            var chargeOwnershipGroups = new List<ChargeOwnershipGroup>();
            var certificatesOfCharge = new List<CertificateOfCharge>();
            Charge obj = new("chargeNumber", "transactionType", chargeOwnershipGroups: chargeOwnershipGroups, certificatesOfCharge: certificatesOfCharge);
            obj.ChargeNumber.Should().Be("chargeNumber");
            obj.TransactionType.Should().Be("transactionType");
            obj.ChargeOwnershipGroups.Should().BeEquivalentTo(chargeOwnershipGroups);
            obj.CertificatesOfCharge.Should().BeEquivalentTo(certificatesOfCharge);
        }
    }
}
