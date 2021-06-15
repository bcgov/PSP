using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Pims.Core.Comparers;
using Moq;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Microsoft.Extensions.Options;
using Pims.Dal.Entities.Models;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services.Admin;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Xunit;
using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services.Admin
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "project")]
    [ExcludeFromCodeCoverage]
    public class ProjectServiceTest
    {
        #region Data
        public static IEnumerable<object[]> ProjectFilters =>
            new List<object[]>
            {
                new object[] { new ProjectFilter() { ProjectNumber = "ProjectNumber" }, 1 },
                new object[] { new ProjectFilter() { Name = "Name" }, 1 },
                new object[] { new ProjectFilter() { Agencies = new int[] { 3 } }, 1 },
                new object[] { new ProjectFilter() { TierLevelId = 2 }, 1 },
                new object[] { new ProjectFilter() { StatusId = new int[] { 2 } }, 1 }
            };

        public static IEnumerable<object[]> Workflows =>
            new List<object[]>
            {
                new object[] { "SubmitDisposal", 6 },
                new object[] { "ReviewDisposal", 1 }
            };
        #endregion

        #region Constructors
        public ProjectServiceTest() { }
        #endregion

        #region Tests
        #region Get
        [Fact]
        public void Get_ByProjectNumber()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView, Permissions.AdminProjects, Permissions.AgencyAdmin);
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user).AddAndSaveChanges(project);

            var service = helper.CreateService<ProjectService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get($"SPP-{1:00000}");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project, result, new ShallowPropertyCompare());
        }

        [Fact]
        public void Get_ById()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView, Permissions.AdminProjects, Permissions.AgencyAdmin);
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user).AddAndSaveChanges(project);

            var service = helper.CreateService<ProjectService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.Get(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(project, result, new ShallowPropertyCompare());
        }

        [Fact]
        public void Get_Snapshots()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView, Permissions.AdminProjects, Permissions.AgencyAdmin);
            var project = EntityHelper.CreateProject(1);
            var snap = EntityHelper.CreateProjectSnapshot(1, agency: project.Agency);
            snap.ProjectId = project.Id;
            var expectedCount = 1;
            helper.CreatePimsContext(user).AddAndSaveChanges(snap);

            var service = helper.CreateService<ProjectService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            var result = service.GetSnapshots(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedCount, result.Count());
        }

        [Fact]
        public void Get_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user).AddAndSaveChanges(project);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Get($"SPP-{1:00000}"));
        }

        /// <summary>
        /// Project does not exist.
        /// </summary>
        [Fact]
        public void Get_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView, Permissions.AdminProjects, Permissions.AgencyAdmin);
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Get(1));
        }
        #endregion


        #region Add
        [Fact]
        public void Add_Project_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin).AddAgency(1);

            var init = helper.InitializeDatabase(user);
            init.CreateDefaultWorkflowsWithStatus();
            init.SaveChanges();
            var agency = init.Agencies.Find(1);
            var tier = init.TierLevels.Find(1);
            var status = init.Workflows.Find(1).Status.First();
            var project = EntityHelper.CreateProject(1, agency, tier, status);

            var options = Options.Create(new PimsOptions() { Project = new ProjectOptions() { DraftFormat = "TEST-{0:00000}" } });
            var service = helper.CreateService<ProjectService>(user, options);

            // Act
            service.Add(project);
            var result = service.Get(project.Id);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result.ProjectNumber);
            Assert.Matches($"SPP-{1:00000}", result.ProjectNumber);
        }

        [Fact]
        public void Add_Project_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var init = helper.InitializeDatabase(user);
            init.CreateDefaultWorkflowsWithStatus();
            init.SaveChanges();
            var filter = new ProjectFilter();
            var agency = init.Agencies.Find(1);
            var tier = init.TierLevels.Find(1);
            var status = init.Workflows.Find(1).Status.First();
            var project = EntityHelper.CreateProject(1, agency, tier, status);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Add(project));
        }
        #endregion


        #region Update
        [Fact]
        public void Update()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin).AddAgency(1);

            var init = helper.InitializeDatabase(user);
            init.CreateDefaultWorkflowsWithStatus();
            init.SaveChanges();

            var project = init.CreateProject(1);
            init.SaveChanges();

            var options = ControllerHelper.CreateDefaultPimsOptions();
            options.Value.Project.NumberFormat = "TEST-{0:00000}";
            var service = helper.CreateService<ProjectService>(user, options);

            // Act
            var projectToUpdate = EntityHelper.CreateProject(1);
            projectToUpdate.ProjectNumber = project.ProjectNumber;
            projectToUpdate.Description = "A new description";
            service.Update(projectToUpdate);

            // Assert
            project.Description.Should().Be("A new description");
        }

        [Fact]
        public void Update_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user).AddAndSaveChanges(project);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Update(project));
        }

        /// <summary>
        /// Project does not exist.
        /// </summary>
        [Fact]
        public void Update_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView, Permissions.AdminProjects, Permissions.AgencyAdmin);
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(project));
        }
        #endregion


        #region Remove
        [Fact]
        public void Remove_NotAuthorized()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user).AddAndSaveChanges(project);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<NotAuthorizedException>(() =>
                service.Remove(project));
        }

        /// <summary>
        /// Project does not exist.
        /// </summary>
        [Fact]
        public void Remove_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ProjectView, Permissions.AdminProjects, Permissions.AgencyAdmin);
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user);

            var service = helper.CreateService<ProjectService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(project));
        }

        [Fact]
        public void Remove_Project_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var project = EntityHelper.CreateProject(1);
            helper.CreatePimsContext(user).AddAndSaveChanges(project);

            var service = helper.CreateService<ProjectService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(project);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(project).State);
        }
        #endregion
        #endregion
    }
}
