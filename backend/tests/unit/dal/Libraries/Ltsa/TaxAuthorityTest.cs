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
            TaxAuthority obj = new TaxAuthority("authorityName");
            obj.AuthorityName.Should().Be("authorityName");
        }
    }
}
