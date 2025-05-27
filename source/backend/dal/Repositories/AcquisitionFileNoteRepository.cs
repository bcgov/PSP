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
    /// AcquisitionFileNoteRepository class, provides a repository to interact with acquisition file notes within the datasource.
    /// </summary>
    public class AcquisitionFileNoteRepository : BaseRepository<PimsAcquisitionFileNote>, INoteRelationshipRepository<PimsAcquisitionFileNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a AcquisitionFileNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public AcquisitionFileNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<AcquisitionFileNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given acquisition file.
        /// </summary>
        /// <param name="parentId">The acquisition file Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsAcquisitionFileNotes
                .Where(x => x.AcquisitionFileId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied acquisition file note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsAcquisitionFileNote AddNoteRelationship(PimsAcquisitionFileNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsAcquisitionFileNotes.Add(noteRelationship);
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
        /// Deletes the acquisition file notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var acquisitionFileNotes = Context.PimsAcquisitionFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (acquisitionFileNotes.Count != 0)
            {
                foreach (var fileNote in acquisitionFileNotes)
                {
                    Context.PimsAcquisitionFileNotes.Remove(fileNote);
                    Context.PimsNotes.Remove(fileNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
