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
            NatureOfTransfer obj = new NatureOfTransfer("transferReason");
            obj.TransferReason.Should().Be("transferReason");
        }
    }
}
