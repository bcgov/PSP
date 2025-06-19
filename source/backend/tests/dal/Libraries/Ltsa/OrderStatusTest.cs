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
    public class OrderStatusTest
    {
        [Fact]
        public void TestConstructor()
        {
            OrderStatus obj = new(OrderStatus.StatusEnum.Cancelled);
            obj.Status.Should().Be(OrderStatus.StatusEnum.Cancelled);
        }
    }
}
