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
    public class StrataPlanCommonPropertyProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            StrataPlanCommonProperty strataCommonProperty = new();
            StrataPlanCommonPropertyProduct obj = new(strataCommonProperty);
            obj.StrataPlanCommonProperty.Should().Be(strataCommonProperty);
        }
    }
}
