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
    public class LegalNotationsOnStrataCommonPropertyTest
    {
        [Fact]
        public void TestConstructor_Null_LegalNotationNumber()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationsOnStrataCommonProperty(legalNotation: new(legalNotationText: "legalNotationText"), legalNotationNumber: null));
        }
        [Fact]
        public void TestConstructor_Null_LegalNotation()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationsOnStrataCommonProperty(legalNotationNumber: "legalNotationNumber", legalNotation: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime cancellationDate = DateTime.Now;
            LegalNotation legalNotation = new(legalNotationText: "legalNotationText");
            LegalNotationsOnStrataCommonProperty obj = new("legalNotationNumber", LegalNotationsOnStrataCommonProperty.StatusEnum.ACTIVE, cancellationDate, legalNotation);
            obj.LegalNotationNumber.Should().Be("legalNotationNumber");
            obj.Status.Should().Be(LegalNotationsOnStrataCommonProperty.StatusEnum.ACTIVE);
            obj.CancellationDate.Should().Be(cancellationDate);
            obj.LegalNotation.Should().Be(legalNotation);
        }
    }
}
