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
            List<SubdividedShortLegalDescription> subdividedShortLegalDescriptions = new List<SubdividedShortLegalDescription>();
            List<UnsubdividedShortLegalDescription> unsubdividedShortLegalDescriptions = new List<UnsubdividedShortLegalDescription>();
            LegalDescription obj = new LegalDescription("fullLegalDescription", subdividedShortLegalDescriptions, unsubdividedShortLegalDescriptions);
            obj.FullLegalDescription.Should().Be("fullLegalDescription");
            obj.SubdividedShortLegals.Should().BeEquivalentTo(subdividedShortLegalDescriptions);
            obj.UnsubdividedShortLegals.Should().BeEquivalentTo(unsubdividedShortLegalDescriptions);
        }
    }
}
