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
    /// PropertyNoteRepository class, provides a repository to interact with property notes within the datasource.
    /// </summary>
    public class PropertyNoteRepository : BaseRepository<PimsPropertyNote>, INoteRelationshipRepository<PimsPropertyNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a PropertyNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public PropertyNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<PropertyNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given property.
        /// </summary>
        /// <param name="parentId">The property Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsPropertyNotes
                .Where(x => x.PropertyId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied property note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsPropertyNote AddNoteRelationship(PimsPropertyNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsPropertyNotes.Add(noteRelationship);
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
        /// Deletes the property notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var propertyNotes = Context.PimsPropertyNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (propertyNotes.Count != 0)
            {
                foreach (var propertyNote in propertyNotes)
                {
                    Context.PimsPropertyNotes.Remove(propertyNote);
                    Context.PimsNotes.Remove(propertyNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
