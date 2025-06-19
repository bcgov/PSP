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
    public class BillingInfoTest
    {
        [Fact]
        public void TestConstructor()
        {
            BillingInfo obj = new(BillingInfo.BillingModelEnum.PROV, "productName", "productCode", true, 1, 2, 3, 4, 5, 6, 7);
            obj.BillingModel.Should().Be(BillingInfo.BillingModelEnum.PROV);
            obj.ProductName.Should().Be("productName");
            obj.ProductCode.Should().Be("productCode");
            obj.FeeExempted.Should().Be(true);
            obj.ProductFee.Should().Be(1);
            obj.ServiceCharge.Should().Be(2);
            obj.SubtotalFee.Should().Be(3);
            obj.ProductFeeTax.Should().Be(4);
            obj.ServiceChargeTax.Should().Be(5);
            obj.TotalTax.Should().Be(6);
            obj.TotalFee.Should().Be(7);
        }
    }
}
