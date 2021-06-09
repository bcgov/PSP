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
    public class OrderStatusTest
    {
        [Fact]
        public void TestConstructor()
        {
            OrderStatus obj = new OrderStatus(OrderStatus.StatusEnum.Cancelled);
            obj.Status.Should().Be(OrderStatus.StatusEnum.Cancelled);
        }
    }
}
