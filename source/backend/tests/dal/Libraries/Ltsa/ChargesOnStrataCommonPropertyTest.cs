using System;
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
    public class ChargesOnStrataCommonPropertyTest
    {
        [Fact]
        public void TestConstructor_Null_InterAlia()
        {
            Assert.Throws<InvalidDataException>(() => new ChargesOnStrataCommonProperty(charge: new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>()), chargeNumber: "chargeNumber", chargeRemarks: "chargeRemarks", interAlia: null));
        }

        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new ChargesOnStrataCommonProperty(charge: new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>()), chargeRemarks: "chargeRemarks", interAlia: true, chargeNumber: null));
        }

        [Fact]
        public void TestConstructor_Null_ChargeRemarks()
        {
            Assert.Throws<InvalidDataException>(() => new ChargesOnStrataCommonProperty(charge: new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>()), chargeNumber: "chargeNumber", interAlia: true, chargeRemarks: null));
        }

        [Fact]
        public void TestConstructor_Null_Charge()
        {
            Assert.Throws<InvalidDataException>(() => new ChargesOnStrataCommonProperty(chargeNumber: "chargeNumber", chargeRemarks: "chargeRemarks", interAlia: true, charge: null));
        }

        [Fact]
        public void TestConstructor()
        {
            var charge = new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>());
            var chargeRelease = new ChargeRelease();
            DateTime enteredDate = DateTime.Now;
            ChargesOnStrataCommonProperty obj = new(ChargesOnStrataCommonProperty.StatusEnum.CANCELLED, ChargesOnStrataCommonProperty.CancellationTypeEnum.I, enteredDate, true, "chargeNumber", "chargeRemarks", charge, chargeRelease);
            obj.Status.Should().Be(ChargesOnStrataCommonProperty.StatusEnum.CANCELLED);
            obj.CancellationType.Should().Be(ChargesOnStrataCommonProperty.CancellationTypeEnum.I);
            obj.EnteredDate.Should().Be(enteredDate);
            obj.InterAlia.Should().Be(true);
            obj.ChargeNumber.Should().Be("chargeNumber");
            obj.ChargeRemarks.Should().Be("chargeRemarks");
            obj.Charge.Should().Be(charge);
            obj.ChargeRelease.Should().Be(chargeRelease);
        }
    }
}
