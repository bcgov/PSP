using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using MapsterMapper;
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
        /// <param name="service"></param>
        /// <param name="logger"></param>
        public NoteRepository(PimsContext dbContext, ClaimsPrincipal user, IPimsRepository service, ILogger<NoteRepository> logger, IMapper mapper) : base(dbContext, user, service, logger, mapper) { }
        #endregion

        #region Methods

        /// <summary>
        /// Adds the specified note to the datasource
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
        /// Retrieves the row version of the note with the specified id.
        /// </summary>
        /// <param name="id">The note id</param>
        /// <returns>The row version</returns>
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

        public IEnumerable<PimsNote> GetActivityNotes(long entityId)
        {
            return this.Context.PimsActivityInstanceNotes
                .Where(x => x.ActivityInstanceId == entityId && x.IsDisabled == false).Select(x => x.Note).ToList();
        }

        public void DeleteActivityNotes(int noteId)
        {
            var activityNotes = this.Context.PimsActivityInstanceNotes.Where(x => x.NoteId == noteId).ToList();
            if (activityNotes.Any())
            {
                foreach (var note in activityNotes)
                {
                    note.IsDisabled = true;
                    note.AppLastUpdateTimestamp = DateTime.UtcNow;
                    note.AppLastUpdateUserid = User.GetUsername();
                    note.AppLastUpdateUserGuid = User.GetUserKey();
                }
            }
        }
        #endregion
    }
}
