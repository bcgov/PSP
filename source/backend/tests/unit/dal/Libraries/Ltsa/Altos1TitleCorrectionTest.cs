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
    public class Altos1TitleCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1TitleCorrection(null, string.Empty));
        }

        [Fact]
        public void TestConstructor_Null_ReferenceDescription()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1TitleCorrection(string.Empty, null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            DateTime correctionDate = DateTime.Now;
            Altos1TitleCorrection obj = new("number", "referenceDescription", enteredDate, correctionDate, "previousCorrectionNumber", "correctionText");
            obj.Number.Should().Be("number");
            obj.ReferenceDescription.Should().Be("referenceDescription");
            obj.EnteredDate.Should().Be(enteredDate);
            obj.CorrectionDate.Should().Be(correctionDate);
            obj.PreviousCorrectionNumber.Should().Be("previousCorrectionNumber");
            obj.CorrectionText.Should().Be("correctionText");
        }
    }
}
