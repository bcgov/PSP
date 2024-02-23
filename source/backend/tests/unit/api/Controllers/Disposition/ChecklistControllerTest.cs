using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
using Moq;
using Pims.Api.Areas.Disposition.Controllers;
using Pims.Api.Models.Concepts.DispositionFile;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Security;
using Xunit;
using Pims.Api.Models.Concepts.File;
using System;
using Pims.Api.Helpers.Exceptions;
using FluentAssertions;

namespace Pims.Api.Test.Controllers
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "disposition")]
    [ExcludeFromCodeCoverage]
    public class DispositionChecklistControllerTest
    {
        #region Variables
        private Mock<IDispositionFileService> _service;
        private ChecklistController _controller;
        private IMapper _mapper;
        #endregion

        public DispositionChecklistControllerTest()
        {
            var helper = new TestHelper();
            this._controller = helper.CreateController<ChecklistController>(Permissions.DispositionAdd, Permissions.DispositionView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IDispositionFileService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to get an acquisition file checklist.
        /// </summary>
        [Fact]
        public void GetDispositionFileChecklist_Success()
        {
            // Arrange
            this._service.Setup(m => m.GetChecklistItems(It.IsAny<long>())).Returns(new List<PimsDispositionChecklistItem>());

            // Act
            var result = this._controller.GetDispositionFileChecklist(1);

            // Assert
            this._service.Verify(m => m.GetChecklistItems(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an acquisition file checklist.
        /// </summary>
        [Fact]
        public void UpdateDispositionFileChecklist_Success()
        {
            // Arrange
            var checklistItems = new List<FileChecklistItemModel>();
            var dispFile = EntityHelper.CreateDispositionFile();
            this._service.Setup(m => m.UpdateChecklistItems(It.IsAny<IList<PimsDispositionChecklistItem>>())).Returns(dispFile);

            // Act
            var result = this._controller.UpdateDispositionFileChecklist(dispFile.DispositionFileId, checklistItems);

            // Assert
            this._service.Verify(m => m.UpdateChecklistItems(It.IsAny<IList<PimsDispositionChecklistItem>>()), Times.Once());
        }


        /// <summary>
        /// Fails if the checklists items do not belong to the requested acquisition file.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFileChecklist_InvalidIds()
        {
            // Arrange
            var dispFile = EntityHelper.CreateDispositionFile();
            var checklistItems = new List<FileChecklistItemModel>() { new FileChecklistItemModel() { FileId = dispFile.DispositionFileId }, new FileChecklistItemModel() { FileId = dispFile.DispositionFileId + 1 } };
            this._service.Setup(m => m.UpdateChecklistItems(It.IsAny<IList<PimsDispositionChecklistItem>>())).Returns(dispFile);

            // Act
            Action act = () => this._controller.UpdateDispositionFileChecklist(dispFile.DispositionFileId, checklistItems);

            // Assert
            act.Should().Throw<BadRequestException>();
            this._service.Verify(m => m.UpdateChecklistItems(It.IsAny<IList<PimsDispositionChecklistItem>>()), Times.Never());
        }

        #endregion
    }
}
