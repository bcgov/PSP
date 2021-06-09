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
            CertificateIdentifier certificateIdentifier = new CertificateIdentifier("documentNumber");
            Assert.Throws<InvalidDataException>(() => new DuplicateCertificate(issuedDate: DateTime.Now, certificateIdentifier: certificateIdentifier, certificateDelivery: null));
        }
        [Fact]
        public void TestConstructor()
        {
            DateTime issuedDate = DateTime.Now;
            DateTime surrenderedDate = DateTime.Now;
            CertificateIdentifier certificateIdentifier = new CertificateIdentifier("documentNumber");
            CertificateDelivery certificateDelivery = new CertificateDelivery("intendedRecipientLastName", "intendedRecipientGivenName");
            DuplicateCertificate obj = new DuplicateCertificate(issuedDate, surrenderedDate, certificateIdentifier, certificateDelivery);
            obj.IssuedDate.Should().Be(issuedDate);
            obj.SurrenderDate.Should().Be(surrenderedDate);
            obj.CertificateIdentifier.Should().Be(certificateIdentifier);
            obj.CertificateDelivery.Should().Be(certificateDelivery);
        }
    }
}
