using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using MapsterMapper;
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
            _controller = helper.CreateController<ChecklistController>(Permissions.AcquisitionFileAdd, Permissions.AcquisitionFileView);
            _mapper = helper.GetService<IMapper>();
            _service = helper.GetService<Mock<IAcquisitionFileService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to get an acquisition file checklist.
        /// </summary>
        [Fact]
        public void GetAcquisitionFileChecklist_Success()
        {
            // Arrange
            _service.Setup(m => m.GetChecklistItems(It.IsAny<long>())).Returns(new List<PimsAcquisitionChecklistItem>());

            // Act
            var result = _controller.GetAcquisitionFileChecklist(1);

            // Assert
            _service.Verify(m => m.GetChecklistItems(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get an acquisition file checklist.
        /// </summary>
        [Fact]
        public void UpdateAcquisitionFileChecklist_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateAcquisitionFile();
            _service.Setup(m => m.UpdateChecklistItems(It.IsAny<PimsAcquisitionFile>())).Returns(acqFile);

            // Act
            var result = _controller.UpdateAcquisitionFileChecklist(_mapper.Map<AcquisitionFileModel>(acqFile));

            // Assert
            _service.Verify(m => m.UpdateChecklistItems(It.IsAny<PimsAcquisitionFile>()), Times.Once());
        }



        #endregion
    }
}
