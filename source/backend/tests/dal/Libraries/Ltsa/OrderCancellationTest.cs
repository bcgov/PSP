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
            OrderCancellation obj = new("orderId");
            obj.OrderId.Should().Be("orderId");
        }
    }
}
