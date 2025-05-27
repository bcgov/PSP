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
    /// LeaseNoteRepository class, provides a repository to interact with lease notes within the datasource.
    /// </summary>
    public class LeaseNoteRepository : BaseRepository<PimsLeaseNote>, INoteRelationshipRepository<PimsLeaseNote>
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a LeaseNoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public LeaseNoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<LeaseNoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get a list of all the notes for a given lease.
        /// </summary>
        /// <param name="parentId">The lease Id.</param>
        /// <returns></returns>
        public IList<PimsNote> GetAllByParentId(long parentId)
        {
            return Context.PimsLeaseNotes
                .Where(x => x.LeaseId == parentId).AsNoTracking().Select(x => x.Note).ToList();
        }

        /// <summary>
        /// Adds the supplied lease note to the database.
        /// </summary>
        /// <param name="noteRelationship"></param>
        /// <returns></returns>
        public PimsLeaseNote AddNoteRelationship(PimsLeaseNote noteRelationship)
        {
            noteRelationship.ThrowIfNull(nameof(noteRelationship));

            var newEntry = Context.PimsLeaseNotes.Add(noteRelationship);
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
        /// Deletes the lease notes matching the supplied note Id.
        /// </summary>
        /// <param name="noteId"></param>
        /// <returns></returns>
        public bool DeleteNoteRelationship(long noteId)
        {
            var leaseNotes = Context.PimsLeaseNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (leaseNotes.Count != 0)
            {
                foreach (var leaseNote in leaseNotes)
                {
                    Context.PimsLeaseNotes.Remove(leaseNote);
                    Context.PimsNotes.Remove(leaseNote.Note);
                }
                return true;
            }
            return false;
        }
        #endregion
    }
}
