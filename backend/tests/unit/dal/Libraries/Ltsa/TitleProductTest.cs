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
    public class TitleProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            Title title = new Title(titleIdentifier: new TitleIdentifier("titleNumber"), ownershipGroups: new List<TitleOwnershipGroup>(), taxAuthorities: new List<TaxAuthority>(),
                tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>()), descriptionsOfLand: new List<DescriptionOfLand>());

            TitleProduct obj = new TitleProduct(title, "href");
            obj.Title.Should().Be(title);
            obj.Href.Should().Be("href");
        }
    }
}
