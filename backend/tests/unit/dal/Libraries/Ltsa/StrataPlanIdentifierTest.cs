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
    public class StrataPlanIdentifierTest
    {
        [Fact]
        public void TestConstructor()
        {
            StrataPlanIdentifier obj = new StrataPlanIdentifier("strataPlanNumber", LandTitleDistrict.KAMLOOPS);
            obj.StrataPlanNumber.Should().Be("strataPlanNumber");
            obj.LandTitleDistrict.Should().Be(LandTitleDistrict.KAMLOOPS);
        }
    }
}
