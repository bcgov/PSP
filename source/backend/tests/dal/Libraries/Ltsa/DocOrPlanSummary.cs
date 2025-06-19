using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Ltsa.Models;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ltsa
{
    [Trait("category", "unit")]
    [Trait("category", "ltsa")]
    [Trait("group", "ltsa")]
    [ExcludeFromCodeCoverage]
    public class DocOrPlanSummaryTest
    {
        [Fact]
        public void TestConstructor()
        {
            DocOrPlanSummary obj = new("docOrPlanNumber", "documentDistrict", LandTitleDistrictCode.KA);
            obj.DocOrPlanNumber.Should().Be("docOrPlanNumber");
            obj.DocumentDistrict.Should().Be("documentDistrict");
            obj.DocumentDistrictCode.Should().Be(LandTitleDistrictCode.KA);
        }
    }
}
