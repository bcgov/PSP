using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Core.Security;
using Xunit;
using Pims.Api.Models.Concepts.File;
using System;
using Pims.Core.Api.Exceptions;
using FluentAssertions;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "acquisition")]
    [ExcludeFromCodeCoverage]
    public class ChecklistControllerTest
    {
        #region Variables
        private Mock<IAcquisitionFileService> _service;
        private ChecklistController _controller;
        private IMapper _mapper;
        #endregion

        public ChecklistControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<ChecklistController>(Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IAcquisitionFileService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to get an acquisition file checklist.
        /// </summary>
        [Fact]
        public void GetAcquisitionFileChecklist_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetChecklistItems(It.IsAny<long>())).Returns(new List<PimsAcquisitionChecklistItem>());

            // Act
            var result = this._controller.GetAcquisitionFileChecklist(1);

            // Assert
            this._service.Verify(m => m.GetChecklistItems(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an acquisition file checklist.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFileChecklist_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            var checklistItems = new List<FileChecklistItemModel>() { new FileChecklistItemModel() { FileId = acqFile.AcquisitionFileId }, new FileChecklistItemModel() { FileId = acqFile.AcquisitionFileId } };
            this._service.Setup(m => m.UpdateChecklistItems(It.IsAny<IList<PimsAcquisitionChecklistItem>>())).Returns(acqFile);

            // Act
            var result = this._controller.UpdateAcquisitionFileChecklist(acqFile.AcquisitionFileId, checklistItems);

            // Assert
            this._service.Verify(m => m.UpdateChecklistItems(It.IsAny<IList<PimsAcquisitionChecklistItem>>()), Times.Once());
        }

        /// <summary>
        /// Fails if the checklists items do not belong to the requested acquisition file.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFileChecklist_InvalidIds()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            var checklistItems = new List<FileChecklistItemModel>() { new FileChecklistItemModel() { FileId = acqFile.AcquisitionFileId }, new FileChecklistItemModel() { FileId = acqFile.AcquisitionFileId + 1 } };
            this._service.Setup(m => m.UpdateChecklistItems(It.IsAny<IList<PimsAcquisitionChecklistItem>>())).Returns(acqFile);

            // Act
            Action act = () => this._controller.UpdateAcquisitionFileChecklist(acqFile.AcquisitionFileId, checklistItems);

            // Assert
            act.Should().Throw<BadRequestException>();
            this._service.Verify(m => m.UpdateChecklistItems(It.IsAny<IList<PimsAcquisitionChecklistItem>>()), Times.Never());
        }

        #endregion
    }
}
