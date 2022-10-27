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
            CertificateIdentifier obj = new("documentNumber", LandTitleDistrictCode.KA);
            obj.DocumentNumber.Should().Be("documentNumber");
            obj.DocumentDistrictCode.Should().Be(LandTitleDistrictCode.KA);
        }
    }
}
