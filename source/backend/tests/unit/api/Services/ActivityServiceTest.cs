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
using Pims.Dal;
using Pims.Dal.Entities;
using Pims.Dal.Exceptions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;
using Xunit;

namespace Pims.Api.Test.Services
{
    [Trait("category", "unit")]
    [Trait("category", "api")]
    [Trait("group", "activity")]
    [ExcludeFromCodeCoverage]
    public class ActivityServiceTest
    {
        private TestHelper _helper;

        public ActivityServiceTest()
        {
            _helper = new TestHelper();
        }

        private ActivityService CreateActivityServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            var service = _helper.Create<ActivityService>(user);
            return service;
        }

        #region Tests
        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityAdd);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            var result = service.Add(activity);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstance>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();

            // Act
            Action act = () => service.Add(activity);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstance>()), Times.Never);
        }

        [Fact]
        public void AddResearchActivity_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityAdd, Permissions.ResearchFileEdit);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            var result = service.AddResearchActivity(activity, 1);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstance>()), Times.Once);
        }

        [Fact]
        public void AddResearchActivity_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();

            // Act
            Action act = () => service.AddResearchActivity(activity, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstance>()), Times.Never);
        }

        [Fact]
        public void AddAcquisitionActivity_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityAdd, Permissions.AcquisitionFileEdit);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstance>())).Returns(activity);

            // Act
            var result = service.AddAcquisitionActivity(activity, 1);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstance>()), Times.Once);
        }

        [Fact]
        public void AddAcquisitionActivity_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();

            // Act
            Action act = () => service.AddAcquisitionActivity(activity, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstance>()), Times.Never);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityView);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetAllByResearchFileId
        [Fact]
        public void GetAllByResearchFileId_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityView, Permissions.ResearchFileView);

            var activities = new[] { EntityHelper.CreateActivity(), EntityHelper.CreateActivity() };

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>())).Returns(activities);

            // Act
            var result = service.GetAllByResearchFileId(1);

            // Assert
            repository.Verify(x => x.GetAllByResearchFileId(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetAllByResearchFileId_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            // Act
            Action act = () => service.GetAllByResearchFileId(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetAllActivityTemplates
        [Fact]
        public void GetAllActivityTemplates_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityView);

            var activities = new[] { EntityHelper.CreateActivityTemplate(), EntityHelper.CreateActivityTemplate() };

            var repository = _helper.GetService<Mock<IActivityTemplateRepository>>();
            repository.Setup(x => x.GetAllActivityTemplates()).Returns(activities);

            // Act
            var result = service.GetAllActivityTemplates();

            // Assert
            repository.Verify(x => x.GetAllActivityTemplates(), Times.Once);
        }

        [Fact]
        public void GetAllActivityTemplates_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            // Act
            Action act = () => service.GetAllActivityTemplates();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityEdit, Permissions.ActivityView);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            var result = service.Update(activity);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsActivityInstance>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            Action act = () => service.Update(activity);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsActivityInstance>()), Times.Never);
        }

        [Fact]
        public void Update_Concurrency()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityEdit, Permissions.ActivityView);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(2);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            Action act = () => service.Update(activity);

            // Assert
            act.Should().Throw<DbUpdateConcurrencyException>();
            repository.Verify(x => x.Update(It.IsAny<PimsActivityInstance>()), Times.Never);
        }

        [Fact]
        public void UpdateActivityResearchProperties_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityEdit, Permissions.ActivityView, Permissions.PropertyEdit);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.UpdateActivityResearchProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            var result = service.UpdateActivityResearchProperties(activity);

            // Assert
            repository.Verify(x => x.UpdateActivityResearchProperties(It.IsAny<PimsActivityInstance>()), Times.Once);
        }

        [Fact]
        public void UpdateActivityResearchProperties_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();

            // Act
            Action act = () => service.UpdateActivityResearchProperties(activity);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsActivityInstance>()), Times.Never);
        }

        [Fact]
        public void UpdateActivityAcquisitionProperties_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityEdit, Permissions.ActivityView, Permissions.PropertyEdit);

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            var result = service.UpdateActivityAcquisitionProperties(activity);

            // Assert
            repository.Verify(x => x.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>()), Times.Once);
        }

        [Fact]
        public void UpdateActivityAcquisitionProperties_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var activity = EntityHelper.CreateActivity();

            var repository = _helper.GetService<Mock<IActivityRepository>>();

            // Act
            Action act = () => service.UpdateActivityAcquisitionProperties(activity);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsActivityInstance>()), Times.Never);
        }
        #endregion

        #region DeleteActivity
        [Fact]
        public async void DeleteActivity_Success()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityDelete);
            var activityNotes = new List<PimsActivityInstanceNote>() { EntityHelper.CreateActivityNote() };
            var activityDocuments = new List<PimsActivityInstanceDocument>() { EntityHelper.CreateActivityDocument() };

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            var noteService = _helper.GetService<Mock<INoteService>>();
            var documentService = _helper.GetService<Mock<IDocumentActivityService>>();
            documentService.Setup(x => x.GetActivityDocuments(It.IsAny<long>())).Returns(activityDocuments);
            documentService.Setup(x => x.DeleteActivityDocumentAsync(It.IsAny<PimsActivityInstanceDocument>())).ReturnsAsync(new ExternalResult<string>());
            noteService.Setup(x => x.GetNotes(NoteType.Activity, It.IsAny<long>())).Returns(activityNotes.Select(n => n.Note));
            repository.Setup(x => x.Delete(It.IsAny<long>()));

            // Act
            await service.Delete(1);

            // Assert
            repository.Verify(x => x.Delete(It.IsAny<long>()), Times.Once);
            documentService.Verify(x => x.DeleteActivityDocumentAsync(activityDocuments.FirstOrDefault()));
            noteService.Verify(x => x.DeleteNote(NoteType.Activity, It.IsAny<long>(), false));
        }

        [Fact]
        public void DeleteActivity_DeleteDocumentFailures()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions(Permissions.ActivityDelete);
            var activityNotes = new List<PimsActivityInstanceNote>() { EntityHelper.CreateActivityNote() };
            var activityDocuments = new List<PimsActivityInstanceDocument>() { EntityHelper.CreateActivityDocument() };

            var repository = _helper.GetService<Mock<IActivityRepository>>();
            var noteService = _helper.GetService<Mock<INoteService>>();
            var documentService = _helper.GetService<Mock<IDocumentActivityService>>();
            documentService.Setup(x => x.GetActivityDocuments(It.IsAny<long>())).Returns(activityDocuments);
            documentService.Setup(x => x.DeleteActivityDocumentAsync(It.IsAny<PimsActivityInstanceDocument>())).ReturnsAsync(new ExternalResult<string>() { Status = ExternalResultStatus.Error });
            noteService.Setup(x => x.GetNotes(NoteType.Activity, It.IsAny<long>())).Returns(activityNotes.Select(n => n.Note));
            repository.Setup(x => x.Delete(It.IsAny<long>()));

            // Act
            Func<Task> act = async () => await service.Delete(1);

            // Assert
            act.Should().Throw<DbUpdateException>();
        }

        [Fact]
        public void DeleteActivity_NoPermission()
        {
            // Arrange
            var service = CreateActivityServiceWithPermissions();

            var repository = _helper.GetService<Mock<IActivityRepository>>();

            // Act
            Func<Task> act = async () => await service.Delete(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Delete(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #endregion
    }
}
