using System.Collections.Generic;
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
    public class StrataPlanCommonPropertyTest
    {
        [Fact]
        public void TestConstructor()
        {
            StrataPlanIdentifier identifier = new();
            List<LegalNotationsOnStrataCommonProperty> legalNotations = new();
            List<ChargesOnStrataCommonProperty> chargesOnStrataCommonProperties = new();
            StrataPlanCommonProperty obj = new(identifier, legalNotations, chargesOnStrataCommonProperties);
            obj.StrataPlanIdentifier.Should().Be(identifier);
            obj.LegalNotationsOnSCP.Should().BeEquivalentTo(legalNotations);
            obj.ChargesOnSCP.Should().BeEquivalentTo(chargesOnStrataCommonProperties);
        }
    }
}
