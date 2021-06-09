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
            LegalNotationCorrection obj = new LegalNotationCorrection("reason", "originatingCorrectionApplication", enteredDate);
            obj.EnteredDate.Should().Be(enteredDate);
            obj.Reason.Should().Be("reason");
            obj.OriginatingCorrectionApplication.Should().Be("originatingCorrectionApplication");
        }
    }
}
