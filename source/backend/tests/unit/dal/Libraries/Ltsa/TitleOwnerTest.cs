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
    public class TitleOwnerTest
    {
        [Fact]
        public void TestConstructor_Null_TitleIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new TitleOwner(lastNameOrCorpName1: null));
        }

        [Fact]
        public void TestConstructor()
        {
            OwnerAddress ownerAddress = new();
            TitleOwner obj = new("lastNameOrCorpName1", "lastNameOrCorpName2", "givenName", "incorporationNumber", "occupationDescription", ownerAddress);
            obj.LastNameOrCorpName1.Should().Be("lastNameOrCorpName1");
            obj.LastNameOrCorpName2.Should().Be("lastNameOrCorpName2");
            obj.GivenName.Should().Be("givenName");
            obj.IncorporationNumber.Should().Be("incorporationNumber");
            obj.OccupationDescription.Should().Be("occupationDescription");
            obj.Address.Should().Be(ownerAddress);
        }
    }
}
