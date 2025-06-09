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
    public class LegalNotationCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_FullLegalDescription()
        {
            Assert.Throws<InvalidDataException>(() => new LegalNotationCorrection(originatingCorrectionApplication: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            LegalNotationCorrection obj = new("reason", "originatingCorrectionApplication", enteredDate);
            obj.EnteredDate.Should().Be(enteredDate);
            obj.Reason.Should().Be("reason");
            obj.OriginatingCorrectionApplication.Should().Be("originatingCorrectionApplication");
        }
    }
}
