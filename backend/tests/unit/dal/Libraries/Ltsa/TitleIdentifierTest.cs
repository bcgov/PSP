using FluentAssertions;
using Pims.Ltsa.Models;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ltsa
{
    [Trait("category", "unit")]
    [Trait("category", "ltsa")]
    [Trait("group", "ltsa")]
    [ExcludeFromCodeCoverage]
    public class TitleIdentifierTest
    {
        [Fact]
        public void TestConstructor_Null_TitleIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new TitleIdentifier(titleNumber: null));
        }

        [Fact]
        public void TestConstructor()
        {
            TitleIdentifier obj = new("titleNumber", LandTitleDistrict.KAMLOOPS);
            obj.TitleNumber.Should().Be("titleNumber");
            obj.LandLandDistrict.Should().Be(LandTitleDistrict.KAMLOOPS);
        }
    }
}
