using System.Collections.Generic;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts.Note;
using Pims.Core.Api.Exceptions;
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
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a NoteService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
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
            IMapper mapper,
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
            _mapper = mapper;
            _noteRepository = noteRepository;
            _acquisitionFileNoteRepository = acquisitionFileNoteRepository;
            _dispositionFileNoteRepository = dispositionFileNoteRepository;
            _leaseNoteRepository = leaseNoteRepository;
            _managementFileNoteRepository = managementFileNoteRepository;
            _projectNoteRepository = projectNoteRepository;
            _propertyNoteRepository = propertyNoteRepository;
            _researchFileNoteRepository = researchFileNoteRepository;
        }

        public NoteModel GetById(long id)
        {
            Logger.LogInformation("Getting note with id {Id}", id);
            User.ThrowIfNotAuthorized(Permissions.NoteView);

            var pimsEntity = _noteRepository.GetById(id);
            var noteModel = _mapper.Map<NoteModel>(pimsEntity);

            return noteModel;
        }

        public EntityNoteModel Add(NoteType type, EntityNoteModel model)
        {
            Logger.LogInformation("Adding note with type {Type} and model {Model}", type, model);
            model.ThrowIfNull(nameof(model));
            User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            EntityNoteModel result;

            switch (type)
            {
                case NoteType.Acquisition_File:
                    PimsAcquisitionFileNote acqNoteEntity = _mapper.Map<PimsAcquisitionFileNote>(model);

                    PimsAcquisitionFileNote createdAcqEntity = _acquisitionFileNoteRepository.AddNoteRelationship(acqNoteEntity);
                    _acquisitionFileNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdAcqEntity);
                    break;
                case NoteType.Disposition_File:
                    PimsDispositionFileNote dispositionNoteEntity = _mapper.Map<PimsDispositionFileNote>(model);

                    PimsDispositionFileNote createdDispositionEntity = _dispositionFileNoteRepository.AddNoteRelationship(dispositionNoteEntity);
                    _dispositionFileNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdDispositionEntity);
                    break;
                case NoteType.Lease_File:
                    PimsLeaseNote leaseNoteEntity = _mapper.Map<PimsLeaseNote>(model);

                    PimsLeaseNote createdLeaseEntity = _leaseNoteRepository.AddNoteRelationship(leaseNoteEntity);
                    _leaseNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdLeaseEntity);
                    break;
                case NoteType.Project:
                    var projectNote = _mapper.Map<PimsProjectNote>(model);

                    var createdNote = _projectNoteRepository.AddNoteRelationship(projectNote);
                    _projectNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdNote);
                    break;
                case NoteType.Research_File:
                    PimsResearchFileNote researchNoteEntity = _mapper.Map<PimsResearchFileNote>(model);

                    PimsResearchFileNote createdResesearchEntity = _researchFileNoteRepository.AddNoteRelationship(researchNoteEntity);
                    _researchFileNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdResesearchEntity);
                    break;
                case NoteType.Management_File:
                    PimsManagementFileNote managementNoteEntity = _mapper.Map<PimsManagementFileNote>(model);

                    PimsManagementFileNote createdManagementNote = _managementFileNoteRepository.AddNoteRelationship(managementNoteEntity);
                    _managementFileNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdManagementNote);
                    break;
                case NoteType.Property:
                    var propertyNoteEntity = _mapper.Map<PimsPropertyNote>(model);

                    var createdPropertyNote = _propertyNoteRepository.AddNoteRelationship(propertyNoteEntity);
                    _propertyNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdPropertyNote);
                    break;
                default:
                    throw new BadRequestException("Relationship type not valid.");
            }

            return result;
        }

        public NoteModel Update(NoteModel model)
        {
            model.ThrowIfNull(nameof(model));

            Logger.LogInformation("Updating note with id {Id}", model.Id);
            User.ThrowIfNotAuthorized(Permissions.NoteEdit);

            ValidateVersion(model.Id, model.RowVersion);

            var pimsEntity = _mapper.Map<PimsNote>(model);
            var newNote = _noteRepository.Update(pimsEntity);
            _noteRepository.CommitTransaction();

            Logger.LogInformation("Note with id {Id} update successfully", model.Id);
            return GetById(newNote.Internal_Id);
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
        /// <param name="entityId">Entity Id to determine the entity to which notes belongs to.</param>
        /// <returns></returns>
        public IEnumerable<PimsNote> GetNotes(NoteType type, long entityId)
        {
            Logger.LogInformation("Getting all notes with type {Type} and parent id {EntityId}", type, entityId);
            User.ThrowIfNotAuthorized(Permissions.NoteView);

            IList<PimsNote> notes = type switch
            {
                NoteType.Acquisition_File => _acquisitionFileNoteRepository.GetAllByParentId(entityId),
                NoteType.Disposition_File => _dispositionFileNoteRepository.GetAllByParentId(entityId),
                NoteType.Project => _projectNoteRepository.GetAllByParentId(entityId),
                NoteType.Lease_File => _leaseNoteRepository.GetAllByParentId(entityId),
                NoteType.Research_File => _researchFileNoteRepository.GetAllByParentId(entityId),
                NoteType.Management_File => _managementFileNoteRepository.GetAllByParentId(entityId),
                NoteType.Property => _propertyNoteRepository.GetAllByParentId(entityId),
                _ => new List<PimsNote>()
            };

            return notes;
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
