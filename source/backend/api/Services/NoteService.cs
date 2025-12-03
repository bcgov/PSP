using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Core.Api.Services;
using Pims.Core.Extensions;
using Pims.Core.Security;
using Pims.Dal.Entities;
using Pims.Dal.Repositories;

namespace Pims.Api.Services
{
    public class NoteService : BaseService, INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly INoteRelationshipRepository<PimsAcquisitionFileNote> _acquisitionFileNoteRepository;
        private readonly INoteRelationshipRepository<PimsDispositionFileNote> _dispositionFileNoteRepository;
        private readonly INoteRelationshipRepository<PimsLeaseNote> _leaseNoteRepository;
        private readonly INoteRelationshipRepository<PimsManagementFileNote> _managementFileNoteRepository;
        private readonly INoteRelationshipRepository<PimsProjectNote> _projectNoteRepository;
        private readonly INoteRelationshipRepository<PimsPropertyNote> _propertyNoteRepository;
        private readonly INoteRelationshipRepository<PimsResearchFileNote> _researchFileNoteRepository;

        /// <summary>
        /// Creates a new instance of a NoteService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="noteRepository"></param>
        /// <param name="acquisitionFileNoteRepository"></param>
        /// <param name="dispositionFileNoteRepository"></param>
        /// <param name="leaseNoteRepository"></param>
        /// <param name="managementFileNoteRepository"></param>
        /// <param name="projectNoteRepository"></param>
        /// <param name="propertyNoteRepository"></param>
        /// <param name="researchFileNoteRepository"></param>
        public NoteService(
            ClaimsPrincipal user,
            ILogger<NoteService> logger,
            INoteRepository noteRepository,
            INoteRelationshipRepository<PimsAcquisitionFileNote> acquisitionFileNoteRepository,
            INoteRelationshipRepository<PimsDispositionFileNote> dispositionFileNoteRepository,
            INoteRelationshipRepository<PimsLeaseNote> leaseNoteRepository,
            INoteRelationshipRepository<PimsManagementFileNote> managementFileNoteRepository,
            INoteRelationshipRepository<PimsProjectNote> projectNoteRepository,
            INoteRelationshipRepository<PimsPropertyNote> propertyNoteRepository,
            INoteRelationshipRepository<PimsResearchFileNote> researchFileNoteRepository)
            : base(user, logger)
        {
            _noteRepository = noteRepository;
            _acquisitionFileNoteRepository = acquisitionFileNoteRepository;
            _dispositionFileNoteRepository = dispositionFileNoteRepository;
            _leaseNoteRepository = leaseNoteRepository;
            _managementFileNoteRepository = managementFileNoteRepository;
            _projectNoteRepository = projectNoteRepository;
            _propertyNoteRepository = propertyNoteRepository;
            _researchFileNoteRepository = researchFileNoteRepository;
        }

        public PimsNote GetById(long id)
        {
            Logger.LogInformation("Getting note with id {Id}", id);
            User.ThrowIfNotAuthorized(Permissions.NoteView);

            return _noteRepository.GetById(id);
        }

