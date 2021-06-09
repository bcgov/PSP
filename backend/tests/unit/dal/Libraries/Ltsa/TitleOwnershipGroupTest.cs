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
            List<TitleOwner> titleOwners = new List<TitleOwner>();
            TitleOwnershipGroup obj = new TitleOwnershipGroup("jointTenancyIndication", "interestFractionNumerator", "interestFractionDenominator", "ownershipRemarks", titleOwners);
            obj.JointTenancyIndication.Should().Be("jointTenancyIndication");
            obj.InterestFractionNumerator.Should().Be("interestFractionNumerator");
            obj.InterestFractionDenominator.Should().Be("interestFractionDenominator");
            obj.OwnershipRemarks.Should().Be("ownershipRemarks");
            obj.TitleOwners.Should().BeEquivalentTo(titleOwners);
        }
    }
}
