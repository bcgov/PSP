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
    public class CertificateIdentifierTest
    {
        [Fact]
        public void TestConstructor_Null_DocumentNumber()
        {
            Assert.Throws<InvalidDataException>(() => new CertificateIdentifier(null, LandTitleDistrictCode.KA));
        }

        [Fact]
        public void TestConstructor()
        {
            CertificateIdentifier obj = new CertificateIdentifier("documentNumber", LandTitleDistrictCode.KA);
            obj.DocumentNumber.Should().Be("documentNumber");
            obj.DocumentDistrictCode.Should().Be(LandTitleDistrictCode.KA);
        }
    }
}
