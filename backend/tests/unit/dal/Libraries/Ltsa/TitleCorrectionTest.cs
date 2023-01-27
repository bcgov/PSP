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
    public class TitleCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_Reason()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(enteredDate: DateTime.Now, relatedChargeNumber: "relatedChargeNumber", relatedLegalNotationNumber: "relatedLegalNotationNumber", originatingCorrectionApplication: "originatingCorrectionApplication", reason: null));
        }

        [Fact]
        public void TestConstructor_Null_OriginatingCorrectionApplication()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(reason: "reason", enteredDate: DateTime.Now, relatedChargeNumber: "relatedChargeNumber", relatedLegalNotationNumber: "relatedLegalNotationNumber", originatingCorrectionApplication: null));
        }

        [Fact]
        public void TestConstructor_Null_EnteredDate()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(reason: "reason", relatedChargeNumber: "relatedChargeNumber", relatedLegalNotationNumber: "relatedLegalNotationNumber", originatingCorrectionApplication: "originatingCorrectionApplication", enteredDate: null));
        }

        [Fact]
        public void TestConstructor_Null_RelatedChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(reason: "reason", enteredDate: DateTime.Now, relatedLegalNotationNumber: "relatedLegalNotationNumber", originatingCorrectionApplication: "originatingCorrectionApplication", relatedChargeNumber: null));
        }
        [Fact]
        public void TestConstructor_Null_RelatedLegalNotationNumber()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(reason: "reason", enteredDate: DateTime.Now, relatedChargeNumber: "relatedChargeNumber", originatingCorrectionApplication: "originatingCorrectionApplication", relatedLegalNotationNumber: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = new();
            TitleCorrection obj = new("reason", "originatingCorrectionApplication", enteredDate, "relatedChargeNumber", "relatedLegalNotationNumber");
            obj.Reason.Should().Be("reason");
            obj.OriginatingCorrectionApplication.Should().Be("originatingCorrectionApplication");
            obj.EnteredDate.Should().Be(enteredDate);
            obj.RelatedChargeNumber.Should().BeEquivalentTo("relatedChargeNumber");
            obj.RelatedLegalNotationNumber.Should().BeEquivalentTo("relatedLegalNotationNumber");
        }
    }
}
