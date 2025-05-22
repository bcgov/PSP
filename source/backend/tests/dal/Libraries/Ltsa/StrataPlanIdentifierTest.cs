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
    public class StrataPlanIdentifierTest
    {
        [Fact]
        public void TestConstructor()
        {
            StrataPlanIdentifier obj = new("strataPlanNumber", LandTitleDistrict.KAMLOOPS);
            obj.StrataPlanNumber.Should().Be("strataPlanNumber");
            obj.LandTitleDistrict.Should().Be(LandTitleDistrict.KAMLOOPS);
        }
    }
}
