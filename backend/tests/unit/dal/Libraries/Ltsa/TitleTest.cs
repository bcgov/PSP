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
    public class TitleTest
    {
        [Fact]
        public void TestConstructor_Null_TitleIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new Title(titleIdentifier: null));
        }

        [Fact]
        public void TestConstructor_Null_Tombstone()
        {
            Assert.Throws<InvalidDataException>(() => new Title(tombstone: null));
        }

        [Fact]
        public void TestConstructor_Null_OwnershipGroups()
        {
            Assert.Throws<InvalidDataException>(() => new Title(ownershipGroups: null));
        }

        [Fact]
        public void TestConstructor_Null_TaxAuthorities()
        {
            Assert.Throws<InvalidDataException>(() => new Title(taxAuthorities: null));
        }
        [Fact]
        public void TestConstructor_Null_DescriptionsOfLand()
        {
            Assert.Throws<InvalidDataException>(() => new Title(descriptionsOfLand: null));
        }

        [Fact]
        public void TestConstructor()
        {
            TitleIdentifier titleIdentifier = new TitleIdentifier(titleNumber: "titleNumber");
            TitleTombstone tombstone = new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>());
            List<TitleOwnershipGroup> ownershipGroups = new List<TitleOwnershipGroup>();
            List<TaxAuthority> taxAuthorities = new List<TaxAuthority>();
            List<DescriptionOfLand> descriptionsOfLand = new List<DescriptionOfLand>();
            List<LegalNotationOnTitle> legalNotationsOnTitle = new List<LegalNotationOnTitle>();
            List<ChargeOnTitle> chargesOnTitle = new List<ChargeOnTitle>();
            List<DuplicateCertificate> duplicateCertificatesOfTitle = new List<DuplicateCertificate>();
            List<TitleTransferDisposition> titleTransfersOrDispositions = new List<TitleTransferDisposition>();
            List<Altos1TitleCorrection> correctionsAltos1 = new List<Altos1TitleCorrection>();
            List<TitleCorrection> corrections = new List<TitleCorrection>();
            Title obj = new Title(Title.TitleStatusEnum.REGISTERED, titleIdentifier, tombstone, ownershipGroups, taxAuthorities, descriptionsOfLand, legalNotationsOnTitle,
                chargesOnTitle, duplicateCertificatesOfTitle, titleTransfersOrDispositions, correctionsAltos1, corrections);
            obj.TitleStatus.Should().Be(Title.TitleStatusEnum.REGISTERED);
            obj.TitleIdentifier.Should().Be(titleIdentifier);
            obj.Tombstone.Should().Be(tombstone);
            obj.OwnershipGroups.Should().BeEquivalentTo(ownershipGroups);
            obj.TaxAuthorities.Should().BeEquivalentTo(taxAuthorities);
            obj.DescriptionsOfLand.Should().BeEquivalentTo(descriptionsOfLand);
            obj.LegalNotationsOnTitle.Should().BeEquivalentTo(legalNotationsOnTitle);
            obj.ChargesOnTitle.Should().BeEquivalentTo(chargesOnTitle);
            obj.DuplicateCertificatesOfTitle.Should().BeEquivalentTo(duplicateCertificatesOfTitle);
            obj.TitleTransfersOrDispositions.Should().BeEquivalentTo(titleTransfersOrDispositions);
            obj.CorrectionsAltos1.Should().BeEquivalentTo(correctionsAltos1);
            obj.Corrections.Should().BeEquivalentTo(corrections);
        }
    }
}
