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
    public class PendingApplicationTest
    {
        [Fact]
        public void TestConstructor()
        {
            PendingApplication obj = new PendingApplication("applicationNumber", "transactionType", true);
            obj.ApplicationNumber.Should().Be("applicationNumber");
            obj.TransactionType.Should().Be("transactionType");
            obj.Defected.Should().Be(true);
        }
    }
}
