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
    public class NatureOfTransferTest
    {
        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new NatureOfTransfer(transferReason: null));
        }

        [Fact]
        public void TestConstructor()
        {
            NatureOfTransfer obj = new("transferReason");
            obj.TransferReason.Should().Be("transferReason");
        }
    }
}
