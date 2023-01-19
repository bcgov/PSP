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

        public IEnumerable<PimsNote> GetAllActivityNotesById(long entityId)
        {
            return this.Context.PimsActivityInstanceNotes
                .Where(x => x.ActivityInstanceId == entityId).Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllAcquisitionNotesById(long entityId)
        {
            return this.Context.PimsAcquisitionFileNotes
                .Where(x => x.AcquisitionFileId == entityId).Select(x => x.Note).ToList();
        }

        public bool DeleteActivityNotes(long entityId)
        {
            var activityNotes = this.Context.PimsActivityInstanceNotes.Include(ai => ai.Note).Where(x => x.NoteId == entityId).ToList();
            if (activityNotes.Any())
            {
                foreach (var activityNote in activityNotes)
                {
                    this.Context.PimsActivityInstanceNotes.Remove(activityNote);
                    this.Context.PimsNotes.Remove(activityNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteAcquisitionFileNotes(long entityId)
        {
            var acquisitionFileNotes = this.Context.PimsAcquisitionFileNotes.Include(ai => ai.Note).Where(x => x.AcquisitionFileId == entityId).ToList();
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

        #endregion
    }
}
