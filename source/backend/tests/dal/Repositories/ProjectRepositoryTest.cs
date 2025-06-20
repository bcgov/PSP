using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.CodeAnalysis;
using Pims.Core.Exceptions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "activity")]
    [ExcludeFromCodeCoverage]
    public class ProjectRepositoryTest
    {
        #region Constructors
        public ProjectRepositoryTest() { }
        #endregion

        #region Tests
        [Fact]
        public void SearchProjects_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var project = new PimsProject() { Description = "test project", RegionCode = 1, ProjectStatusTypeCode = "ACTIVE" };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            var result = repository.SearchProjects("test project", new HashSet<short>() { 1 }, 1);

            // Assert
            result.Should().NotBeNull();
            result.Count.Should().Be(1);
        }

        [Fact]
        public void SearchProjects_FilterRegion_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var project = new PimsProject() { Description = "test project", RegionCode = 1, ProjectStatusTypeCode = "ACTIVE" };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            var result = repository.SearchProjects("test project", new HashSet<short>() { 2 }, 1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        #region GetPageAsync

        [Fact]
        public async void GetPageAsync_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            var result = await repository.GetPageAsync(new Dal.Entities.Models.ProjectFilter() { ProjectName = "test project", ProjectNumber = "551234", ProjectStatusCode = "ACTIVE", ProjectRegionCode = "1", Sort = new string[] { "LastUpdatedBy" } }, new HashSet<short>() { 1 });

            // Assert
            result.Should().NotBeNull();
            result.Items.Count.Should().Be(1);
        }

        [Fact]
        public void GetPageAsync_Error_Permissions()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var project = new PimsProject() { Description = "test project", RegionCode = 1, ProjectStatusTypeCode = "ACTIVE" };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            Action act = () => repository.GetPageAsync(new Dal.Entities.Models.ProjectFilter() { ProjectName = "test project" }, new HashSet<short>() { 1 });

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        [Fact]
        public void GetPageAsync_Error_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = new PimsProject() { Description = "test project", RegionCode = 1, ProjectStatusTypeCode = "ACTIVE" };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            Action act = () => repository.GetPageAsync(null, new HashSet<short>() { 1 });

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void GetPageAsync_Error_Invalid()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = new PimsProject() { Description = "test project", RegionCode = 1, ProjectStatusTypeCode = "ACTIVE" };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            Action act = () => repository.GetPageAsync(new Dal.Entities.Models.ProjectFilter() { Page = -1 }, new HashSet<short>() { 1 });

            // Assert
            act.Should().Throw<ArgumentException>();
        }

        #endregion

        #region TryGet

        [Fact]
        public void TryGet_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            var result = repository.TryGet(1);

            // Assert
            result.Should().NotBeNull();
        }

        public void TryGet_Permissions()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            Action act = () => repository.TryGet(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }

        #endregion

        #region GetAllByName

        [Fact]
        public void GetAllByName_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            var result = repository.GetAllByName("test project");

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(1);
        }

        public void GetAllByName_Permissions()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            Action act = () => repository.GetAllByName("test project");

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" },
            };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            repository.Add(project);
            context.CommitTransaction();

            // Assert
            context.PimsProjects.Should().HaveCount(1);
        }

        [Fact]
        public void Add_Project_Product_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" },
                PimsProjectProducts = new List<PimsProjectProduct>() { new PimsProjectProduct() { Product = new PimsProduct() { Code = "PRODUCT", Description = "Product" } } }
            };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            repository.Add(project);
            context.CommitTransaction();

            // Assert
            context.PimsProjects.Should().HaveCount(1);
            context.PimsProjectProducts.Should().HaveCount(1);
        }

        [Fact]
        public void Add_Project_ProductUpdate_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" },
                PimsProjectProducts = new List<PimsProjectProduct>() { new PimsProjectProduct() { ProductId = 1, Product = new PimsProduct() { Code = "PRODUCT", Description = "Product" } } }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project.PimsProjectProducts.FirstOrDefault());
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            repository.Add(project);
            context.CommitTransaction();

            // Assert
            context.PimsProjects.Should().HaveCount(1);
            context.PimsProjectProducts.Should().HaveCount(1);
        }
        #endregion


        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" },
                PimsProjectProducts = new List<PimsProjectProduct>() { new PimsProjectProduct() { Product = new PimsProduct() { Code = "PRODUCT", Description = "Product" } } }
            };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            project.Description = "updated";
            // Act
            var response = repository.Update(project);
            context.CommitTransaction();

            // Assert
            response.Description.Should().Be("updated");
        }

        [Fact]
        public void Update_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var project = new PimsProject()
            {
                Description = "test project",
                RegionCode = 1,
                Code = "551234",
                ProjectStatusTypeCode = "ACTIVE",
                ProjectStatusTypeCodeNavigation = new PimsProjectStatusType() { Id = "ACTIVE", Description = "Active", DbLastUpdateUserid = "test", DbCreateUserid = "test" },
                RegionCodeNavigation = new PimsRegion() { Id = 1, DbCreateUserid = "test", DbLastUpdateUserid = "test", RegionName = "test region" },
                PimsProjectProducts = new List<PimsProjectProduct>() { new PimsProjectProduct() { Product = new PimsProduct() { Code = "PRODUCT", Description = "Product" } } }
            };

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ProjectRepository>(user);

            // Act
            Action act = () => repository.Update(project);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }

        #endregion

        #endregion
    }
}
