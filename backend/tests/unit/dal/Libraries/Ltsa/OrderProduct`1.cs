using FluentAssertions;
using Pims.Ltsa.Models;
using System.Diagnostics.CodeAnalysis;
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
            TitleOrder titleOrder = new();
            OrderedProduct<TitleOrder> obj = new() { FieldedData = titleOrder };
            obj.FieldedData.Should().Be(titleOrder);
        }
    }
}
