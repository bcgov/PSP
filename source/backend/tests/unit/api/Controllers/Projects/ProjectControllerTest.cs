using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Acquisition.Controllers;
using Pims.Api.Areas.Activities.Controllers;
using Pims.Api.Areas.Projects.Controllers;
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
    [Trait("group", "project")]
    [ExcludeFromCodeCoverage]
    public class ProjectControllerTest
    {
        #region Variables
        private Mock<IProjectService> _service;
        private ProjectController _controller;
        private IMapper _mapper;
        #endregion

        public ProjectControllerTest()
        {
            var helper = new TestHelper();
            _controller = helper.CreateController< ProjectController>(Permissions.ProjectAdd, Permissions.ProjectEdit, Permissions.ProjectView);
            _mapper = helper.GetService<IMapper>();
            _service = helper.GetService<Mock<IProjectService>>();
        }

        #region Tests
        /// <summary>
        /// Make a successful request search a project.
        /// </summary>
        [Fact]
        public void SearchProjects_Success()
        {
            // Arrange
            var project = EntityHelper.CreateProject(1, "7", "Test Project");
            var projectList = new List<PimsProject>() { project };

            _service.Setup(m => m.SearchProjects(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(projectList);

            // Act
            var result = _controller.SearchProjects("test string", 5);

            // Assert
            _service.Verify(m => m.SearchProjects(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Test that the search parameter is necessary.
        /// </summary>
        [Fact]
        public void SearchProjects_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();

            // Act
            Action act = () => _controller.SearchProjects("", 7);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void UpdateProject_BadRequest()
        {
            // Act
            var result =  _controller.UpdateProject(1, new ProjectModel { Id=2 });

            // Assert
            result.Should().BeOfType(typeof(BadRequestResult));
        }

        #endregion
    }
}
