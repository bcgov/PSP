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
    public class TitleOwnershipGroupTest
    {
        [Fact]
        public void TestConstructor_Null_OwnershipRemarks()
        {
            Assert.Throws<InvalidDataException>(() => new TitleOwnershipGroup(titleOwners: new List<TitleOwner>(), ownershipRemarks: null));
        }
        [Fact]
        public void TestConstructor_Null_TitleOwners()
        {
            Assert.Throws<InvalidDataException>(() => new TitleOwnershipGroup(ownershipRemarks: "ownershipRemarks", titleOwners: null));
        }

        [Fact]
        public void TestConstructor()
        {
            List<TitleOwner> titleOwners = new();
            TitleOwnershipGroup obj = new("jointTenancyIndication", "interestFractionNumerator", "interestFractionDenominator", "ownershipRemarks", titleOwners);
            obj.JointTenancyIndication.Should().Be("jointTenancyIndication");
            obj.InterestFractionNumerator.Should().Be("interestFractionNumerator");
            obj.InterestFractionDenominator.Should().Be("interestFractionDenominator");
            obj.OwnershipRemarks.Should().Be("ownershipRemarks");
            obj.TitleOwners.Should().BeEquivalentTo(titleOwners);
        }
    }
}
