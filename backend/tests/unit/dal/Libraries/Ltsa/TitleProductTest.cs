using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Ltsa.Models;
using Xunit;

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
            Title title = new(titleIdentifier: new TitleIdentifier("titleNumber"), ownershipGroups: new List<TitleOwnershipGroup>(), taxAuthorities: new List<TaxAuthority>(),
                tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now, natureOfTransfers: new List<NatureOfTransfer>()), descriptionsOfLand: new List<DescriptionOfLand>());

            TitleProduct obj = new(title, "href");
            obj.Title.Should().Be(title);
            obj.Href.Should().Be("href");
        }
    }
}
