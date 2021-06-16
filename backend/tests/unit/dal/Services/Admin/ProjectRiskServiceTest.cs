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
    public class ProjectRiskServiceTest
    {
        #region Tests
        #region Get
        [Fact]
        public void GetAll_ProjectRisks()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);
            var projectRisk = EntityHelper.CreateProjectRisk(100, "Test", "TST", 1);
            init.AddAndSaveChanges(projectRisk);

            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<Entity.ProjectRisk>>(result);
            Assert.NotEmpty(result);
        }

        [Fact]
        public void Get_ProjectRiskById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            using var init = helper.InitializeDatabase(user);
            var projectRisk = EntityHelper.CreateProjectRisk(100, "Test", "TST", 1);
            init.AddAndSaveChanges(projectRisk);

            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            var result = service.Get(100);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test", result.Name);
        }

        [Fact]
        public void Get_ProjectRiskById_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Get(100));
        }
        #endregion

        #region Update
        [Fact]
        public void Update_ProjectRisk_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Update(null));
        }

        [Fact]
        public void Update_ProjectRisk_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var projectRisk = EntityHelper.CreateProjectRisk(100, "Test", "TST", 1);
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Update(projectRisk));
        }

        [Fact]
        public void Update_ProjectRisk_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var projectRisk = EntityHelper.CreateProjectRisk(100, "Test", "TST", 1);

            var updatedProjectRisk = EntityHelper.CreateProjectRisk(100, "Updated", "TST", 1);

            using var init = helper.InitializeDatabase(user);
            init.AddAndSaveChanges(projectRisk);

            var service = helper.CreateService<ProjectRiskService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Update(updatedProjectRisk);

            // Assert
            projectRisk.Name.Should().Be("Updated");

        }
        #endregion

        #region Remove
        [Fact]
        public void Remove_ProjectRisk_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var projectRisk = EntityHelper.CreateProjectRisk(100, "Test", "TST", 1);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(projectRisk);

            var service = helper.CreateService<ProjectRiskService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(projectRisk);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(projectRisk).State);
        }
        [Fact]
        public void Remove_ProjectRisk_NullArguments()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() =>
                service.Remove(null));
        }

        [Fact]
        public void Remove_ProjectRisk_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var projectRisk = EntityHelper.CreateProjectRisk(100, "Test", "TST", 1);
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.CreateService<ProjectRiskService>(user);

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() =>
                service.Remove(projectRisk));
        }
        #endregion
        #endregion
    }
}
