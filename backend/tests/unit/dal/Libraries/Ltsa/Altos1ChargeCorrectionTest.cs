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
    public class Altos1ChargeCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1ChargeCorrection(correctionText: "correctionText", referenceDescription: "referenceDescription", number: null));
        }

        [Fact]
        public void TestConstructor_Null_ReferenceDescription()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1ChargeCorrection(correctionText: "correctionText", number: "number", referenceDescription: null));
        }

        [Fact]
        public void TestConstructor_Null_CorrectionText()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1ChargeCorrection(referenceDescription: "referenceDescription", number: "number", correctionText: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            DateTime correctionDate = DateTime.Now;
            Altos1ChargeCorrection obj = new Altos1ChargeCorrection("number", "referenceDescription", enteredDate, correctionDate, "previousCorrectionNumber", "correctionText");
            obj.Number.Should().Be("number");
            obj.ReferenceDescription.Should().Be("referenceDescription");
            obj.EnteredDate.Should().Be(enteredDate);
            obj.CorrectionDate.Should().Be(correctionDate);
            obj.PreviousCorrectionNumber.Should().Be("previousCorrectionNumber");
            obj.CorrectionText.Should().Be("correctionText");
        }
    }
}
