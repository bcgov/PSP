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
    public class OrderCancellationResponseTest
    {
        [Fact]
        public void TestConstructor()
        {
            OrderCancellationResponse obj = new("orderId", "orderCancellationID", "status");
            obj.OrderId.Should().Be("orderId");
            obj.OrderCancellationID.Should().Be("orderCancellationID");
            obj.Status.Should().Be("status");
        }
    }
}
