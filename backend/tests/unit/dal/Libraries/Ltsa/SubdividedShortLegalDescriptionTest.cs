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
    public class SubdividedShortLegalDescriptionTest
    {
        [Fact]
        public void TestConstructor_Null_PlanNumber1()
        {
            Assert.Throws<InvalidDataException>(() => new SubdividedShortLegalDescription(concatShortLegal: "concatShortLegal", planNumber1: null));
        }

        [Fact]
        public void TestConstructor_Null_ConcatShortLegal()
        {
            Assert.Throws<InvalidDataException>(() => new SubdividedShortLegalDescription(planNumber1: "planNumber1", concatShortLegal: null));
        }

        [Fact]
        public void TestConstructor()
        {
            SubdividedShortLegalDescription obj = new("planNumber1", "townshipOrTownSite2", "range3", "block4", "subdivision5", "lotOrDistrictLotOrSubLot6", "subdivision7",
                "lotOrParcel8", "section9", "quadrant10", "blockOrLot11", "lotOrParcel12", "parcelOrBlock13", "concatShortLegal", "marginalNotes");
            obj.PlanNumber1.Should().Be("planNumber1");
            obj.TownshipOrTownSite2.Should().Be("townshipOrTownSite2");
            obj.Block4.Should().Be("block4");
            obj.Subdivision5.Should().Be("subdivision5");
            obj.LotOrDistrictLotOrSubLot6.Should().Be("lotOrDistrictLotOrSubLot6");
            obj.Subdivision7.Should().Be("subdivision7");
            obj.LotOrParcel8.Should().Be("lotOrParcel8");
            obj.Section9.Should().Be("section9");
            obj.Quadrant10.Should().Be("quadrant10");
            obj.BlockOrLot11.Should().Be("blockOrLot11");
            obj.LotOrParcel12.Should().Be("lotOrParcel12");
            obj.ParcelOrBlock13.Should().Be("parcelOrBlock13");
            obj.ConcatShortLegal.Should().Be("concatShortLegal");
            obj.MarginalNotes.Should().Be("marginalNotes");
        }
    }
}
