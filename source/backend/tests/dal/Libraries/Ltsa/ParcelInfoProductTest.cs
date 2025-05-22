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
    public class ParcelInfoProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            LegalDescription legalDescription = new(fullLegalDescription: "fullLegalDescription");
            ParcelInfo parcelInfo = new("parcelIdentifier", registeredTitlesCount: 1, pendingApplicationCount: 2, legalDescription: legalDescription);
            ParcelInfoProduct obj = new(parcelInfo);
            obj.ParcelInfo.Should().Be(parcelInfo);
        }
    }
}
