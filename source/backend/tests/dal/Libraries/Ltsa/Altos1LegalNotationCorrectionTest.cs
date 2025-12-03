using System;
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
    public class Altos1LegalNotationCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1LegalNotationCorrection(correctionText: "correctionText", referenceDescription: "referenceDescription", enteredDate: DateTime.Now, correctionDate: DateTime.Now, number: null));
        }

        [Fact]
        public void TestConstructor_Null_ReferenceDescription()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1LegalNotationCorrection(correctionText: "correctionText", enteredDate: DateTime.Now, correctionDate: DateTime.Now, number: "number", referenceDescription: null));
        }

        [Fact]
        public void TestConstructor_Null_EnteredDate()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1LegalNotationCorrection(correctionText: "correctionText", referenceDescription: "referenceDescription", correctionDate: DateTime.Now, number: "number", enteredDate: null));
        }

        [Fact]
        public void TestConstructor_Null_CorrectionDate()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1LegalNotationCorrection(correctionText: "correctionText", referenceDescription: "referenceDescription", enteredDate: DateTime.Now, number: "number", correctionDate: null));
        }

        [Fact]
        public void TestConstructor_Null_CorrectionText()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1LegalNotationCorrection(referenceDescription: "referenceDescription", enteredDate: DateTime.Now, correctionDate: DateTime.Now, number: "number", correctionText: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            DateTime correctionDate = DateTime.Now;
            Altos1LegalNotationCorrection obj = new("number", "referenceDescription", enteredDate, correctionDate, "previousCorrectionNumber", "correctionText");
            obj.Number.Should().Be("number");
            obj.ReferenceDescription.Should().Be("referenceDescription");
            obj.EnteredDate.Should().Be(enteredDate);
            obj.CorrectionDate.Should().Be(correctionDate);
            obj.PreviousCorrectionNumber.Should().Be("previousCorrectionNumber");
            obj.CorrectionText.Should().Be("correctionText");
        }
    }
}
