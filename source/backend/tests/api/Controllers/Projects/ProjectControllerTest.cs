using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Pims.Api.Areas.Projects.Controllers;
using Pims.Api.Models.Concepts.Project;
using Pims.Core.Api.Exceptions;
using Pims.Api.Services;
using Pims.Core.Exceptions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Core.Security;
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
            this._controller = helper.CreateController<ProjectController>(Permissions.ProjectAdd, Permissions.ProjectEdit, Permissions.ProjectView);
            this._mapper = helper.GetService<IMapper>();
            this._service = helper.GetService<Mock<IProjectService>>();
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

            this._service.Setup(m => m.SearchProjects(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(projectList);

            // Act
            var result = this._controller.SearchProjects("test string", 5);

            // Assert
            this._service.Verify(m => m.SearchProjects(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
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
            Action act = () => this._controller.SearchProjects(string.Empty, 7);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        [Fact]
        public void UpdateProject_BadRequest()
        {
            // Act
            var result = this._controller.UpdateProject(1, new ProjectModel { Id = 2 }, Array.Empty<string>());

            // Assert
            result.Should().BeOfType(typeof(BadRequestObjectResult));
        }

        [Fact]
        public void UpdateProject_Conflict()
        {
            var helper = new TestHelper();

            this._service.Setup(x => x.Update(It.IsAny<PimsProject>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Throws(new DuplicateEntityException());

            // Act
            var result = this._controller.UpdateProject(1, new ProjectModel { Id = 1 }, Array.Empty<string>());

            // Assert
            result.Should().BeOfType(typeof(ConflictObjectResult));
        }

        [Fact]
        public void AddProject_Conflict()
        {
            var helper = new TestHelper();

            this._service.Setup(x => x.Add(It.IsAny<PimsProject>(), It.IsAny<IEnumerable<UserOverrideCode>>())).Throws(new DuplicateEntityException());

            // Act
            var result = this._controller.AddProject(new ProjectModel { Id = 2 }, Array.Empty<string>());

            // Assert
            result.Should().BeOfType(typeof(ConflictObjectResult));
        }

        #endregion
    }
}
