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
        [Fact]
        public void Search_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);
            var service = helper.Create<ProjectService>(user);

            var project = EntityHelper.CreateProject(1, 7, "Test Project");
            var projectList = new List<PimsProject>() { project };

            var repository = helper.GetService<Mock<IProjectRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ProjectService>(user);

            var repository = helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.SearchProjects("some string", 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<int>()), Times.Never);
        }
    }
}
