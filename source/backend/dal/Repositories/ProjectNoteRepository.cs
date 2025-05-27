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
    /// ProjectNoteRepository class, provides a repository to interact with project notes within the datasource.
    /// </summary>
    public class ProjectNoteRepository : BaseRepository<PimsProjectNote>, INoteRelationshipRepository<PimsProjectNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a ProjectNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public ProjectNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<ProjectNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given project.
        /// </summary>
        /// <param name="parentId">The project Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsProjectNotes
                .Where(x => x.ProjectId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied project note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsProjectNote AddNoteRelationship(PimsProjectNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsProjectNotes.Add(noteRelationship);
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
        /// Deletes the project notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var projectNotes = Context.PimsProjectNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (projectNotes.Count != 0)
            {
                foreach (var projectNote in projectNotes)
                {
                    Context.PimsProjectNotes.Remove(projectNote);
                    Context.PimsNotes.Remove(projectNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
