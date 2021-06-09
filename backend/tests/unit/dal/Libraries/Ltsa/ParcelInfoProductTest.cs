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
    public class ParcelInfoProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            LegalDescription legalDescription = new LegalDescription(fullLegalDescription: "fullLegalDescription");
            ParcelInfo parcelInfo = new ParcelInfo("parcelIdentifier", registeredTitlesCount: 1, pendingApplicationCount: 2, legalDescription: legalDescription);
            ParcelInfoProduct obj = new ParcelInfoProduct(parcelInfo);
            obj.ParcelInfo.Should().Be(parcelInfo);
        }
    }
}
