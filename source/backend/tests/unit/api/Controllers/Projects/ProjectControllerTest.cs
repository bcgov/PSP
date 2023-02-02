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

        #endregion

        #region Tests
        /// <summary>
        /// Make a successful request search a project.
        /// </summary>
        [Fact]
        public void SearchProjects_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ProjectController>(Permissions.AcquisitionFileAdd); // TODO: set permission correctly
            var project = EntityHelper.CreateProject(1, 7, "Test Project");
            var projectList = new List<PimsProject>() { project };

            var service = helper.GetService<Mock<IProjectService>>();
            var mapper = helper.GetService<IMapper>();

            service.Setup(m => m.SearchProjects(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(projectList);

            // Act
            var result = controller.SearchProjects("test string", 5);

            // Assert
            service.Verify(m => m.SearchProjects(It.IsAny<string>(), It.IsAny<int>()), Times.Once());
        }

        /// <summary>
        /// Test that the search parameter is necessary.
        /// </summary>
        [Fact]
        public void SearchProjects_BadRequest()
        {
            // Arrange
            var helper = new TestHelper();
            var controller = helper.CreateController<ProjectController>(Permissions.ActivityEdit);

            // Act
            Action act = () => controller.SearchProjects("", 7);

            // Assert
            act.Should().Throw<BadRequestException>();
        }

        #endregion
    }
}
