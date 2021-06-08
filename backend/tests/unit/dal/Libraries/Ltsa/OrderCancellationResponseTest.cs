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
    public class OrderCancellationResponseTest
    {
        [Fact]
        public void TestConstructor()
        {
            OrderCancellationResponse obj = new OrderCancellationResponse("orderId", "orderCancellationID", "status");
            obj.OrderId.Should().Be("orderId");
            obj.OrderCancellationID.Should().Be("orderCancellationID");
            obj.Status.Should().Be("status");
        }
    }
}
