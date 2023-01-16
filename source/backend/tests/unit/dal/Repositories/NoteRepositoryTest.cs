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
    [Trait("group", "note")]
    [ExcludeFromCodeCoverage]
    public class NoteRepositoryTest
    {
        #region Constructors
        public NoteRepositoryTest() { }
        #endregion

        #region Tests

        #region Add Note
        [Fact]
        public void Add_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);

            var activity = EntityHelper.CreateActivity(1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            var activityNote = EntityHelper.CreateActivityNote(activity);

            // Act
            var result = repository.Add(activityNote);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsActivityInstanceNote>();
            result.Note.NoteTxt.Should().Be("Test Note");
            result.ActivityInstance.ActivityInstanceId.Should().Be(1);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            Action act = () => repository.Add<PimsActivityInstance>(null);

            // Assert
            act.Should().Throw<ArgumentNullException>();
        }
        #endregion

        #region Get Note By Id
        [Fact]
        public void GetById_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note = EntityHelper.CreateNote("Test Note", id: 1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(note);
            var repository = helper.CreateRepository<NoteRepository>(user);

            // Act
            var result = repository.GetById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsNote>();
            result.NoteTxt.Should().Be("Test Note");
            result.Id.Should().Be(note.Id);
        }

        [Fact]
        public void GetById_NotFound()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<NoteRepository>(user);

            // Act
            Action act = () => repository.GetById(1);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Get All Notes for Entity
        [Fact]
        public void GetActivityNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var note2 = EntityHelper.CreateNote("Test Note 2", id: 2);
            var activity = EntityHelper.CreateActivity(1, new[] { note1, note2 });

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetActivityNotes(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetAcquisitionFileNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var acqFile = EntityHelper.CreateAcquisitionFile(1);
            var activity = EntityHelper.CreateAcquisitionFileNote(acqFile, note1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAcquisitionFileNotes(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetActivityNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetActivityNotes(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(0);
        }
        #endregion

        #region Update
        [Fact]
        public void Update_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteEdit);

            var note = EntityHelper.CreateNote("Test Note", id: 1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(note);

            var repository = helper.CreateRepository<NoteRepository>(user);

            // Act
            var noteUpdated = EntityHelper.CreateNote("updated", id: 1);
            var result = repository.Update(noteUpdated);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsNote>();
            result.NoteTxt.Should().Be(noteUpdated.NoteTxt);
        }

        [Fact]
        public void Update_Null()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteEdit);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<NoteRepository>(user);

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
            var repository = helper.CreateRepository<NoteRepository>(user);
            var note = EntityHelper.CreateNote("Test Note 1", id: 1);

            // Act
            Action act = () => repository.Update(note);

            // Assert
            act.Should().Throw<KeyNotFoundException>();
        }
        #endregion

        #region Delete
        [Fact]
        public void Delete_Activity_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var activityNote = EntityHelper.CreateActivityNote();
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activityNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteActivityNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Acquisition_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateAcquisitionFileNote();
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteAcquisitionFileNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Acquisition_NoNote()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteAcquisitionFileNotes(1);

            // Assert
            deleted.Should().BeFalse();
        }
        #endregion

        #region Count
        [Fact]
        public void Notes_Count()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note = EntityHelper.CreateNote("Test Note", id: 1);
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(note);

            var repository = helper.CreateRepository<NoteRepository>(user);

            // Act
            var result = repository.Count();

            // Assert
            result.Should().Be(1);
        }
        #endregion

        #endregion
    }
}
