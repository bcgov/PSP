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
    public class OrderCancellationTest
    {
        [Fact]
        public void TestConstructor_Null_OrderId()
        {
            Assert.Throws<InvalidDataException>(() => new OrderCancellation(orderId: null));
        }
        [Fact]
        public void TestConstructor()
        {
            OrderCancellation obj = new OrderCancellation("orderId");
            obj.OrderId.Should().Be("orderId");
        }
    }
}
