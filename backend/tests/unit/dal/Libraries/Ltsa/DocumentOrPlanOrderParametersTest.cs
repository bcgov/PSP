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
    public class DocumentOrPlanOrderParametersTest
    {
        [Fact]
        public void TestConstructor_Null_DocOrPlanNumber()
        {
            Assert.Throws<InvalidDataException>(() => new DocumentOrPlanOrderParameters(includePlanCertificationPage: true, docOrPlanNumber: null));
        }

        [Fact]
        public void TestConstructor_Null_IncludePlanCertificationPage()
        {
            DocumentOrPlanOrderParameters obj = new(docOrPlanNumber: "docOrPlanNumber", includePlanCertificationPage: null);
            obj.IncludePlanCertificationPage.Should().Be(false);
        }

        [Fact]
        public void TestConstructor()
        {
            var landTitleDistrictCode = new LandTitleDistrictCode();
            DocumentOrPlanOrderParameters obj = new("docOrPlanNumber", "orderRemarks", true, landTitleDistrictCode);
            obj.DocOrPlanNumber.Should().Be("docOrPlanNumber");
            obj.OrderRemarks.Should().Be("orderRemarks");
            obj.IncludePlanCertificationPage.Should().Be(true);
            obj.DocumentDistrictCode.Should().BeEquivalentTo(landTitleDistrictCode);
        }
    }
}
