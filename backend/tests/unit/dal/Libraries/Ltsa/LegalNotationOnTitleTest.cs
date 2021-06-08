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
    public class LegalNotationOnTitleTest
    {
        [Fact]
        public void TestConstructor_Null_LegalNotationNumber()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationOnTitle(legalNotationNumber: null));
        }
        [Fact]
        public void TestConstructor_Null_Status()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationOnTitle(status: null));
        }
        [Fact]
        public void TestConstructor_Null_LegalNotation()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationOnTitle(legalNotation: null));
        }

        [Fact]
        public void TestConstructor()
        {
            LegalNotation legalNotation = new LegalNotation(legalNotationText: "legalNotationText");
            LegalNotationOnTitle obj = new LegalNotationOnTitle("legalNotationNumber", "status", "cancellationDate", legalNotation);
            obj.LegalNotationNumber.Should().Be("legalNotationNumber");
            obj.Status.Should().Be("status");
            obj.CancellationDate.Should().Be("cancellationDate");
            obj.LegalNotation.Should().Be(legalNotation);
        }
    }
}
