using FluentAssertions;
using Pims.Ltsa.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Xunit;

namespace Pims.Dal.Test.Libraries.Ltsa
{
    [Trait("category", "unit")]
    [Trait("category", "ltsa")]
    [Trait("group", "ltsa")]
    [ExcludeFromCodeCoverage]
    public class CertificateOfChargeTest
    {
        [Fact]
        public void TestConstructor_Null_Number()
        {
            Assert.Throws<InvalidDataException>(() => new CertificateOfCharge(null, "", DateTime.Now));
        }

        [Fact]
        public void TestConstructor_Null_Type()
        {
            Assert.Throws<InvalidDataException>(() => new CertificateOfCharge("", null, DateTime.Now));
        }

        [Fact]
        public void TestConstructor_Null_IssuedDate()
        {
            Assert.Throws<InvalidDataException>(() => new CertificateOfCharge("", "", null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime issuedDate = DateTime.Now;
            DateTime surrenderDate = DateTime.Now;
            CertificateOfCharge obj = new("number", "type", issuedDate, surrenderDate);
            obj.Number.Should().Be("number");
            obj.Type.Should().Be("type");
            obj.IssuedDate.Should().Be(issuedDate);
            obj.SurrenderDate.Should().Be(surrenderDate);
        }
    }
}
