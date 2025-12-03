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
    public class DocOrPlanOrderTest
    {
        [Fact]
        public void TestConstructor()
        {
            var docParams = new DocumentOrPlanOrderParameters("docOrPlanNumber");
            DocOrPlanOrder obj = new(docParams);
            obj.ProductOrderParameters.Should().Be(docParams);
        }
    }
}
