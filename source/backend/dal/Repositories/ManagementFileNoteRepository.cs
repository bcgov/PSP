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
    /// ManagementFileNoteRepository class, provides a repository to interact with management file notes within the datasource.
    /// </summary>
    public class ManagementFileNoteRepository : BaseRepository<PimsManagementFileNote>, INoteRelationshipRepository<PimsManagementFileNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ManagementFileNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ManagementFileNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ManagementFileNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given management file.
        /// </summary>
        /// <param name="parentId">The management file Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsManagementFileNotes
                .Where(x => x.ManagementFileId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied management file note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsManagementFileNote AddNoteRelationship(PimsManagementFileNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsManagementFileNotes.Add(noteRelationship);
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
        /// Deletes the management file notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var managementFileNotes = Context.PimsManagementFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (managementFileNotes.Count != 0)
            {
                foreach (var fileNote in managementFileNotes)
                {
                    Context.PimsManagementFileNotes.Remove(fileNote);
                    Context.PimsNotes.Remove(fileNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
