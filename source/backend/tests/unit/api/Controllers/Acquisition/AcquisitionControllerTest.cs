using System;
using System.Diagnostics.CodeAnalysis;
using ClosedXML.Excel;
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
        private Mock<IAcquisitionFileService> _service;
        private AcquisitionFileController _controller;
        private IMapper _mapper;
        #endregion

        public AcquisitionControllerTest() {
            var helper = new TestHelper();
            _controller = helper.CreateController<AcquisitionFileController>(Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);
            _mapper = helper.GetService<IMapper>();
            _service = helper.GetService<Mock<IAcquisitionFileService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to add an acquisition file to the datastore.
        /// </summary>
        [Fact]
        public void AddAcquisitionFile_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            _service.Setup(m => m.Add(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            var result = _controller.AddAcquisitionFile(_mapper.Map<AcquisitionFileModel>(acqFile));

            // Assert
            _service.Verify(m => m.Add(It.IsAny<PimsAcquisitionFile>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an acquisition file by id.
        /// </summary>
        [Fact]
        public void GetAcquisitionFile_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();

            _service.Setup(m => m.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = _controller.GetAcquisitionFile(1);

            // Assert
            _service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update an acquisition file.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFile_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            _service.Setup(m => m.Update(It.IsAny<PimsAcquisitionFile>(), It.IsAny<bool>())).Returns(acqFile);

            // Act
            var result = _controller.UpdateAcquisitionFile(1, _mapper.Map<AcquisitionFileModel>(acqFile), true);

            // Assert
            _service.Verify(m => m.Update(It.IsAny<PimsAcquisitionFile>(), It.IsAny<bool>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update an acquisition file's properties.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFileProperties_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();

            _service.Setup(m => m.UpdateProperties(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            var result = _controller.UpdateAcquisitionFileProperties(_mapper.Map<AcquisitionFileModel>(acqFile));

            // Assert
            _service.Verify(m => m.UpdateProperties(It.IsAny<PimsAcquisitionFile>()), Times.Once());
        }
        #endregion
    }
}
