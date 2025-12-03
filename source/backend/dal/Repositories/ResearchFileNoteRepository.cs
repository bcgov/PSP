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
    /// ResearchFileNoteRepository class, provides a repository to interact with research file notes within the datasource.
    /// </summary>
    public class ResearchFileNoteRepository : BaseRepository<PimsResearchFileNote>, INoteRelationshipRepository<PimsResearchFileNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ResearchFileNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ResearchFileNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ResearchFileNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given research file.
        /// </summary>
        /// <param name="parentId">The research file Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsResearchFileNotes
                .Where(x => x.ResearchFileId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied research file note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsResearchFileNote AddNoteRelationship(PimsResearchFileNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsResearchFileNotes.Add(noteRelationship);
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
        /// Deletes the research file notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var researchFileNotes = Context.PimsResearchFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (researchFileNotes.Count != 0)
            {
                foreach (var fileNote in researchFileNotes)
                {
                    Context.PimsResearchFileNotes.Remove(fileNote);
                    Context.PimsNotes.Remove(fileNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
