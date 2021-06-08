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
    public class LegalNotationsOnStrataCommonPropertyTest
    {
        [Fact]
        public void TestConstructor_Null_LegalNotationNumber()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationsOnStrataCommonProperty(legalNotationNumber: null));
        }
        [Fact]
        public void TestConstructor_Null_LegalNotation()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationsOnStrataCommonProperty(legalNotation: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime cancellationDate = DateTime.Now;
            LegalNotation legalNotation = new LegalNotation(legalNotationText: "legalNotationText");
            LegalNotationsOnStrataCommonProperty obj = new LegalNotationsOnStrataCommonProperty("legalNotationNumber", LegalNotationsOnStrataCommonProperty.StatusEnum.ACTIVE, cancellationDate, legalNotation);
            obj.LegalNotationNumber.Should().Be("legalNotationNumber");
            obj.Status.Should().Be(LegalNotationsOnStrataCommonProperty.StatusEnum.ACTIVE);
            obj.CancellationDate.Should().Be(cancellationDate);
            obj.LegalNotation.Should().Be(legalNotation);
        }
    }
}
