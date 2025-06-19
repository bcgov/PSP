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
    public class TaxAuthorityTest
    {
        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new TaxAuthority(authorityName: null));
        }

        [Fact]
        public void TestConstructor()
        {
            TaxAuthority obj = new("authorityName");
            obj.AuthorityName.Should().Be("authorityName");
        }
    }
}
