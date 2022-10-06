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
    [Trait("group", "activity")]
    [ExcludeFromCodeCoverage]
    public class ActivityServiceTest
    {
        #region Tests
        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityAdd);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var activityModel = mapper.Map<EntityNoteModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstance>())).Returns(activity);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityAdd, Permissions.ResearchFileEdit);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var activityModel = mapper.Map<EntityNoteModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstance>())).Returns(activity);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityAdd, Permissions.AcquisitionFileEdit);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var activityModel = mapper.Map<EntityNoteModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstance>())).Returns(activity);

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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityView);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region GetAllByResearchFileId
        [Fact]
        public void GetAllByResearchFileId_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityView, Permissions.ResearchFileView);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activities = new[] { EntityHelper.CreateActivity(), EntityHelper.CreateActivity() };

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.GetAllByResearchFileId(It.IsAny<long>()));

            // Act
            Action act = () => service.GetAllByResearchFileId(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllByResearchFileId(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #region GetAllActivityTemplates
        [Fact]
        public void GetAllActivityTemplates_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityView);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activities = new[] { EntityHelper.CreateActivityTemplate(), EntityHelper.CreateActivityTemplate() };

            var repository = helper.GetService<Mock<IActivityTemplateRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();

            var repository = helper.GetService<Mock<IActivityTemplateRepository>>();
            repository.Setup(x => x.GetAllActivityTemplates());

            // Act
            Action act = () => service.GetAllActivityTemplates();

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.GetAllActivityTemplates(), Times.Never);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityEdit, Permissions.ActivityView);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<NoteModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<ActivityInstanceModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityEdit, Permissions.ActivityView);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<ActivityInstanceModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityEdit, Permissions.ActivityView, Permissions.PropertyEdit);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<NoteModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<ActivityInstanceModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.UpdateActivityResearchProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            Action act = () => service.UpdateActivityResearchProperties(activity);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.UpdateActivityResearchProperties(It.IsAny<PimsActivityInstance>()), Times.Never);
        }

        [Fact]
        public void UpdateActivityAcquisitionProperties_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityEdit, Permissions.ActivityView, Permissions.PropertyEdit);
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<NoteModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);

            var mapper = helper.GetService<IMapper>();
            var activity = EntityHelper.CreateActivity();
            var model = mapper.Map<ActivityInstanceModel>(activity);

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>())).Returns(activity);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(activity);

            // Act
            Action act = () => service.UpdateActivityAcquisitionProperties(activity);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.UpdateActivityAcquisitionProperties(It.IsAny<PimsActivityInstance>()), Times.Never);
        }
        #endregion

        #region DeleteActivity
        [Fact]
        public async void DeleteActivity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityDelete);
            var service = helper.Create<ActivityService>(user);
            var mapper = helper.GetService<IMapper>();
            var activityNotes = new List<PimsActivityInstanceNote>() { EntityHelper.CreateActivityNote() };
            var activityDocuments = new List<PimsActivityInstanceDocument>() { EntityHelper.CreateActivityDocument() };

            var repository = helper.GetService<Mock<IActivityRepository>>();
            var noteService = helper.GetService<Mock<INoteService>>();
            var documentService = helper.GetService<Mock<IDocumentActivityService>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.ActivityDelete);
            var service = helper.Create<ActivityService>(user);
            //var mapper = helper.GetService<IMapper>();
            var activityNotes = new List<PimsActivityInstanceNote>() { EntityHelper.CreateActivityNote() };
            var activityDocuments = new List<PimsActivityInstanceDocument>() { EntityHelper.CreateActivityDocument() };

            var repository = helper.GetService<Mock<IActivityRepository>>();
            var noteService = helper.GetService<Mock<INoteService>>();
            var documentService = helper.GetService<Mock<IDocumentActivityService>>();
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
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();
            var service = helper.Create<ActivityService>(user);
            var mapper = helper.GetService<IMapper>();

            var repository = helper.GetService<Mock<IActivityRepository>>();
            repository.Setup(x => x.Delete(It.IsAny<long>()));

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
