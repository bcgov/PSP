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
            AssociatedPlan obj = new AssociatedPlan(AssociatedPlan.PlanTypeEnum.AIRSPACEPLAN, "planNumber");
            obj.PlanType.Should().Be(AssociatedPlan.PlanTypeEnum.AIRSPACEPLAN);
            obj.PlanNumber.Should().Be("planNumber");
        }
    }
}
