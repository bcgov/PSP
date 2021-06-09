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
    public class StrataPlanCommonPropertyProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            StrataPlanCommonProperty strataCommonProperty = new StrataPlanCommonProperty();
            StrataPlanCommonPropertyProduct obj = new StrataPlanCommonPropertyProduct(strataCommonProperty);
            obj.StrataPlanCommonProperty.Should().Be(strataCommonProperty);
        }
    }
}
