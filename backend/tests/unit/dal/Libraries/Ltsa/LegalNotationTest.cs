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
    public class LegalNotationTest
    {
        [Fact]
        public void TestConstructor_Null_FullLegalDescription()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotation(legalNotationText: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime applicationReceivedDate = DateTime.Now;
            List<Altos1LegalNotationCorrection> altos1LegalNotationCorrections = new List<Altos1LegalNotationCorrection>();
            List<LegalNotationCorrection> legalNotationCorrections = new List<LegalNotationCorrection>();
            LegalNotation obj = new LegalNotation(applicationReceivedDate, "originalLegalNotationNumber", "planIdentifier", "legalNotationText", altos1LegalNotationCorrections, legalNotationCorrections);
            obj.ApplicationReceivedDate.Should().Be(applicationReceivedDate);
            obj.OriginalLegalNotationNumber.Should().Be("originalLegalNotationNumber");
            obj.PlanIdentifier.Should().Be("planIdentifier");
            obj.LegalNotationText.Should().Be("legalNotationText");
            obj.CorrectionsAltos1.Should().BeEquivalentTo(altos1LegalNotationCorrections);
            obj.Corrections.Should().BeEquivalentTo(legalNotationCorrections);
        }
    }
}
