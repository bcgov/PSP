using System.Collections.Generic;
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
    public class ParcelInfoTest
    {
        [Fact]
        public void TestConstructor_Null_ParcelIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(legalDescription: new LegalDescription(fullLegalDescription: "fullLegalDescription"), pendingApplicationCount: 2, registeredTitlesCount: 1, parcelIdentifier: null));
        }
        [Fact]
        public void TestConstructor_Null_RegisteredTitlesCount()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(legalDescription: new LegalDescription(fullLegalDescription: "fullLegalDescription"), pendingApplicationCount: 2, parcelIdentifier: "parcelIdentifier", registeredTitlesCount: null));
        }
        [Fact]
        public void TestConstructor_Null_PendingApplicationCount()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(legalDescription: new LegalDescription(fullLegalDescription: "fullLegalDescription"), registeredTitlesCount: 1, parcelIdentifier: "parcelIdentifier", pendingApplicationCount: null));
        }
        [Fact]
        public void TestConstructor_Null_LegalDescription()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(pendingApplicationCount: 2, registeredTitlesCount: 1, parcelIdentifier: "parcelIdentifier", legalDescription: null));
        }

        [Fact]
        public void TestConstructor()
        {
            ParcelTombstone parcelTombstone = new();
            LegalDescription legalDescription = new(fullLegalDescription: "fullLegalDescription");
            List<AssociatedPlan> associatedPlans = new();
            ParcelInfo obj = new("parcelIdentifier", ParcelInfo.StatusEnum.ACTIVE, 1, 2, "miscellaneousNotes", parcelTombstone, legalDescription, associatedPlans);
            obj.ParcelIdentifier.Should().Be("parcelIdentifier");
            obj.Status.Should().Be(ParcelInfo.StatusEnum.ACTIVE);
            obj.RegisteredTitlesCount.Should().Be(1);
            obj.PendingApplicationCount.Should().Be(2);
            obj.Tombstone.Should().Be(parcelTombstone);
            obj.LegalDescription.Should().Be(legalDescription);
            obj.AssociatedPlans.Should().BeEquivalentTo(associatedPlans);
        }
    }
}
