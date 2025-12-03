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
    /// DispositionFileNoteRepository class, provides a repository to interact with disposition file notes within the datasource.
    /// </summary>
    public class DispositionFileNoteRepository : BaseRepository<PimsDispositionFileNote>, INoteRelationshipRepository<PimsDispositionFileNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a DispositionFileNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public DispositionFileNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<DispositionFileNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given disposition file.
        /// </summary>
        /// <param name="parentId">The disposition file Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsDispositionFileNotes
                .Where(x => x.DispositionFileId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied disposition file note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsDispositionFileNote AddNoteRelationship(PimsDispositionFileNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsDispositionFileNotes.Add(noteRelationship);
            if (newEntry.State == EntityState.Added)
            {
                return newEntry.Entity;
            }
            else
            {
                throw new InvalidOperationException("Could not create note");
            }
        }

        /// <summary>
        /// Deletes the disposition file notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var dispositionFileNotes = Context.PimsDispositionFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (dispositionFileNotes.Count != 0)
            {
                foreach (var fileNote in dispositionFileNotes)
                {
                    Context.PimsDispositionFileNotes.Remove(fileNote);
                    Context.PimsNotes.Remove(fileNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
