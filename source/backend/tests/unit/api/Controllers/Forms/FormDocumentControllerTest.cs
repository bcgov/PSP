using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;

using Pims.Api.Constants;
using Pims.Api.Controllers;
using Pims.Api.Helpers.Exceptions;
using Pims.Api.Models.Concepts;
using Pims.Api.Models.Lookup;
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
            this._controller = helper.CreateController<FormDocumentController>(Permissions.AcquisitionFileView);
            this._service = helper.GetService<Mock<IFormDocumentService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to add a form to the datastore.
        /// </summary>
        [Fact]
        public void AddFormFile_Acquisition_Success()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            var result = this._controller.AddFormDocumentFile(FileType.Acquisition, new FormDocumentFileModel() { FormDocumentType = new FormDocumentTypeModel() { FormTypeCode = "H120" }, FileId = 1 });

            // Assert
            this._service.Verify(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make an invalid request using a non-existent file type.
        /// </summary>
        [Fact]
        public void AddFormFile_Acquisition_InvalidFileType()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            Action act = () => this._controller.AddFormDocumentFile(FileType.Unknown, new FormDocumentFileModel() { FormDocumentType = new FormDocumentTypeModel() { FormTypeCode = "H120" }, FileId = 1 });

            // Assert
            act.Should().Throw<BadRequestException>();
            this._service.Verify(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>()), Times.Never());
        }

        /// <summary>
        /// Make a successful request to get a form from the datastore.
        /// </summary>
        [Fact]
        public void GetFormFile_Acquisition_Success()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.GetAcquisitionForm(It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            var result = this._controller.GetFileForm(FileType.Acquisition, 1);

            // Assert
            this._service.Verify(m => m.GetAcquisitionForm(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make an invalid request using a non-existent file type.
        /// </summary>
        [Fact]
        public void GetFormFile_Acquisition_InvalidFileType()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.GetAcquisitionForm(It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            Action act = () => this._controller.GetFileForm(FileType.Unknown, 1);

            // Assert
            act.Should().Throw<BadRequestException>();
            this._service.Verify(m => m.GetAcquisitionForm(It.IsAny<long>()), Times.Never());
        }

        /// <summary>
        /// Make a successful request to get multiple forms from the datasource.
        /// </summary>
        [Fact]
        public void GetFormFiles_Acquisition_Success()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.GetAcquisitionForms(It.IsAny<long>())).Returns(new List<PimsAcquisitionFileForm>() { acquisitionFileForm });

            // Act
            var result = this._controller.GetFileForms(FileType.Acquisition, 1);

            // Assert
            this._service.Verify(m => m.GetAcquisitionForms(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make an invalid request using a non-existent file type.
        /// </summary>
        [Fact]
        public void GetFormFiles_Acquisition_InvalidFileType()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            Action act = () => this._controller.AddFormDocumentFile(FileType.Unknown, new FormDocumentFileModel() { FormDocumentType = new FormDocumentTypeModel() { FormTypeCode = "H120" }, FileId = 1 });

            // Assert
            act.Should().Throw<BadRequestException>();
            this._service.Verify(m => m.AddAcquisitionForm(It.IsAny<PimsFormType>(), It.IsAny<long>()), Times.Never());
        }

        /// <summary>
        /// Make a successful request to delete a form from the datasource.
        /// </summary>
        [Fact]
        public void DeleteFormFile_Acquisition_Success()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.DeleteAcquisitionFileForm(It.IsAny<long>())).Returns(true);

            // Act
            var result = this._controller.DeleteFileForm(FileType.Acquisition, 1);

            // Assert
            this._service.Verify(m => m.DeleteAcquisitionFileForm(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make an invalid request using a non-existent file type.
        /// </summary>
        [Fact]
        public void DeleteFormFile_Acquisition_InvalidFileType()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            this._service.Setup(m => m.DeleteAcquisitionFileForm(It.IsAny<long>())).Returns(true);

            // Act
            Action act = () => this._controller.DeleteFileForm(FileType.Unknown, 1);

            // Assert
            act.Should().Throw<BadRequestException>();
            this._service.Verify(m => m.DeleteAcquisitionFileForm(It.IsAny<long>()), Times.Never());
        }
        #endregion
    }
}
