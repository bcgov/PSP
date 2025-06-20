using System;
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
    public class OrderProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            Title title = new(titleIdentifier: new TitleIdentifier("titleNumber"), tombstone: new TitleTombstone(applicationReceivedDate: DateTime.Now, enteredDate: DateTime.Now,
                natureOfTransfers: new System.Collections.Generic.List<NatureOfTransfer>()), ownershipGroups: new System.Collections.Generic.List<TitleOwnershipGroup>(),
                taxAuthorities: new System.Collections.Generic.List<TaxAuthority>(),
                descriptionsOfLand: new System.Collections.Generic.List<DescriptionOfLand>());
            OrderedProduct<IFieldedData> obj = new() { FieldedData = (IFieldedData)title };
            obj.FieldedData.Should().Be(title);
        }
    }
}
