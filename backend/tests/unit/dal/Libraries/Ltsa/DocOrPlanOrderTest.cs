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
    public class DocOrPlanOrderTest
    {
        [Fact]
        public void TestConstructor()
        {
            var docParams = new DocumentOrPlanOrderParameters("docOrPlanNumber");
            DocOrPlanOrder obj = new DocOrPlanOrder(docParams);
            obj.ProductOrderParameters.Should().Be(docParams);
        }
    }
}
