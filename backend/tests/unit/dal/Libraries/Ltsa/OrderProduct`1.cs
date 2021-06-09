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
    public class OrderProductTest
    {
        [Fact]
        public void TestConstructor()
        {
            TitleOrder titleOrder = new TitleOrder();
            OrderedProduct<TitleOrder> obj = new OrderedProduct<TitleOrder>() { FieldedData = titleOrder };
            obj.FieldedData.Should().Be(titleOrder);
        }
    }
}
