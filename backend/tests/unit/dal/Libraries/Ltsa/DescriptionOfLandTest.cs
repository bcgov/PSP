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
    public class DescriptionOfLandTest
    {
        [Fact]
        public void TestConstructor_Null_ChargeNumber()
        {
            Assert.Throws<InvalidDataException>(() => new DescriptionOfLand(parcelIdentifier: null));
        }

        [Fact]
        public void TestConstructor_Null_TransactionType()
        {
            Assert.Throws<InvalidDataException>(() => new DescriptionOfLand(fullLegalDescription: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DescriptionOfLand obj = new DescriptionOfLand("parcelIdentifier", "fullLegalDescription", DescriptionOfLand.ParcelStatusEnum.AActive);
            obj.ParcelIdentifier.Should().Be("parcelIdentifier");
            obj.FullLegalDescription.Should().Be("fullLegalDescription");
            obj.ParcelStatus.Should().Be(DescriptionOfLand.ParcelStatusEnum.AActive);
        }
    }
}
