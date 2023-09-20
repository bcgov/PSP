using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    /// <summary>
    /// Provides a repository to interact with notes within the datasource.
    /// </summary>
    public class EntityNoteRepository : BaseRepository, IEntityNoteRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a NoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public EntityNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<EntityNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Associates a note to an existing parent entity via intermediate table (e.g. PimsActivityInstanceNote).
        /// </summary>
        /// <typeparam name="T">The entity type - e.g. PimsActivityInstanceNote.</typeparam>
        /// <param name="entity">The entity to add to the datastore.</param>
        /// <returns>The created association.</returns>
        public T Add<T>(T entity)
            where T : class
        {
            entity.ThrowIfNull(nameof(entity));
            this.Context.Add<T>(entity);
            return entity;
        }

        /// <summary>
        /// Retrieves all notes associated with a parent entity.
        /// </summary>
        /// <typeparam name="T">The entity type - e.g. PimsActivityInstanceNote.</typeparam>
        /// <param name="predicate">The predicate to filter all notes - e.g. where parentId == parent.Id.</param>
        /// <returns>The entity-notes associations.</returns>
        public IEnumerable<T> GetAll<T>(Func<T, bool> predicate)
            where T : class
        {
            predicate.ThrowIfNull(nameof(predicate));
            return this.Context.Set<T>().AsNoTracking().Where(predicate).ToArray();
        }

        public IEnumerable<PimsNote> GetAllAcquisitionNotesById(long acquisitionId)
        {
            return this.Context.PimsAcquisitionFileNotes
                .Where(x => x.AcquisitionFileId == acquisitionId).Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllLeaseNotesById(long leaseId)
        {
            return this.Context.PimsLeaseNotes
                .Where(x => x.LeaseId == leaseId).Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllProjectNotesById(long entityId)
        {
            return this.Context.PimsProjectNotes
                .Where(x => x.ProjectId == entityId).Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllResearchNotesById(long entityId)
        {
            return this.Context.PimsResearchFileNotes
                .Where(x => x.ResearchFileId == entityId).Select(x => x.Note).ToList();
        }

        public bool DeleteAcquisitionFileNotes(long noteId)
        {
            var acquisitionFileNotes = this.Context.PimsAcquisitionFileNotes.
                                        Include(an => an.Note).
                                        Where(x => x.NoteId == noteId).ToList();

            if (acquisitionFileNotes.Any())
            {
                foreach (var acquisitionFileNote in acquisitionFileNotes)
                {
                    this.Context.PimsAcquisitionFileNotes.Remove(acquisitionFileNote);
                    this.Context.PimsNotes.Remove(acquisitionFileNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteLeaseFileNotes(long noteId)
        {
            var leaseFileNotes = this.Context.PimsLeaseNotes.
                                        Include(ln => ln.Note).
                                        Where(x => x.NoteId == noteId).ToList();

            if (leaseFileNotes.Any())
            {
                foreach (var leaseFileNote in leaseFileNotes)
                {
                    this.Context.PimsLeaseNotes.Remove(leaseFileNote);
                    this.Context.PimsNotes.Remove(leaseFileNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteProjectNotes(long noteId)
        {
            var projectNotes = Context.PimsProjectNotes
                .Include(x => x.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (projectNotes.Any())
            {
                foreach (var note in projectNotes)
                {
                    this.Context.PimsProjectNotes.Remove(note);
                    this.Context.PimsNotes.Remove(note.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteResearchNotes(long noteId)
        {
            var researchNotes = Context.PimsResearchFileNotes
                .Include(x => x.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (researchNotes.Any())
            {
                foreach (var note in researchNotes)
                {
                    this.Context.PimsResearchFileNotes.Remove(note);
                    this.Context.PimsNotes.Remove(note.Note);
                }
                return true;
            }
            return false;
        }

        #endregion
    }
}
