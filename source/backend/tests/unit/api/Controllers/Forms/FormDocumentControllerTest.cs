using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;

using Pims.Api.Constants;
using Pims.Api.Controllers;
using Pims.Api.Helpers.Exceptions;
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
    [Trait("group", "form")]
    [ExcludeFromCodeCoverage]
    public class FormDocumentControllerTest
    {
        #region Variables
        private readonly Mock<IFormDocumentService> _service;
        private readonly FormDocumentController _controller;

        #endregion

        public FormDocumentControllerTest()
        {
            var helper = new TestHelper();
            _controller = helper.CreateController<FormDocumentController>(Permissions.AcquisitionFileView);
            _service = helper.GetService<Mock<IFormDocumentService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to add a form to the datastore.
        /// </summary>
        [Fact]
        public void AddFormDocumentFile_Acquisition_Success()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            _service.Setup(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            var result = _controller.AddFormDocumentFile(FileType.Acquisition, new FormDocumentFileModel()
            {
                FormDocumentType = new FormDocumentTypeModel(),
                FileId = 1
            });

            // Assert
            _service.Verify(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to add a form to the datastore.
        /// </summary>
        [Fact]
        public void AddFormDocumentFile_Acquisition_InvalidFileType()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            _service.Setup(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            Action act = () => _controller.AddFormDocumentFile(FileType.Unknown, new FormDocumentFileModel()
            {
                FormDocumentType = new FormDocumentTypeModel(),
                FileId = 1
            });

            // Assert
            act.Should().Throw<BadRequestException>();
            _service.Verify(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>()), Times.Never());
        }
        #endregion
    }
}
