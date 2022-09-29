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
    public class ChargeOnTitleTest
    {
        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeOnTitle(charge: new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>()), chargeRemarks: "chargeRemarks", chargeNumber: null));
        }

        [Fact]
        public void TestConstructor_Null_TransactionType()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeOnTitle(charge: new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>()), chargeNumber: "chargeNumber", chargeRemarks: null));
        }

        [Fact]
        public void TestConstructor_Null_ChargeOwnershipGroups()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeOnTitle(chargeRemarks: "chargeRemarks", chargeNumber: "chargeNumber", charge: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            var chargeRelease = new ChargeRelease();
            var charge = new Charge(chargeNumber: "chargeNumber", transactionType: "transactionType", chargeOwnershipGroups: new List<ChargeOwnershipGroup>());

            ChargeOnTitle obj = new("chargeNumber", ChargeOnTitle.StatusEnum.CANCELLED, ChargeOnTitle.CancellationTypeEnum.I, enteredDate, ChargeOnTitle.InterAliaEnum.NO, "chargeRemarks", chargeRelease, charge);
            obj.ChargeNumber.Should().Be("chargeNumber");
            obj.Status.Should().Be(ChargeOnTitle.StatusEnum.CANCELLED);
            obj.CancellationType.Should().Be(ChargeOnTitle.CancellationTypeEnum.I);
            obj.EnteredDate.Should().Be(enteredDate);
            obj.InterAlia.Should().Be(ChargeOnTitle.InterAliaEnum.NO);
            obj.ChargeRemarks.Should().Be("chargeRemarks");
            obj.ChargeRelease.Should().Be(chargeRelease);
            obj.Charge.Should().Be(charge);
        }
    }
}
