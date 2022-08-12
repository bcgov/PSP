using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class AcquisitionControllerTest
    {
        #region Variables

        #endregion

        #region Tests
        /// <summary>
        /// Make a successful request to add an acquisition file to the datastore.
        /// </summary>
        [Fact]
        public void AddAcquisitionFile_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AcquisitionFileController>(Permissions.AcquisitionFileAdd);
            var acqFile = EntityHelper.CreateAcquisitionFile();

            var service = helper.GetService<Mock<IAcquisitionFileService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            var result = controller.AddAcquisitionFile(mapper.Map<AcquisitionFileModel>(acqFile));

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<AcquisitionFileModel>(actionResult.Value);
            var expectedResult = mapper.Map<AcquisitionFileModel>(acqFile);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an acquisition file by id.
        /// </summary>
        [Fact]
        public void GetAcquisitionFile_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AcquisitionFileController>(Permissions.AcquisitionFileView);
            var acqFile = EntityHelper.CreateAcquisitionFile();

            var service = helper.GetService<Mock<IAcquisitionFileService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = controller.GetAcquisitionFile(1);

            // Assert
            var actionResult = Assert.IsType<JsonResult>(result);
            var actualResult = Assert.IsType<AcquisitionFileModel>(actionResult.Value);
            var expectedResult = mapper.Map<AcquisitionFileModel>(acqFile);
            expectedResult.Should().BeEquivalentTo(actualResult);
            service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update an acquisition file.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFile_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<AcquisitionFileController>(Permissions.AcquisitionFileEdit);
            var acqFile = EntityHelper.CreateAcquisitionFile();

            var mapper = helper.GetService<IMapper>();

            // TODO: Update test when Update gets implemented
            Action act = () => controller.UpdateAcquisitionFile(1, mapper.Map<AcquisitionFileModel>(acqFile));
            act.Should().Throw<System.NotImplementedException>();
        }
        #endregion
    }
}