        public PimsAcquisitionFileNote AddAcquisitionFileNote(PimsAcquisitionFileNote acquisitionFileNote)
        {
            acquisitionFileNote.ThrowIfNull(nameof(acquisitionFileNote));
            Logger.LogInformation("Adding note for acquisition file with Id: {Id}", acquisitionFileNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(acquisitionFileNote, _acquisitionFileNoteRepository);
        }

        public PimsDispositionFileNote AddDispositionFileNote(PimsDispositionFileNote dispositionFileNote)
        {
            dispositionFileNote.ThrowIfNull(nameof(dispositionFileNote));
            Logger.LogInformation("Adding note for disposition file with Id: {Id}", dispositionFileNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(dispositionFileNote, _dispositionFileNoteRepository);
        }

        public PimsLeaseNote AddLeaseNote(PimsLeaseNote leaseNote)
        {
            leaseNote.ThrowIfNull(nameof(leaseNote));
            Logger.LogInformation("Adding note for lease with Id: {Id}", leaseNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(leaseNote, _leaseNoteRepository);
        }

        public PimsManagementFileNote AddManagementFileNote(PimsManagementFileNote managementFileNote)
        {
            managementFileNote.ThrowIfNull(nameof(managementFileNote));
            Logger.LogInformation("Adding note for management file with Id: {Id}", managementFileNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(managementFileNote, _managementFileNoteRepository);
        }

        public PimsProjectNote AddProjectNote(PimsProjectNote projectNote)
        {
            projectNote.ThrowIfNull(nameof(projectNote));
            Logger.LogInformation("Adding note for project with Id: {Id}", projectNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(projectNote, _projectNoteRepository);
        }

        public PimsPropertyNote AddPropertyNote(PimsPropertyNote propertyNote)
        {
            propertyNote.ThrowIfNull(nameof(propertyNote));
            Logger.LogInformation("Adding note for property with Id: {Id}", propertyNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(propertyNote, _propertyNoteRepository);
        }

        public PimsResearchFileNote AddResearchFileNote(PimsResearchFileNote researchFileNote)
        {
            researchFileNote.ThrowIfNull(nameof(researchFileNote));
            Logger.LogInformation("Adding note for research file with Id: {Id}", researchFileNote.ParentId);
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            return AddNoteRelationship(researchFileNote, _researchFileNoteRepository);
        }

        public PimsNote Update(PimsNote note)
        {
            note.ThrowIfNull(nameof(note));
            Logger.LogInformation("Updating note with id {Id}", note.Internal_Id);
            User.ThrowIfNotAuthorized(Permissions.NoteEdit);

            ValidateVersion(note.Internal_Id, note.ConcurrencyControlNumber);

            var updatedNote = _noteRepository.Update(note);
            _noteRepository.CommitTransaction();

            Logger.LogInformation("Note with id {Id} updated successfully", note.Internal_Id);
            return GetById(updatedNote.Internal_Id);
        }

        /// <summary>
        /// Find and delete the note.
        /// </summary>
        /// <param name="type">Note type to determine the type of note to delete.</param>
        /// <param name="noteId">Note id to identify the note to delete.</param>
        /// <param name="commitTransaction">Whether or not this transaction should be commited as part of this function.</param>
        public bool DeleteNote(NoteType type, long noteId, bool commitTransaction = true)
        {
            Logger.LogInformation("Deleting note with type {Type} and id {NoteId}", type, noteId);
            User.ThrowIfNotAuthorized(Permissions.NoteDelete);
            bool deleted = false;

            deleted = type switch
            {
                NoteType.Acquisition_File => _acquisitionFileNoteRepository.DeleteNoteRelationship(noteId),
                NoteType.Disposition_File => _dispositionFileNoteRepository.DeleteNoteRelationship(noteId),
                NoteType.Project => _projectNoteRepository.DeleteNoteRelationship(noteId),
                NoteType.Lease_File => _leaseNoteRepository.DeleteNoteRelationship(noteId),
                NoteType.Research_File => _researchFileNoteRepository.DeleteNoteRelationship(noteId),
                NoteType.Management_File => _managementFileNoteRepository.DeleteNoteRelationship(noteId),
                NoteType.Property => _propertyNoteRepository.DeleteNoteRelationship(noteId),
                _ => deleted
            };

            if (commitTransaction)
            {
                _noteRepository.CommitTransaction();
            }

            return deleted;
        }

        /// <summary>
        /// Get notes by note type.
        /// </summary>
        /// <param name="type">Note type to determine the type of notes to return.</param>
        /// <param name="parentId">Entity Id to determine the entity to which notes belongs to.</param>
        /// <returns></returns>
        public IEnumerable<PimsNote> GetNotes(NoteType type, long parentId)
        {
            Logger.LogInformation("Getting all notes with type {Type} and parent id {EntityId}", type, parentId);
            User.ThrowIfNotAuthorized(Permissions.NoteView);

            IList<PimsNote> notes = type switch
            {
                NoteType.Acquisition_File => _acquisitionFileNoteRepository.GetAllByParentId(parentId),
                NoteType.Disposition_File => _dispositionFileNoteRepository.GetAllByParentId(parentId),
                NoteType.Project => _projectNoteRepository.GetAllByParentId(parentId),
                NoteType.Lease_File => _leaseNoteRepository.GetAllByParentId(parentId),
                NoteType.Research_File => _researchFileNoteRepository.GetAllByParentId(parentId),
                NoteType.Management_File => _managementFileNoteRepository.GetAllByParentId(parentId),
                NoteType.Property => _propertyNoteRepository.GetAllByParentId(parentId),
                _ => new List<PimsNote>()
            };

            return notes;
        }

        private static T AddNoteRelationship<T>(T noteRelationship, INoteRelationshipRepository<T> noteRelationshipRepository)
            where T : PimsNoteRelationship, new()
        {
            T newEntity = noteRelationshipRepository.AddNoteRelationship(noteRelationship);
            noteRelationshipRepository.CommitTransaction();

            return newEntity;
        }

        private void ValidateVersion(long noteId, long? noteVersion)
        {
            long currentRowVersion = _noteRepository.GetRowVersion(noteId);
            if (currentRowVersion != noteVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this note, please refresh the application and retry.");
            }
        }
    }
}
