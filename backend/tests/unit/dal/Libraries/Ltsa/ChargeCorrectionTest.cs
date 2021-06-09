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
    public class ChargeCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_Reason()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeCorrection(enteredDate: DateTime.Now, originatingCorrectionApplication: "originatingCorrectionApplication", reason: null));
        }

        [Fact]
        public void TestConstructor_Null_OriginatingCorrectionApplication()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeCorrection(enteredDate: DateTime.Now, reason: "reason", originatingCorrectionApplication: null));
        }

        [Fact]
        public void TestConstructor_Null_EnteredDate()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeCorrection(originatingCorrectionApplication: "originatingCorrectionApplication", reason: "reason", enteredDate: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = DateTime.Now;
            ChargeCorrection obj = new ChargeCorrection("reason", "originatingCorrectionApplication", enteredDate);
            obj.Reason.Should().Be("reason");
            obj.OriginatingCorrectionApplication.Should().Be("originatingCorrectionApplication");
            obj.EnteredDate.Should().Be(enteredDate);
        }
    }
}
