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
    public class DocumentOrPlanOrderParametersTest
    {
        [Fact]
        public void TestConstructor_Null_DocOrPlanNumber()
        {
            Assert.Throws<InvalidDataException>(() => new DocumentOrPlanOrderParameters(docOrPlanNumber: null));
        }

        [Fact]
        public void TestConstructor_Null_IncludePlanCertificationPage()
        {
            Assert.Throws<InvalidDataException>(() => new DocumentOrPlanOrderParameters(includePlanCertificationPage: null));
        }

        [Fact]
        public void TestConstructor()
        {
            var landTitleDistrictCode = new LandTitleDistrictCode();
            DocumentOrPlanOrderParameters obj = new DocumentOrPlanOrderParameters("docOrPlanNumber", "orderRemarks", true, landTitleDistrictCode);
            obj.DocOrPlanNumber.Should().Be("docOrPlanNumber");
            obj.OrderRemarks.Should().Be("orderRemarks");
            obj.IncludePlanCertificationPage.Should().Be(true);
            obj.DocumentDistrictCode.Should().BeEquivalentTo(landTitleDistrictCode);
        }
    }
}
