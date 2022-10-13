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
    public class TitleTombstoneTest
    {
        [Fact]
        public void TestConstructor_Null_NatureOfTransfers()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: null));
        }
        [Fact]
        public void TestConstructor_Null_ApplicationReceivedDate()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTombstone(enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>(), applicationReceivedDate: null));
        }

        [Fact]
        public void TestConstructor_Null_EnteredDate()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTombstone(applicationReceivedDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>(), enteredDate: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime applicationReceivedDate = DateTime.Now;
            DateTime enteredDate = DateTime.Now;
            DateTime cancellationDate = DateTime.Now;
            List<TitleIdentifier> fromTitles = new();
            List<NatureOfTransfer> natureOfTransfers = new();

            TitleTombstone obj = new(applicationReceivedDate, enteredDate, "titleRemarks", "rootOfTitle", cancellationDate, "marketValueAmount", fromTitles, natureOfTransfers);
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
