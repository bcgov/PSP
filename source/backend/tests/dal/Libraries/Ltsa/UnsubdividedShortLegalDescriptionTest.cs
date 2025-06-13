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
    public class UnsubdividedShortLegalDescriptionTest
    {
        [Fact]
        public void TestConstructor_Null_LandDistrict1()
        {
            Assert.Throws<InvalidDataException>(() => new UnsubdividedShortLegalDescription(landDistrict1: null));
        }

        [Fact]
        public void TestConstructor_Null_ConcatShortLegal()
        {
            Assert.Throws<InvalidDataException>(() => new UnsubdividedShortLegalDescription(landDistrict1: "landDistrict1", concatShortLegal: null));
        }

        [Fact]
        public void TestConstructor()
        {
            UnsubdividedShortLegalDescription obj = new("landDistrict1", "meridian2", "range3", "townshipOrIsland4", "group5",
                "block6", "districtLotOrLotOrSection7", "section8", "quadrant9", "blockOrSection10", "subdivision11", "lotOrSubLotOrParcel12", "lotOrParcel13",
                "mineralClaimIOrIndianReserveName14", "mineralClaimOrIndianReserveNumber15", "concatShortLegal", "marginalNotes");
            obj.LandDistrict1.Should().Be("landDistrict1");
            obj.Meridian2.Should().Be("meridian2");
            obj.Range3.Should().Be("range3");
            obj.TownshipOrIsland4.Should().Be("townshipOrIsland4");
            obj.Group5.Should().Be("group5");
            obj.Block6.Should().Be("block6");
            obj.DistrictLotOrLotOrSection7.Should().Be("districtLotOrLotOrSection7");
            obj.Section8.Should().Be("section8");
            obj.Quadrant9.Should().Be("quadrant9");
            obj.BlockOrSection10.Should().Be("blockOrSection10");
            obj.Subdivision11.Should().Be("subdivision11");
            obj.LotOrSubLotOrParcel12.Should().Be("lotOrSubLotOrParcel12");
            obj.LotOrParcel13.Should().Be("lotOrParcel13");
            obj.MineralClaimIOrIndianReserveName14.Should().Be("mineralClaimIOrIndianReserveName14");
            obj.MineralClaimOrIndianReserveNumber15.Should().Be("mineralClaimOrIndianReserveNumber15");
            obj.ConcatShortLegal.Should().Be("concatShortLegal");
            obj.MarginalNotes.Should().Be("marginalNotes");
        }
    }
}
