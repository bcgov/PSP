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
    public class TitleCorrectionTest
    {
        [Fact]
        public void TestConstructor_Null_TitleIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(reason: null));
        }

        [Fact]
        public void TestConstructor_Null_Tombstone()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(originatingCorrectionApplication: null));
        }

        [Fact]
        public void TestConstructor_Null_OwnershipGroups()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(enteredDate: null));
        }

        [Fact]
        public void TestConstructor_Null_TaxAuthorities()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(relatedChargeNumber: null));
        }
        [Fact]
        public void TestConstructor_Null_DescriptionsOfLand()
        {
            Assert.Throws<InvalidDataException>(() => new TitleCorrection(relatedLegalNotationNumber: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime enteredDate = new DateTime();
            TitleCorrection obj = new TitleCorrection("reason", "originatingCorrectionApplication", enteredDate, "relatedChargeNumber", "relatedLegalNotationNumber");
            obj.Reason.Should().Be("reason");
            obj.OriginatingCorrectionApplication.Should().Be("originatingCorrectionApplication");
            obj.EnteredDate.Should().Be(enteredDate);
            obj.RelatedChargeNumber.Should().BeEquivalentTo("relatedChargeNumber");
            obj.RelatedLegalNotationNumber.Should().BeEquivalentTo("relatedLegalNotationNumber");
        }
    }
}
