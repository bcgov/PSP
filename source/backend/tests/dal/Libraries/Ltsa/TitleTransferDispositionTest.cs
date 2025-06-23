using System;
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
    public class TitleTransferDispositionTest
    {
        [Fact]
        public void TestConstructor_Null_NatureOfTransfers()
        {
            Assert.Throws<InvalidDataException>(() => new TitleTransferDisposition(titleNumber: null));
        }

        [Fact]
        public void TestConstructor()
        {
            DateTime dispositionDate = DateTime.Now;
            DateTime acceptanceDate = DateTime.Now;
            TitleTransferDisposition obj = new("disposition", dispositionDate, acceptanceDate, "titleNumber", LandTitleDistrict.KAMLOOPS);
            obj.Disposition.Should().Be("disposition");
            obj.DispositionDate.Should().Be(dispositionDate);
            obj.AcceptanceDate.Should().Be(acceptanceDate);
            obj.TitleNumber.Should().Be("titleNumber");
            obj.LandLandDistrict.Should().Be(LandTitleDistrict.KAMLOOPS);
        }
    }
}
