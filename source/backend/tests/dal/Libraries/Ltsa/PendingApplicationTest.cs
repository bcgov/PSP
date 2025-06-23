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
    public class PendingApplicationTest
    {
        [Fact]
        public void TestConstructor()
        {
            PendingApplication obj = new("applicationNumber", "transactionType", true);
            obj.ApplicationNumber.Should().Be("applicationNumber");
            obj.TransactionType.Should().Be("transactionType");
            obj.Defected.Should().Be(true);
        }
    }
}
