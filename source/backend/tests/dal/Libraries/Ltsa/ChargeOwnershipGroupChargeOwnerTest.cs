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
    public class ChargeOwnershipGroupChargeOwnerTest
    {
        [Fact]
        public void TestConstructor_Null_JointTenancyIndication()
        {
            Assert.Throws<InvalidDataException>(() => new ChargeOwnershipGroupChargeOwner(lastNameOrCorpName1: null));
        }

        [Fact]
        public void TestConstructor()
        {
            ChargeOwnershipGroupChargeOwner obj = new("lastNameOrCorpName1", "lastNameOrCorpName2", "givenName", "incorporationNumber");
            obj.LastNameOrCorpName1.Should().Be("lastNameOrCorpName1");
            obj.LastNameOrCorpName2.Should().Be("lastNameOrCorpName2");
            obj.GivenName.Should().Be("givenName");
            obj.IncorporationNumber.Should().Be("incorporationNumber");
        }
    }
}
