using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using Pims.Core.Comparers;
using Pims.Core.Extensions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;
using Pims.Dal.Security;
using Pims.Dal.Services.Admin;
using Xunit;

using Entity = Pims.Dal.Entities;

namespace Pims.Dal.Test.Services.Admin
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "workflows")]
    [ExcludeFromCodeCoverage]
    public class WorkflowServiceTest
    {
        #region Data
        public static IEnumerable<object[]> Workflows =>
            new List<object[]> {
                new object[] { "SUBMIT", 6 },
                new object[] { "ASSESS", 2 }
            };
        public static IEnumerable<object[]> WorkflowsWithId =>
            new List<object[]> {
                new object[] {  6, 1 },
                new object[] {  2, 2 }
            };
        public static IEnumerable<object[]> WorkflowsOnlyCode =>
            new List<object[]> {
                new object[] { "SUBMIT" },
                new object[] { "ASSESS" }
            };
        #endregion



        #region Constructors
        public WorkflowServiceTest()
        {
        }
        #endregion

        #region Tests
        #region Get
        [Theory]
        [MemberData(nameof(WorkflowsWithId))]
        public void Get(int expectedCount, int id)
        {
            // Arrange
            var helper = new TestHelper();
            var user =
                PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var init = helper.CreatePimsContext(user, true);
            var status = EntityHelper.CreateProjectStatus(1, 7);
            init.AddAndSaveRange (status);
            var submit =
                EntityHelper.CreateWorkflow(1, "Submit", "SUBMIT", status);
            var assess =
                EntityHelper
                    .CreateWorkflow(2, "Assess", "ASSESS", status.Take(2));
            init.AddAndSaveChanges (submit, assess);

            var service = helper.CreateService<WorkflowService>(user);

            // Act
            var result = service.Get(id);

            // Assert
            Assert.NotNull (result);
            Assert
                .IsAssignableFrom
                <IEnumerable<Entity.WorkflowProjectStatus>>(result.Status);
            result.Status.Count().Should().Be(expectedCount);
        }

        [Fact]
        public void GetAll_Workflows()
        {
            // Arrange
            var helper = new TestHelper();
            var user =
                PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var init = helper.CreatePimsContext(user, true);
            var status = EntityHelper.CreateProjectStatus(1, 7);
            init.AddAndSaveRange (status);
            var submit =
                EntityHelper.CreateWorkflow(1, "Submit", "SUBMIT", status);
            var assess =
                EntityHelper
                    .CreateWorkflow(2, "Assess", "ASSESS", status.Take(2));
            var expectedCount = 2;
            init.AddAndSaveChanges (submit, assess);

            var service = helper.CreateService<WorkflowService>(user);

            // Act
            var result = service.GetAll();

            // Assert
            Assert.Equal(expectedCount, result.Count());
        }

        [Theory]
        [MemberData(nameof(WorkflowsOnlyCode))]
        public void Get_Workflow_ByStatus(string code)
        {
            // Arrange
            var helper = new TestHelper();
            var user =
                PrincipalHelper.CreateForPermission(Permissions.ProjectView);

            var init = helper.CreatePimsContext(user, true);
            var status = EntityHelper.CreateProjectStatus(1, 7);
            init.AddAndSaveRange (status);
            var submit =
                EntityHelper.CreateWorkflow(1, "Submit", "SUBMIT", status);
            var assess =
                EntityHelper
                    .CreateWorkflow(2, "Assess", "ASSESS", status.Take(2));
            init.AddAndSaveChanges (submit, assess);

            var service = helper.CreateService<WorkflowService>(user);

            // Act
            var result = service.GetForStatus(code);

            // Assert
            Assert.NotNull (result);
            Assert.IsAssignableFrom<Pims.Dal.Entities.Workflow[]> (result);
        }
        #endregion



        #region Update
        [Fact]
        public void Update_Workflow_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user =
                PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);

            var init = helper.CreatePimsContext(user, true);
            var status = EntityHelper.CreateProjectStatus(1, 7);
            init.AddAndSaveRange (status);
            var workflow =
                EntityHelper.CreateWorkflow(1, "Submit", "SUBMIT", status);
            init.AddAndSaveChanges (workflow);
            var updatedWorkflow =
                EntityHelper.CreateWorkflow(1, "Updated", "UPDATE", status);

            var service = helper.CreateService<WorkflowService>(user);

            // Act
            service.Update (updatedWorkflow);

            // Assert
            workflow.Code.Should().Be("UPDATE");
        }

        [Fact]
        public void Update_Workflow_NullException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.CreatePimsContext(user, true);

            var service = helper.CreateService<WorkflowService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<ArgumentNullException>(() => service.Update(null));
        }

        [Fact]
        public void Update_Workflow_KeyNotFoundException()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            helper.CreatePimsContext(user, true);
            var status = EntityHelper.CreateProjectStatus(1, 7);
            var workflow =
                EntityHelper.CreateWorkflow(1, "Updated", "UPDATE", status);

            var service = helper.CreateService<WorkflowService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            // Assert
            Assert.Throws<KeyNotFoundException>(() => service.Update(workflow));
        }
        #endregion



        #region Remove
        [Fact]
        public void Remove_Workflow_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user =
                PrincipalHelper.CreateForPermission(Permissions.SystemAdmin);
            var status = EntityHelper.CreateProjectStatus(1, 7);
            var workflow =
                EntityHelper.CreateWorkflow(1, "Updated", "UPDATE", status);
            helper.CreatePimsContext(user, true).AddAndSaveChanges(workflow);

            var service = helper.CreateService<WorkflowService>(user);
            var context = helper.GetService<PimsContext>();

            // Act
            service.Remove(workflow);

            // Assert
            Assert.Equal(EntityState.Detached, context.Entry(workflow).State);
        }
        #endregion
        #endregion
    }
}
