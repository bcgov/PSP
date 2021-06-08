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
    public class Altos1TitleCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1TitleCorrection(null, ""));
        }

        [Fact]
        public void TestConstructor_Null_ReferenceDescription()
        {
            Assert.Throws<InvalidDataException>(() => new Altos1TitleCorrection("", null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            DateTime correctionDate = DateTime.Now;
            Altos1TitleCorrection obj = new Altos1TitleCorrection("number", "referenceDescription", enteredDate, correctionDate, "previousCorrectionNumber", "correctionText");
            obj.Number.Should().Be("number");
            obj.ReferenceDescription.Should().Be("referenceDescription");
            obj.EnteredDate.Should().Be(enteredDate);
            obj.CorrectionDate.Should().Be(correctionDate);
            obj.PreviousCorrectionNumber.Should().Be("previousCorrectionNumber");
            obj.CorrectionText.Should().Be("correctionText");
        }
    }
}
