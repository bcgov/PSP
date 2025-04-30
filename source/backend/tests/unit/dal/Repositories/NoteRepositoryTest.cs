using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using Pims.Core.Security;
using Pims.Core.Test;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;
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

            var acquisitionFile = EntityHelper.CreateAcquisitionFile(1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(acquisitionFile);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            var activityNote = EntityHelper.CreateAcquisitionFileNote(acquisitionFile);

            // Act
            var result = repository.Add(activityNote);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<PimsAcquisitionFileNote>();
            result.Note.NoteTxt.Should().Be("Test Note");
            result.AcquisitionFile.AcquisitionFileId.Should().Be(1);
        }

        [Fact]
        public void Add_ThrowIfNull()
        {
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteAdd);
            helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            Action act = () => repository.Add<PimsProject>(null);

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
            result.NoteId.Should().Be(note.NoteId);
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
            var result = repository.GetAllAcquisitionNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetAcquisitionFileNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllAcquisitionNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetDispositionFileNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var dispFile = EntityHelper.CreateDispositionFile(1);
            var activity = EntityHelper.CreateDispositionFileNote(dispFile, note1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllDispositionNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetDispositionFileNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllDispositionNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetLeaseFileNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var leaseFile = EntityHelper.CreateLease(1);
            var activity = EntityHelper.CreateLeaseNote(leaseFile, note1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllLeaseNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetLeaseFileNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllLeaseNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetProjectNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var note2 = EntityHelper.CreateNote("Test Note 2", id: 2);
            var project = EntityHelper.CreateProject(1, "PROJECT_CODE_TEST", "Some Description");
            project.PimsProjectNotes = new List<PimsProjectNote>() { new PimsProjectNote() { Note = note1 }, new PimsProjectNote() { Note = note2 } };

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(project);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllProjectNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(2);
        }

        [Fact]
        public void GetProjectNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllProjectNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetResearchFileNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var researchFile = EntityHelper.CreateResearchFile(1);
            var activity = EntityHelper.CreateResearchNote(researchFile, note1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(activity);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllResearchNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetResearchFileNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllResearchNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(0);
        }

        [Fact]
        public void GetManagementFileNotes_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var note1 = EntityHelper.CreateNote("Test Note 1", id: 1);
            var file = EntityHelper.CreateManagementFile(1);
            var fileNote = EntityHelper.CreateManagementFileNote(file, note1);

            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllManagementNotesById(1);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeAssignableTo<IEnumerable<PimsNote>>();
            result.Should().HaveCount(1);
        }

        [Fact]
        public void GetManagementFileNotes_NotFound()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission(Permissions.NoteView);

            var context = helper.CreatePimsContext(user, true);
            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var result = repository.GetAllManagementNotesById(1);

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
        public void Delete_Acquisition_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateAcquisitionFileNote();
            fileNote.AcquisitionFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteAcquisitionFileNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Acquisition_WrongId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateAcquisitionFileNote();
            fileNote.AcquisitionFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteAcquisitionFileNotes(2);

            // Assert
            deleted.Should().BeFalse();
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

        [Fact]
        public void Delete_Disposition_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateDispositionFileNote();
            fileNote.DispositionFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteDispositionFileNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Disposition_WrongId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateDispositionFileNote();
            fileNote.DispositionFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteDispositionFileNotes(2);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Disposition_NoNote()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteDispositionFileNotes(1);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Lease_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateLeaseNote();
            fileNote.LeaseId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteLeaseFileNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Lease_WrongId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateLeaseNote();
            fileNote.LeaseId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteLeaseFileNotes(2);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Lease_NoNote()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteLeaseFileNotes(1);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Project_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateProjectNote();
            fileNote.ProjectId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteProjectNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Project_WrongId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateProjectNote();
            fileNote.ProjectId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteProjectNotes(2);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Project_NoNote()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteProjectNotes(1);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Research_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateResearchNote();
            fileNote.ResearchFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteResearchNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Research_WrongId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateResearchNote();
            fileNote.ResearchFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteResearchNotes(2);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Research_NoNote()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteResearchNotes(1);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Management_Success()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateManagementFileNote();
            fileNote.ManagementFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteManagementFileNotes(1);

            // Assert
            deleted.Should().BeTrue();
        }

        [Fact]
        public void Delete_Management_WrongId()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var fileNote = EntityHelper.CreateManagementFileNote();
            fileNote.ManagementFileId = 2;
            var context = helper.CreatePimsContext(user, true).AddAndSaveChanges(fileNote);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteManagementFileNotes(2);

            // Assert
            deleted.Should().BeFalse();
        }

        [Fact]
        public void Delete_Management_NoNote()
        {
            // Arrange
            var helper = new TestHelper();
            var user = PrincipalHelper.CreateForPermission();

            var context = helper.CreatePimsContext(user, true);

            var repository = helper.CreateRepository<EntityNoteRepository>(user);

            // Act
            var deleted = repository.DeleteManagementFileNotes(1);

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
