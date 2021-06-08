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
    public class DocOrPlanSummaryTest
    {
        [Fact]
        public void TestConstructor()
        {
            DocOrPlanSummary obj = new DocOrPlanSummary("docOrPlanNumber", "documentDistrict", LandTitleDistrictCode.KA);
            obj.DocOrPlanNumber.Should().Be("docOrPlanNumber");
            obj.DocumentDistrict.Should().Be("documentDistrict");
            obj.DocumentDistrictCode.Should().Be(LandTitleDistrictCode.KA);
        }
    }
}
