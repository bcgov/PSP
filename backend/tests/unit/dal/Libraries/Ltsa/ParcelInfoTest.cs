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
    public class ParcelInfoTest
    {
        [Fact]
        public void TestConstructor_Null_ParcelIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(parcelIdentifier: null));
        }
        [Fact]
        public void TestConstructor_Null_RegisteredTitlesCount()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(registeredTitlesCount: null));
        }
        [Fact]
        public void TestConstructor_Null_PendingApplicationCount()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(pendingApplicationCount: null));
        }
        [Fact]
        public void TestConstructor_Null_LegalDescription()
        {
            Assert.Throws<InvalidDataException>(() => new ParcelInfo(legalDescription: null));
        }

        [Fact]
        public void TestConstructor()
        {
            ParcelTombstone parcelTombstone = new ParcelTombstone();
            LegalDescription legalDescription = new LegalDescription(fullLegalDescription: "fullLegalDescription");
            List<AssociatedPlan> associatedPlans = new List<AssociatedPlan>();
            ParcelInfo obj = new ParcelInfo("parcelIdentifier", ParcelInfo.StatusEnum.ACTIVE, 1, 2, "miscellaneousNotes", parcelTombstone, legalDescription, associatedPlans);
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
