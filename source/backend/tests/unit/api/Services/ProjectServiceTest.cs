using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using Pims.Api.Constants;
using Pims.Api.Models;
using Pims.Api.Models.Concepts;
using Pims.Api.Services;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "project")]
    [ExcludeFromCodeCoverage]
    public class ProjectServiceTest
    {
        private TestHelper _helper;

        public ProjectServiceTest()
        {
            _helper = new TestHelper();
        }

        private ProjectService CreateProjectServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<ProjectService>();
        }

        [Fact]
        public void Search_Success()
        {
            // Arrange
            var service = CreateProjectServiceWithPermissions(Permissions.ProjectView);
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = EntityHelper.CreateProject(1, "7", "Test Project");
            var projectList = new List<PimsProject>() { project };

            var repository = _helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<int>())).Returns(projectList);

            // Act
            var result = service.SearchProjects("query string", 1);

            // Assert
            repository.Verify(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Search_NoPermission()
        {
            // Arrange
            var service = CreateProjectServiceWithPermissions();

            // Act
            Action act = () => service.SearchProjects("some string", 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Search_GetPage_ShouldFail_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action result = () => service.GetPage(new ProjectFilter { ProjectName = "test" });

            // Assert
            result.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>()), Times.Never);
        }

        [Fact]
        public void Search_GetPage_ShouldFail_Filter_IsNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action result = () => service.GetPage(null);

            // Assert
            result.Should().Throw<ArgumentException>();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>()), Times.Never);
        }

        [Fact]
        public void Search_GetPage_ShouldFail_Filter_IsInvalid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action result = () => service.GetPage(new ProjectFilter { Page = 0 });

            // Assert
            result.Should().Throw<ArgumentException>();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>()), Times.Never);
        }

        [Fact]
        public async void Search_GetPage_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.GetPageAsync(It.IsAny<ProjectFilter>()))
                .ReturnsAsync(new Paged<PimsProject>()
                {
                    Page = 1,
                });


            // Act
            var result = await service.GetPage(new ProjectFilter { ProjectName = "test" });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>()), Times.Once);
        }

        [Fact]
        public void Add_Project_ShouldFail_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action result = () => service.Add(new PimsProject());

            // Assert
            result.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Add_Project_ShouldFail_IfNull()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectAdd);
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action result = () => service.Add(null);

            // Assert
            result.Should().Throw<ArgumentException>();
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public async void Add_Project_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectAdd);
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsProject>())).ReturnsAsync(new PimsProject());
            repository.Setup(x => x.Get(It.IsAny<long>())).ReturnsAsync(new PimsProject());

            // Act
            var result = await service.Add(new PimsProject());

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Once);
        }
    }
}
