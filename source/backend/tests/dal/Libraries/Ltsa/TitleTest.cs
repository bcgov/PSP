using System;
using System.Collections.Generic;
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
    public class TitleTest
    {
        [Fact]
        public void TestConstructor_Null_TitleIdentifier()
        {
            Assert.Throws<InvalidDataException>(() => new Title(ownershipGroups: new List<TitleOwnershipGroup>(), taxAuthorities: new List<TaxAuthority>(), descriptionsOfLand: new List<DescriptionOfLand>(), tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>()), titleIdentifier: null));
        }

        [Fact]
        public void TestConstructor_Null_Tombstone()
        {
            Assert.Throws<InvalidDataException>(() => new Title(titleIdentifier: new TitleIdentifier(titleNumber: "titleNumber"), ownershipGroups: new List<TitleOwnershipGroup>(), taxAuthorities: new List<TaxAuthority>(), descriptionsOfLand: new List<DescriptionOfLand>(), tombstone: null));
        }

        [Fact]
        public void TestConstructor_Null_OwnershipGroups()
        {
            Assert.Throws<InvalidDataException>(() => new Title(titleIdentifier: new TitleIdentifier(titleNumber: "titleNumber"), taxAuthorities: new List<TaxAuthority>(), descriptionsOfLand: new List<DescriptionOfLand>(), tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>()), ownershipGroups: null));
        }

        [Fact]
        public void TestConstructor_Null_TaxAuthorities()
        {
            Assert.Throws<InvalidDataException>(() => new Title(titleIdentifier: new TitleIdentifier(titleNumber: "titleNumber"), ownershipGroups: new List<TitleOwnershipGroup>(), descriptionsOfLand: new List<DescriptionOfLand>(), tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>()), taxAuthorities: null));
        }
        [Fact]
        public void TestConstructor_Null_DescriptionsOfLand()
        {
            Assert.Throws<InvalidDataException>(() => new Title(titleIdentifier: new TitleIdentifier(titleNumber: "titleNumber"), ownershipGroups: new List<TitleOwnershipGroup>(), taxAuthorities: new List<TaxAuthority>(), tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>()), descriptionsOfLand: null));
        }

        [Fact]
        public void TestConstructor()
        {
            TitleIdentifier titleIdentifier = new(titleNumber: "titleNumber");
            TitleTombstone tombstone = new(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>());
            List<TitleOwnershipGroup> ownershipGroups = new();
            List<TaxAuthority> taxAuthorities = new();
            List<DescriptionOfLand> descriptionsOfLand = new();
            List<LegalNotationOnTitle> legalNotationsOnTitle = new();
            List<ChargeOnTitle> chargesOnTitle = new();
            List<DuplicateCertificate> duplicateCertificatesOfTitle = new();
            List<TitleTransferDisposition> titleTransfersOrDispositions = new();
            List<Altos1TitleCorrection> correctionsAltos1 = new();
            List<TitleCorrection> corrections = new();
            Title obj = new(Title.TitleStatusEnum.REGISTERED, titleIdentifier, tombstone, ownershipGroups, taxAuthorities, descriptionsOfLand, legalNotationsOnTitle,
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
