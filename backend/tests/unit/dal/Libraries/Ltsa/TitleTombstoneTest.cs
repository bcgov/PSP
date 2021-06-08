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
    public class TitleTombstoneTest
    {
        [Fact]
        public void TestConstructor_Null_NatureOfTransfers()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTombstone(natureOfTransfers: null));
        }
        [Fact]
        public void TestConstructor_Null_ApplicationReceivedDate()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTombstone(applicationReceivedDate: null));
        }

        [Fact]
        public void TestConstructor_Null_EnteredDate()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTombstone(enteredDate: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime applicationReceivedDate = DateTime.Now;
            DateTime enteredDate = DateTime.Now;
            DateTime cancellationDate = DateTime.Now;
            List<TitleIdentifier> fromTitles = new List<TitleIdentifier>();
            List<NatureOfTransfer> natureOfTransfers = new List<NatureOfTransfer>();

            TitleTombstone obj = new TitleTombstone(applicationReceivedDate, enteredDate, "titleRemarks", "rootOfTitle", cancellationDate, "marketValueAmount", fromTitles, natureOfTransfers);
            obj.ApplicationReceivedDate.Should().Be(applicationReceivedDate);
            obj.EnteredDate.Should().Be(enteredDate);
            obj.TitleRemarks.Should().Be("titleRemarks");
            obj.RootOfTitle.Should().Be("rootOfTitle");
            obj.CancellationDate.Should().Be(cancellationDate);
            obj.MarketValueAmount.Should().Be("marketValueAmount");
            obj.FromTitles.Should().BeEquivalentTo(fromTitles);
            obj.NatureOfTransfers.Should().BeEquivalentTo(natureOfTransfers);
        }
    }
}
