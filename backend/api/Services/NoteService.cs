using System.Collections.Generic;
using System.Security.Claims;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Api.Constants;
using Pims.Api.Models.Concepts;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Pims.Dal.Helpers.Extensions;
using Pims.Dal.Repositories;
using Pims.Dal.Security;

namespace Pims.Api.Services
{
    public class NoteService : BaseService, INoteService
    {
        private readonly INoteRepository _noteRepository;
        private readonly IEntityNoteRepository _entityNoteRepository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Creates a new instance of a NoteService, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="noteRepository"></param>
        /// <param name="entityNoteRepository"></param>
        public NoteService(ClaimsPrincipal user, ILogger<NoteService> logger, IMapper mapper, INoteRepository noteRepository, IEntityNoteRepository entityNoteRepository)
            : base(user, logger)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
            _entityNoteRepository = entityNoteRepository;
        }

        public NoteModel GetById(long id)
        {
            this.Logger.LogInformation("Getting note with id {id}", id);
            this.User.ThrowIfNotAuthorized(Permissions.NoteView);

            var pimsEntity = _noteRepository.GetById(id);
            var noteModel = _mapper.Map<NoteModel>(pimsEntity);

            return noteModel;
        }

        public EntityNoteModel Add(NoteType type, EntityNoteModel model)
        {
            this.Logger.LogInformation("Adding note with type {type} and model {model}", type, model);
            model.ThrowIfNull(nameof(model));
            this.User.ThrowIfNotAuthorized(Permissions.NoteAdd);

            EntityNoteModel result;

            switch (type)
            {
                case NoteType.Activity:
                default:
                    var pimsEntity = _mapper.Map<PimsActivityInstanceNote>(model);

                    var createdEntity = _entityNoteRepository.Add<PimsActivityInstanceNote>(pimsEntity);
                    _entityNoteRepository.CommitTransaction();

                    result = _mapper.Map<EntityNoteModel>(createdEntity);
                    break;
            }

            return result;
        }

        public NoteModel Update(NoteModel model)
        {
            model.ThrowIfNull(nameof(model));

            this.Logger.LogInformation("Updating note with id {id}", model.Id);
            this.User.ThrowIfNotAuthorized(Permissions.NoteEdit);

            ValidateVersion(model.Id, model.RowVersion);

            var pimsEntity = _mapper.Map<PimsNote>(model);
            var newNote = _noteRepository.Update(pimsEntity);
            _noteRepository.CommitTransaction();

            this.Logger.LogInformation("Note with id {id} update successfully", model.Id);
            return GetById(newNote.Id);
        }

        /// <summary>
        /// Find and delete the note.
        /// </summary>
        /// <param name="type">Note type to determine the type of note to delete.</param>
        /// <param name="noteId">Note id to identify the note to delete.</param>
        public void DeleteNote(NoteType type, long noteId)
        {
            this.Logger.LogInformation("Deleting note with type {type} and id {noteId}", type, noteId);
            this.User.ThrowIfNotAuthorized(Permissions.NoteDelete);

            switch (type)
            {
                case NoteType.Activity:
                    _noteRepository.DeleteActivityNotes(noteId);
                    _entityNoteRepository.CommitTransaction();
                    break;
                case NoteType.File:
                    // Write code to delete the note from FileNotes table
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get notes by note type.
        /// </summary>
        /// <param name="type">Note type to determine the type of notes to return.</param>
        /// <param name="entityId">Entity Id to determine the entity to which notes belongs to.</param>
        /// <returns></returns>
        public IEnumerable<PimsNote> GetNotes(NoteType type, long entityId)
        {
            this.Logger.LogInformation("Getting all notes with type {type} and parent id {entityId}", type, entityId);
            this.User.ThrowIfNotAuthorized(Permissions.NoteView);

            switch (type)
            {
                case NoteType.Activity:
                    return _noteRepository.GetActivityNotes(entityId);
                case NoteType.File:
                default:
                    return new List<PimsNote>();
            }
        }

        private void ValidateVersion(long noteId, long noteVersion)
        {
            long currentRowVersion = _noteRepository.GetRowVersion(noteId);
            if (currentRowVersion != noteVersion)
            {
                throw new DbUpdateConcurrencyException("You are working with an older version of this note, please refresh the application and retry.");
            }
        }
    }
}
