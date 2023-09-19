using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pims.Dal.Helpers.Extensions;
using Xunit;

namespace Pims.Core.Test.Extensions
{
    [Trait("category", "unit")]
    [Trait("category", "core")]
    [Trait("category", "function")]
    [ExcludeFromCodeCoverage]

    public class AcquisitionFileExtensionsTests
    {
        private readonly TestHelper _helper;

        public AcquisitionFileExtensionsTests()
        {
            this._helper = new TestHelper();
        }

        [Fact]
        public void AcquisitionFile_GetLegacyInterestHolders()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.LegacyStakeholder = "<Bob Jones||NameOnly><Robert Jones Inc.||NameOnly>";

            // Act
            var sut = acqFile.GetLegacyInterestHolders();

            // Assert
            Assert.Equal(2, sut.Count());
            Assert.Equal("Bob Jones", sut.First());
            Assert.Equal("Robert Jones Inc.", sut[1]);
        }

        [Fact]
        public void AcquisitionFile_GetLegacyInterestHolders_returns_EmptyWhen_Null()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.LegacyStakeholder = null;

            // Act
            var sut = acqFile.GetLegacyInterestHolders();

            // Assert
            Assert.Equal(0, sut.Count());
        }

        [Fact]
        public void AcquisitionFile_GetLegacyInterestHolders_returns_EmptyWhen_Empty()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            acqFile.LegacyStakeholder = string.Empty;

            // Act
            var sut = acqFile.GetLegacyInterestHolders();

            // Assert
            Assert.Equal(0, sut.Count());
        }
    }
}
