using System.Collections.Generic;
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
    public class LegalDescriptionTest
    {
        [Fact]
        public void TestConstructor_Null_FullLegalDescription()
        {
            Assert.Throws<InvalidDataException>(() => new LegalDescription(fullLegalDescription: null));
        }

        [Fact]
        public void TestConstructor()
        {
            List<SubdividedShortLegalDescription> subdividedShortLegalDescriptions = new();
            List<UnsubdividedShortLegalDescription> unsubdividedShortLegalDescriptions = new();
            LegalDescription obj = new("fullLegalDescription", subdividedShortLegalDescriptions, unsubdividedShortLegalDescriptions);
            obj.FullLegalDescription.Should().Be("fullLegalDescription");
            obj.SubdividedShortLegals.Should().BeEquivalentTo(subdividedShortLegalDescriptions);
            obj.UnsubdividedShortLegals.Should().BeEquivalentTo(unsubdividedShortLegalDescriptions);
        }
    }
}
