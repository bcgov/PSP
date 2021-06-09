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
    public class StrataPlanCommonPropertyTest
    {
        [Fact]
        public void TestConstructor()
        {
            StrataPlanIdentifier identifier = new StrataPlanIdentifier();
            List<LegalNotationsOnStrataCommonProperty> legalNotations = new List<LegalNotationsOnStrataCommonProperty>();
            List<ChargesOnStrataCommonProperty> chargesOnStrataCommonProperties = new List<ChargesOnStrataCommonProperty>();
            StrataPlanCommonProperty obj = new StrataPlanCommonProperty(identifier, legalNotations, chargesOnStrataCommonProperties);
            obj.StrataPlanIdentifier.Should().Be(identifier);
            obj.LegalNotationsOnSCP.Should().BeEquivalentTo(legalNotations);
            obj.ChargesOnSCP.Should().BeEquivalentTo(chargesOnStrataCommonProperties);
        }
    }
}
