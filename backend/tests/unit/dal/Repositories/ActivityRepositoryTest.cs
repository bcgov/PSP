using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Dal.Test.Repositories
{
    [Trait("category", "unit")]
    [Trait("category", "dal")]
    [Trait("group", "activity")]
    [ExcludeFromCodeCoverage]
    public class ActivityRepositoryTest
    {
        #region Constructors
        public ActivityRepositoryTest() { }
        #endregion

        #region Tests

        #region Add Note
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ActivityRepository>(user);

            var template = EntityHelper.CreateActivityTemplate(1);
            var activity = EntityHelper.CreateActivity(1, template: template);

            // Act
            var result = repository.Add(activity);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsActivityInstance>();
            result.ActivityTemplate.Should().Be(template);
            result.ActivityInstanceId.Should().Be(1);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            Action act = () => repository.Add(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Get Activity By Id
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityView);

            var template = EntityHelper.CreateActivityTemplate(1);
            var activity = EntityHelper.CreateActivity(id: 1, template: template);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);
            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsActivityInstance>();
            result.ActivityTemplateId.Should().Be(template.ActivityTemplateId);
            result.ActivityInstanceId.Should().Be(activity.ActivityInstanceId);
        }

        [Fact]
        public void GetById_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityEdit);

            var activity = EntityHelper.CreateActivity(1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            activity.ActivityInstanceId = 1;
            activity.ActivityDataJson = "test";

            var result = repository.Update(activity);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsActivityInstance>();
            result.ActivityDataJson.Should().Be("test");
            result.ActivityInstanceId.Should().Be(activity.ActivityInstanceId);
        }

        [Fact]
        public void Update_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            Action act = () => repository.Update(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Update_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<ActivityRepository>(user);
            var activity = EntityHelper.CreateActivity(id: 1);

            // Act
            Action act = () => repository.Update(activity);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var activity = EntityHelper.CreateActivity(1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            var result = repository.Delete(activity.ActivityInstanceId);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void Delete_Relationships_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var activity = EntityHelper.CreateActivity(1);
            activity.PimsAcquisitionActivityInstances = new List<PimsAcquisitionActivityInstance>() { new PimsAcquisitionActivityInstance() };
            activity.PimsResearchActivityInstances = new List<PimsResearchActivityInstance>() { new PimsResearchActivityInstance() };
            activity.PimsActInstPropAcqFiles = new List<PimsActInstPropAcqFile>() { new PimsActInstPropAcqFile() };
            activity.PimsActInstPropRsrchFiles = new List<PimsActInstPropRsrchFile>() { new PimsActInstPropRsrchFile() };
            activity.PimsActivityInstanceDocuments = new List<PimsActivityInstanceDocument>() { new PimsActivityInstanceDocument() };
            activity.PimsActivityInstanceNotes = new List<PimsActivityInstanceNote>() { new PimsActivityInstanceNote() };
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            var result = repository.Delete(activity.ActivityInstanceId);

            // Assert
            result.Should().Be(true);
        }

        [Fact]
        public void Delete_KeyNotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var activity = EntityHelper.CreateActivity(1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<ActivityRepository>(user);

            // Act
            Action act = () => repository.Delete(0);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #endregion
    }
}
