using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using MapsterMapper;
using Moq;
using Pims.Api.Constants;
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
    [Trait("group", "note")]
    [ExcludeFromCodeCoverage]
    public class NoteServiceTest
    {
        private TestHelper _helper;

        public NoteServiceTest()
        {
            _helper = new TestHelper();
        }

        private NoteService CreateNoteServiceWithPermissions(params Permissions[] permissions)
        {
            var user = PrincipalHelper.CreateForPermission(permissions);
            _helper.CreatePimsContext(user, true);
            return _helper.Create<NoteService>();
        }

        #region Tests
        #region Add
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteAdd);

            var mapper = _helper.GetService<IMapper>();
            var activityNote = EntityHelper.CreateActivityNote();
            var noteModel = mapper.Map<EntityNoteModel>(activityNote);

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsActivityInstanceNote>())).Returns(activityNote);

            // Act
            var result = service.Add(NoteType.Activity, noteModel);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstanceNote>()), Times.Once);
        }

        [Fact]
        public void Add_AcquisitionFileNote_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteAdd);

            var mapper = _helper.GetService<IMapper>();
            var acquisitionFileNote = EntityHelper.CreateAcquisitionFileNote();
            var noteModel = mapper.Map<EntityNoteModel>(acquisitionFileNote);

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.Add(It.IsAny<PimsAcquisitionFileNote>())).Returns(acquisitionFileNote);

            // Act
            var result = service.Add(NoteType.Acquisition_File, noteModel);

            // Assert
            repository.Verify(x => x.Add(It.IsAny<PimsAcquisitionFileNote>()), Times.Once);
        }

        [Fact]
        public void Add_NoPermission()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions();

            var mapper = _helper.GetService<IMapper>();
            var activityNote = EntityHelper.CreateActivityNote();
            var noteModel = mapper.Map<EntityNoteModel>(activityNote);

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            Action act = () => service.Add(NoteType.Activity, noteModel);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Add(It.IsAny<PimsActivityInstanceNote>()), Times.Never);
        }
        #endregion

        #region GetById
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteView);

            var note = EntityHelper.CreateNote("Test Note");

            var repository = _helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(note);

            // Act
            var result = service.GetById(1);

            // Assert
            repository.Verify(x => x.GetById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetById_NoPermission()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions();

            var note = EntityHelper.CreateNote("Test Note");

            // Act
            Action act = () => service.GetById(1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region GetNotes
        [Fact]
        public void GetNotes_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteView);

            var notes = new[] { EntityHelper.CreateNote("Test Note 1"), EntityHelper.CreateNote("Test Note 2") };

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.GetAllActivityNotesById(It.IsAny<long>())).Returns(notes);

            // Act
            var result = service.GetNotes(NoteType.Activity, 1);

            // Assert
            repository.Verify(x => x.GetAllActivityNotesById(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void GetNotes_NoPermission()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions();

            // Act
            Action act = () => service.GetNotes(NoteType.Activity, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteEdit, Permissions.NoteView);

            var mapper = _helper.GetService<IMapper>();
            var note = EntityHelper.CreateNote("Test Note");
            var model = mapper.Map<NoteModel>(note);

            var repository = _helper.GetService<Mock<INoteRepository>>();
            repository.Setup(x => x.Update(It.IsAny<PimsNote>())).Returns(note);
            repository.Setup(x => x.GetRowVersion(It.IsAny<long>())).Returns(1);
            repository.Setup(x => x.GetById(It.IsAny<long>())).Returns(note);

            // Act
            var result = service.Update(model);

            // Assert
            repository.Verify(x => x.Update(It.IsAny<PimsNote>()), Times.Once);
        }

        [Fact]
        public void Update_NoPermission()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions();

            var mapper = _helper.GetService<IMapper>();
            var note = EntityHelper.CreateNote("Test Note");
            var model = mapper.Map<NoteModel>(note);

            var repository = _helper.GetService<Mock<INoteRepository>>();

            // Act
            Action act = () => service.Update(model);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.Update(It.IsAny<PimsNote>()), Times.Never);
        }
        #endregion

        #region DeleteNote
        [Fact]
        public void DeleteNote_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteDelete);

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.DeleteActivityNotes(It.IsAny<long>()));

            // Act
            service.DeleteNote(NoteType.Activity, 1);

            // Assert
            repository.Verify(x => x.DeleteActivityNotes(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeleteNote_AcquisitionFile_Success()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions(Permissions.NoteDelete);

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();
            repository.Setup(x => x.DeleteActivityNotes(It.IsAny<long>()));

            // Act
            service.DeleteNote(NoteType.Acquisition_File, 1);

            // Assert
            repository.Verify(x => x.DeleteAcquisitionFileNotes(It.IsAny<long>()), Times.Once);
        }

        [Fact]
        public void DeleteNote_NoPermission()
        {
            // Arrange
            var service = CreateNoteServiceWithPermissions();

            var repository = _helper.GetService<Mock<IEntityNoteRepository>>();

            // Act
            Action act = () => service.DeleteNote(NoteType.Activity, 1);

            // Assert
            act.Should().Throw<NotAuthorizedException>();
            repository.Verify(x => x.DeleteActivityNotes(It.IsAny<long>()), Times.Never);
        }
        #endregion

        #endregion
    }
}
