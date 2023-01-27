using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Areas.Activities.Controllers;
using Pims.Api.Constants;
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
    [Trait("group", "activity")]
    [ExcludeFromCodeCoverage]
    public class ActivityControllerTest
    {
        #region Variables
        private Mock<IActivityService> _service;
        private ActivityController _controller;
        private IMapper _mapper;
        #endregion

        public ActivityControllerTest()
        {
            var helper = new TestHelper();
            _controller = helper.CreateController<ActivityController>(Permissions.ActivityAdd, Permissions.ActivityEdit, Permissions.ActivityView);
            _mapper = helper.GetService<IMapper>();
            _service = helper.GetService<Mock<IActivityService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request to add an activity to the datastore.
        /// </summary>
        [Fact]
        public void AddActivityFile_Research_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.AddResearchActivity(It.IsAny<PimsActivityInstance>(), It.IsAny<long>())).Returns(activity);

            // Act
            var result = _controller.AddActivityFile(FileType.Research, new ActivityInstanceFileModel() { Activity = _mapper.Map<ActivityInstanceModel>(activity), FileId = 1 });

            // Assert
            _service.Verify(m => m.AddResearchActivity(It.IsAny<PimsActivityInstance>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to add an activity to the datastore.
        /// </summary>
        [Fact]
        public void AddActivityFile_Acquisition_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.AddAcquisitionActivity(It.IsAny<PimsActivityInstance>(), It.IsAny<long>())).Returns(activity);

            // Act
            var result = _controller.AddActivityFile(FileType.Acquisition, new ActivityInstanceFileModel() { Activity = _mapper.Map<ActivityInstanceModel>(activity), FileId = 1 });

            // Assert
            _service.Verify(m => m.AddAcquisitionActivity(It.IsAny<PimsActivityInstance>(), It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Test that invalid file types throw the expected error.
        /// </summary>
        [Fact]
        public void AddActivityFile_BadRequest()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.AddAcquisitionActivity(It.IsAny<PimsActivityInstance>(), It.IsAny<long>())).Returns(activity);

            // Act
            Action act = () => _controller.AddActivityFile(FileType.Unknown, new ActivityInstanceFileModel() { Activity = _mapper.Map<ActivityInstanceModel>(activity), FileId = 1 });

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        /// <summary>
        /// Make a successful request to get an activity by id.
        /// </summary>
        [Fact]
        public void GetActivityById_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            var result = _controller.GetActivityById(1);

            // Assert
            _service.Verify(m => m.GetById(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get all file activities by parent file id.
        /// </summary>
        [Fact]
        public void GetFileActivities_Research_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.GetAllByResearchFileId(It.IsAny<long>())).Returns(new List<PimsActivityInstance>() { activity });

            // Act
            var result = _controller.GetFileActivities(FileType.Research, 1);

            // Assert
            _service.Verify(m => m.GetAllByResearchFileId(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get all file activities by parent file id.
        /// </summary>
        [Fact]
        public void GetFileActivities_Acquisition_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsActivityInstance>() { activity });

            // Act
            var result = _controller.GetFileActivities(FileType.Acquisition, 1);

            // Assert
            _service.Verify(m => m.GetAllByAcquisitionFileId(It.IsAny<long>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to get all file activities by parent file id.
        /// </summary>
        [Fact]
        public void GetFileActivities_BadRequest()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.GetAllByAcquisitionFileId(It.IsAny<long>())).Returns(new List<PimsActivityInstance>() { activity });

            // Act
            Action act = () => _controller.GetFileActivities(FileType.Unknown, 1);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        /// <summary>
        /// Make a successful request to get all activity templates.
        /// </summary>
        [Fact]
        public void GetActivityTemplateTypes_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateActivity();

            _service.Setup(m => m.GetAllActivityTemplates()).Returns(new List<PimsActivityTemplate>());

            // Act
            var result = _controller.GetActivityTemplateTypes();

            // Assert
            _service.Verify(m => m.GetAllActivityTemplates(), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update an activity file.
        /// </summary>
        [Fact]
        public void UpdateActivity_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.Update(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            var result = _controller.UpdateActivity(1, _mapper.Map<ActivityInstanceModel>(activity));

            // Assert
            _service.Verify(m => m.Update(It.IsAny<PimsActivityInstance>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update properties on an activity.
        /// </summary>
        [Fact]
        public void UpdateActivityProperty_Research_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.UpdateActivityResearchProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            var result = _controller.UpdateActivityProperties(FileType.Research, _mapper.Map<ActivityInstanceModel>(activity));

            // Assert
            _service.Verify(m => m.UpdateActivityResearchProperties(It.IsAny<PimsActivityInstance>()), Times.Once());
        }

        /// <summary>
        /// Make a successful request to update properties on an activity.
        /// </summary>
        [Fact]
        public void UpdateActivityProperty_Acquisition_Success()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            var result = _controller.UpdateActivityProperties(FileType.Acquisition, _mapper.Map<ActivityInstanceModel>(activity));

            // Assert
            _service.Verify(m => m.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>()), Times.Once());
        }

        /// <summary>
        /// Test that invalid file types throw the expected exception.
        /// </summary>
        [Fact]
        public void UpdateActivityProperty_BadRequest()
        {
            // Arrange
            var activity = EntityHelper.CreateActivity();

            _service.Setup(m => m.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            Action act = () => _controller.UpdateActivityProperties(FileType.Unknown, _mapper.Map<ActivityInstanceModel>(activity));

            act.Should().Throw<BadRequestException>();
        }

        /// <summary>
        /// Make a successful request to delete an activity.
        /// </summary>
        [Fact]
        public async void DeleteActivity_Success()
        {
            // Arrange
            var acqFile = EntityHelper.CreateActivity();

            var deleteResponse = await _controller.DeleteActivity(1);

            var result = Assert.IsType<JsonResult>(deleteResponse);
            result.Value.Should().Be(true);
        }
        #endregion
    }
}
