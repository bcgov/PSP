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
    public class NoteRepository : BaseRepository<PimsNote>, INoteRepository
    {
        #region Constructors

        /// <summary>
        /// Creates a new instance of a NoteRepository, and initializes it with the specified arguments.
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="user"></param>
        /// <param name="logger"></param>
        public NoteRepository(PimsContext dbContext, ClaimsPrincipal user, ILogger<NoteRepository> logger)
            : base(dbContext, user, logger)
        {
        }
        #endregion

        #region Methods

        /// <summary>
        /// Retrieves the note with the specified id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PimsNote GetById(long id)
        {
            return this.Context.PimsNotes.AsNoTracking()
                .FirstOrDefault(x => x.NoteId == id) ?? throw new KeyNotFoundException();
        }

        /// <summary>
        /// Adds the specified note to the datasource.
        /// </summary>
        /// <param name="note">The note to add.</param>
        /// <returns></returns>
        public PimsNote Add(PimsNote note)
        {
            note.ThrowIfNull(nameof(note));
            this.Context.PimsNotes.Add(note);
            return note;
        }

        /// <summary>
        /// Updates the specified note.
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public PimsNote Update(PimsNote note)
        {
            note.ThrowIfNull(nameof(note));

            var existingNote = this.Context.PimsNotes
                .FirstOrDefault(x => x.NoteId == note.Internal_Id) ?? throw new KeyNotFoundException();

            // update main entity - PimsNote
            this.Context.Entry(existingNote).CurrentValues.SetValues(note);

            return existingNote;
        }

        /// <summary>
        /// Retrieves the row version of the note with the specified id.
        /// </summary>
        /// <param name="id">The note id.</param>
        /// <returns>The row version.</returns>
        public long GetRowVersion(long id)
        {
            return this.Context.PimsNotes.AsNoTracking()
                .Where(n => n.NoteId == id)?
                .Select(n => n.ConcurrencyControlNumber)?
                .FirstOrDefault() ?? throw new KeyNotFoundException();
        }

        public int Count()
        {
            return this.Context.PimsNotes.Count();
        }
        #endregion
    }
}
