using System;
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
    public class DuplicateCertificateTest
    {
        [Fact]
        public void TestConstructor_Null_IssuedDate()
        {
            Assert.Throws<InvalidDataException>(() => new DuplicateCertificate(issuedDate: null));
        }
        [Fact]
        public void TestConstructor_Null_CertificateIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new DuplicateCertificate(issuedDate: DateTime.Now, certificateIdentifier: null));
        }
        [Fact]
        public void TestConstructor_Null_CertificateDelivery()
        {
            CertificateIdentifier certificateIdentifier = new("documentNumber");
            Assert.Throws<InvalidDataException>(() => new DuplicateCertificate(issuedDate: DateTime.Now, certificateIdentifier: certificateIdentifier, certificateDelivery: null));
        }
        [Fact]
        public void TestConstructor()
        {
            DateTime issuedDate = DateTime.Now;
            DateTime surrenderedDate = DateTime.Now;
            CertificateIdentifier certificateIdentifier = new("documentNumber");
            CertificateDelivery certificateDelivery = new("intendedRecipientLastName", "intendedRecipientGivenName");
            DuplicateCertificate obj = new(issuedDate, surrenderedDate, certificateIdentifier, certificateDelivery);
            obj.IssuedDate.Should().Be(issuedDate);
            obj.SurrenderDate.Should().Be(surrenderedDate);
            obj.CertificateIdentifier.Should().Be(certificateIdentifier);
            obj.CertificateDelivery.Should().Be(certificateDelivery);
        }
    }
}
