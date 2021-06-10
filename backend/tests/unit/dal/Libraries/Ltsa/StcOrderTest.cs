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
    public class StcOrderTest
    {
        [Fact]
        public void TestConstructor()
        {
            StateTitleCertificateOrderParameters stateTitleCertificateOrderParameters = new(titleNumber: "titleNumber");
            StcOrder obj = new(stateTitleCertificateOrderParameters);
            obj.ProductOrderParameters.Should().Be(stateTitleCertificateOrderParameters);
        }
    }
}
