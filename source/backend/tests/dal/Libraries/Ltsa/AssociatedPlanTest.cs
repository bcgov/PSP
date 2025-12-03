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
    public class AssociatedPlanTest
    {
        [Fact]
        public void TestConstructor_Null_PlanNumber()
        {
            Assert.Throws<InvalidDataException>(() => new AssociatedPlan(AssociatedPlan.PlanTypeEnum.AIRSPACEPLAN, null));
        }

        [Fact]
        public void TestConstructor()
        {
            AssociatedPlan obj = new(AssociatedPlan.PlanTypeEnum.AIRSPACEPLAN, "planNumber");
            obj.PlanType.Should().Be(AssociatedPlan.PlanTypeEnum.AIRSPACEPLAN);
            obj.PlanNumber.Should().Be("planNumber");
        }
    }
}
