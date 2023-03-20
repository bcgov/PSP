using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Areas.Activities.Controllers;
using Pims.Api.Areas.Forms.Controllers;
using Pims.Api.Constants;
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
    public class FormControllerTest
    {
        #region Variables
        private Mock<IFormService> _service;
        private FormController _controller;
        private IMapper _mapper;
        #endregion

        public FormControllerTest()
        {
            var helper = new TestHelper();
            _controller = helper.CreateController<FormController>(Permissions.AcquisitionFileView);
            _mapper = helper.GetService<IMapper>();
            _service = helper.GetService<Mock<IFormService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to add an activity to the datastore.
        /// </summary>
        [Fact]
        public void AddFormFile_Acquisition_Success()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            _service.Setup(m => m.AddAcquisitionForm(It.IsAny<LookupModel<string>>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            var result = _controller.AddFormFile(FileType.Acquisition, new FileFormModel() { FormTypeCode = new LookupModel<string>() { Id = "H120" }, FileId = 1 }) ;

            // Assert
            _service.Verify(m => m.AddAcquisitionForm(It.IsAny<LookupModel<string>>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to add an activity to the datastore.
        /// </summary>
        [Fact]
        public void AddFormFile_Acquisition_InvalidFileType()
        {
            // Arrange
            var acquisitionFileForm = new PimsAcquisitionFileForm();

            _service.Setup(m => m.AddAcquisitionForm(It.IsAny<LookupModel<string>>(), It.IsAny<long>())).Returns(acquisitionFileForm);

            // Act
            Action act = () => _controller.AddFormFile(FileType.Unknown, new FileFormModel() { FormTypeCode = new LookupModel<string>() { Id = "H120" }, FileId = 1 });

            // Assert
            act.Should().Throw<BadRequestException>();
            _service.Verify(m => m.AddAcquisitionForm(It.IsAny<LookupModel<string>>(), It.IsAny<long>()), Times.Never());
        }
        #endregion
    }
}
