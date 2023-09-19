using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
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
        private Mock<ICompReqFinancialService> _compReqFinancialservice;
        private AcquisitionFileController _controller;
        private IMapper _mapper;
        #endregion

        public AcquisitionControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<AcquisitionFileController>(Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IAcquisitionFileService>>();
            this._compReqFinancialservice = helper.GetService<Mock<ICompReqFinancialService>>();
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
            this._service.Setup(m => m.Add(It.IsAny<PimsAcquisitionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(acqFile);

            // Act
            var result = this._controller.AddAcquisitionFile(this._mapper.Map<AcquisitionFileModel>(acqFile), Array.Empty<string>());

            // Assert
            this._service.Verify(m => m.Add(It.IsAny<PimsAcquisitionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an acquisition file by id.
        /// </summary>
        [Fact]
        public void GetAcquisitionFile_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();

            this._service.Setup(m => m.GetById(It.IsAny<long>())).Returns(acqFile);

            // Act
            var result = this._controller.GetAcquisitionFile(1);

            // Assert
            this._service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update an acquisition file.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFile_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            this._service.Setup(m => m.Update(It.IsAny<PimsAcquisitionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(acqFile);

            // Act
            var result = this._controller.UpdateAcquisitionFile(1, this._mapper.Map<AcquisitionFileModel>(acqFile), Array.Empty<string>());

            // Assert
            this._service.Verify(m => m.Update(It.IsAny<PimsAcquisitionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update an acquisition file's properties.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFileProperties_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();

            this._service.Setup(m => m.UpdateProperties(It.IsAny<PimsAcquisitionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Returns(acqFile);

            // Act
            var result = this._controller.UpdateAcquisitionFileProperties(this._mapper.Map<AcquisitionFileModel>(acqFile), Array.Empty<string>());

            // Assert
            this._service.Verify(m => m.UpdateProperties(It.IsAny<PimsAcquisitionFile>(), It.IsAny<IEnumerable<UserOverrideCode>>()), Times.Once());
        }

        /// <summary>
        /// Get all compensation financials for a file.
        /// </summary>
        [Fact]
        public void GetFileCompReqH120_Success()
        {
            // Arrange
            this._compReqFinancialservice.Setup(m => m.GetAllByAcquisitionFileId(It.IsAny<long>(), It.IsAny<bool>())).Returns(new List<PimsCompReqFinancial>());

            // Act
            var result = this._controller.GetFileCompReqH120(1, false);

            // Assert
            this._compReqFinancialservice.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), false));
        }

        /// <summary>
        /// get all compensation financials for a file that belong to compensation requisitions in the final status.
        /// </summary>
        [Fact]
        public void GetFileCompReqH120_FinalOnly()
        {
            // Arrange
            this._compReqFinancialservice.Setup(m => m.GetAllByAcquisitionFileId(It.IsAny<long>(), It.IsAny<bool>())).Returns(new List<PimsCompReqFinancial>());

            // Act
            var result = this._controller.GetFileCompReqH120(1, true);

            // Assert
            this._compReqFinancialservice.Verify(x => x.GetAllByAcquisitionFileId(It.IsAny<long>(), true));
        }

        #endregion
    }
}
