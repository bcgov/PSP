using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Moq;
using Pims.Api.Services;
using Pims.Core.Api.Exceptions;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "project")]
    [ExcludeFromCodeCoverage]
    public class ProjectServiceTest
    {
        readonly TestHelper _helper;

        public ProjectServiceTest()
        {
            this._helper = new TestHelper();
        }

        private ProjectService CreateProjectServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            this._helper.CreatePimsContext(user, true);
            return this._helper.Create<ProjectService>();
        }

        [Fact]
        public void Search_Success()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectView);
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = EntityHelper.CreateProject(1, "7", "Test Project");
            var projectList = new List<PimsProject>() { project };

            var repository = this._helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<HashSet<short>>(), It.IsAny<int>())).Returns(projectList);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.SearchProjects("query string", 1);

            // Assert
            repository.Verify(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<HashSet<short>>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void Search_NoPermission()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();

            // Act
            Action act = () => service.SearchProjects("some string", 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Search_GetPage_ShouldFail_NotAuthorized()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();

            // Act
            Action act = () => service.GetPage(new ProjectFilter { ProjectName = "test" });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Search_GetPage_ShouldFail_Filter_IsNull()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectView);

            var repository = this._helper.GetService<Mock<IProjectRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            Action act = () => service.GetPage(null);

            // Assert
            act.Should().Throw<ArgumentException>();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>(), It.IsAny<IEnumerable<short>>()), Times.Never);
        }

        [Fact]
        public void Search_GetPage_ShouldFail_Filter_IsInvalid()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectView);

            var repository = this._helper.GetService<Mock<IProjectRepository>>();
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            Action act = () => service.GetPage(new ProjectFilter { Page = 0 });

            // Assert
            act.Should().Throw<ArgumentException>();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>(), It.IsAny<IEnumerable<short>>()), Times.Never);
        }

        [Fact]
        public async void Search_GetPage_Success()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectView);

            var repository = this._helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.GetPageAsync(It.IsAny<ProjectFilter>(), It.IsAny<IEnumerable<short>>()))
                .ReturnsAsync(new Paged<PimsProject>()
                {
                    Page = 1,
                });
            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(new PimsUser());

            // Act
            var result = await service.GetPage(new ProjectFilter { ProjectName = "test" });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.GetPageAsync(It.IsAny<ProjectFilter>(), It.IsAny<IEnumerable<short>>()), Times.Once);
        }

        [Fact]
        public void GetAll_Success()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectView);
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = EntityHelper.CreateProject(1, "7", "Test Project");
            var projectList = new List<PimsProject>() { project };

            var repository = this._helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<HashSet<short>>(), It.IsAny<int>())).Returns(projectList);

            var userRepository = this._helper.GetService<Mock<IUserRepository>>();
            userRepository.Setup(x => x.GetUserInfoByKeycloakUserId(It.IsAny<Guid>())).Returns(EntityHelper.CreateUser("Test"));

            // Act
            var result = service.GetAll();

            // Assert
            repository.Verify(x => x.SearchProjects(It.IsAny<string>(), It.IsAny<HashSet<short>>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public void GetAll_NoPermission()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();

            // Act
            Action act = () => service.GetAll();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void Add_Project_ShouldFail_NotAuthorized()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();

            var repository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.Add(new PimsProject(), new List<UserOverrideCode>() { });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Add_Project_ShouldFail_IfNull()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectAdd);

            var repository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.Add(null, new List<UserOverrideCode>() { });

            // Assert
            act.Should().Throw<ArgumentException>();
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Add_Project_ShouldFail_IfDuplicateProject()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectAdd, Permissions.ProjectView);

            var duplicateProject = new PimsProject() { Code = "1" };

            var projectRepo = _helper.GetService<Mock<IProjectRepository>>();
            projectRepo.Setup(x => x.GetAllByName(It.IsAny<string>())).Returns(new List<PimsProject>() { duplicateProject });

            // Act
            Action act = () => service.Add(new PimsProject() { Code = "1" }, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BusinessRuleViolationException>();
            projectRepo.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Add_Project_ShouldFail_IfDuplicateProduct()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectAdd, Permissions.ProjectView);

            var duplicateProduct = new PimsProduct() { Code = "1" };

            var existingProjectProducts = new List<PimsProjectProduct>()
                {
                    new PimsProjectProduct() {
                        Product = duplicateProduct
                    }
                };

            var productRepo = _helper.GetService<Mock<IProductRepository>>();
            productRepo.Setup(x => x.GetProjectProductsByProject(It.IsAny<long>())).Returns(new List<PimsProjectProduct>());
            productRepo.Setup(x => x.GetProducts(It.IsAny<IEnumerable<PimsProduct>>())).Returns(new List<PimsProduct> { duplicateProduct });


            var projectRepo = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.Add(new PimsProject() { PimsProjectProducts = existingProjectProducts }, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<UserOverrideException>();
            projectRepo.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Add_Project_ShouldFail_IfDuplicateTeamMembers()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectAdd, Permissions.ProjectView);

            var repository = _helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.GetAllByName(It.IsAny<string>())).Returns(new List<PimsProject>());

            var project = EntityHelper.CreateProject(1, "007", "Test Project");
            project.PimsProjectPeople = new List<PimsProjectPerson>() { new PimsProjectPerson() { PersonId = 1 }, new PimsProjectPerson() { PersonId = 1 } };

            // Act
            Action act = () => service.Add(project, new List<UserOverrideCode>());

            // Assert
            act.Should().Throw<BadRequestException>().WithMessage("Invalid Project management team, each team member can only be added once.");
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Add_Project_Success()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectAdd, Permissions.ProjectView);

            var repository = _helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsProject>())).Returns(new PimsProject());
            repository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(new PimsProject());

            // Act
            var result = service.Add(new PimsProject(), new List<UserOverrideCode>() { });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Add(It.IsAny<PimsProject>()), Times.Once);
        }

        [Fact]
        public void Get_ProjectById_ShouldFail_NotAuthorized()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();
            var repository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.TryGet(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Get_ProjectById_Success()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectView);

            var repository = _helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(new PimsProject());

            // Act
            var result = service.GetById(1);

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.TryGet(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void Get_Products_ShouldFail_NotAuthorized()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();
            var repository = _helper.GetService<Mock<IProductRepository>>();

            // Act
            Action act = () => service.GetProducts(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetByProject(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Get_ProductFiles_ShouldFail_NotAuthorized()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();
            var repository = _helper.GetService<Mock<IAcquisitionFileRepository>>();

            // Act
            Action act = () => service.GetProductFiles(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetByProductId(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public void Update_Project_ShouldFail_NotAuthorized()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions();
            var repository = _helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.Update(new PimsProject(), new List<UserOverrideCode>() { });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Update_Project_ShouldFail_When_Null()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectEdit);
            var repository = this._helper.GetService<Mock<IProjectRepository>>();

            // Act
            Action act = () => service.Update(null, new List<UserOverrideCode>() { });

            // Assert
            act.Should().Throw<ArgumentNullException>();
            repository.Verify(x => x.Update(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Update_Project_ShouldFail_IfDuplicateTeamMembers()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectEdit);
            var repository = _helper.GetService<Mock<IProjectRepository>>();

            var project = EntityHelper.CreateProject(1, "007", "Test Project");
            project.PimsProjectPeople = new List<PimsProjectPerson>() { new PimsProjectPerson() { PersonId = 1 }, new PimsProjectPerson() { PersonId = 1 } };

            // Act
            Action result = () => service.Update(project, new List<UserOverrideCode>());

            // Assert
            result.Should().Throw<BadRequestException>().WithMessage("Invalid Project management team, each team member can only be added once.");
            repository.Verify(x => x.Update(It.IsAny<PimsProject>()), Times.Never);
        }

        [Fact]
        public void Update_Project_Success()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectEdit);
            var repository = this._helper.GetService<Mock<IProjectRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsProject>())).Returns(new PimsProject { Internal_Id = 1 });
            repository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(new PimsProject()
            {
                ProjectStatusTypeCode = null,
                ProjectStatusTypeCodeNavigation = null,
            });

            // Act
            var result = service.Update(new PimsProject { Id = 1, ConcurrencyControlNumber = 100 }, new List<UserOverrideCode>() { });

            // Assert
            result.Should().NotBeNull();
            repository.Verify(x => x.Update(It.IsAny<PimsProject>()), Times.Once);
        }

        [Fact]
        public void Update_Project_Success_AddsNote()
        {
            // Arrange
            var service = this.CreateProjectServiceWithPermissions(Permissions.ProjectEdit);

            var project = EntityHelper.CreateProject(1, "9999", "TEST PROJECT");
            project.ConcurrencyControlNumber = 1;
            project.AppCreateUserid = "TESTER";

            var projectRepository = this._helper.GetService<Mock<IProjectRepository>>();
            var noteRepository = this._helper.GetService<Mock<IEntityNoteRepository>>();
            var lookupRepository = this._helper.GetService<Mock<ILookupRepository>>();

            projectRepository.Setup(x => x.Update(It.IsAny<PimsProject>())).Returns(project);
            projectRepository.Setup(x => x.TryGet(It.IsAny<long>())).Returns(new PimsProject()
            {
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Description = "Active" },
            });
            lookupRepository.Setup(x => x.GetAllProjectStatusTypes()).Returns(new PimsProjectStatusType[]{ new PimsProjectStatusType() {
                Id = project.ProjectStatusTypeCodeNavigation.Id,
                Description = project.ProjectStatusTypeCodeNavigation.Description,
            },});

            // Act
            var result = service.Update(project, new List<UserOverrideCode>() { });

            // Assert
            projectRepository.Verify(x => x.Update(It.IsAny<PimsProject>()), Times.Once);
            noteRepository.Verify(x => x.Add(It.Is<PimsProjectNote>(x => x.ProjectId == 1
                    && x.Note.NoteTxt == "Project status changed from Active to 'No Status'")), Times.Once);
        }
    }
}
