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
        /// Creates a new instance of a EntityNoteRepository, and initializes it with the specified arguments.
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
            Context.Add<T>(entity);
            return entity;
        }

        public IEnumerable<PimsNote> GetAllAcquisitionNotesById(long acquisitionId)
        {
            return Context.PimsAcquisitionFileNotes
                .Where(x => x.AcquisitionFileId == acquisitionId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllDispositionNotesById(long dispositionId)
        {
            return Context.PimsDispositionFileNotes
                .Where(x => x.DispositionFileId == dispositionId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllLeaseNotesById(long leaseId)
        {
            return Context.PimsLeaseNotes
                .Where(x => x.LeaseId == leaseId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllProjectNotesById(long projectId)
        {
            return Context.PimsProjectNotes
                .Where(x => x.ProjectId == projectId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllResearchNotesById(long researchId)
        {
            return Context.PimsResearchFileNotes
                .Where(x => x.ResearchFileId == researchId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllManagementNotesById(long managementId)
        {
            return Context.PimsManagementFileNotes
                .Where(x => x.ManagementFileId == managementId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public IEnumerable<PimsNote> GetAllPropertyNotesById(long propertyId)
        {
            return Context.PimsPropertyNotes
                .Where(x => x.PropertyId == propertyId).AsNoTracking().Select(x => x.Note).ToList();
        }

        public bool DeleteAcquisitionFileNotes(long noteId)
        {
            var acquisitionFileNotes = Context.PimsAcquisitionFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (acquisitionFileNotes.Count != 0)
            {
                foreach (var acquisitionFileNote in acquisitionFileNotes)
                {
                    Context.PimsAcquisitionFileNotes.Remove(acquisitionFileNote);
                    Context.PimsNotes.Remove(acquisitionFileNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteDispositionFileNotes(long noteId)
        {
            var dispositionFileNotes = Context.PimsDispositionFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (dispositionFileNotes.Count != 0)
            {
                foreach (var dispositionFileNote in dispositionFileNotes)
                {
                    Context.PimsDispositionFileNotes.Remove(dispositionFileNote);
                    Context.PimsNotes.Remove(dispositionFileNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteLeaseFileNotes(long noteId)
        {
            var leaseFileNotes = Context.PimsLeaseNotes
                .Include(ln => ln.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (leaseFileNotes.Count != 0)
            {
                foreach (var leaseFileNote in leaseFileNotes)
                {
                    Context.PimsLeaseNotes.Remove(leaseFileNote);
                    Context.PimsNotes.Remove(leaseFileNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteProjectNotes(long noteId)
        {
            var projectNotes = Context.PimsProjectNotes
                .Include(x => x.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (projectNotes.Count != 0)
            {
                foreach (var note in projectNotes)
                {
                    Context.PimsProjectNotes.Remove(note);
                    Context.PimsNotes.Remove(note.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteResearchNotes(long noteId)
        {
            var researchNotes = Context.PimsResearchFileNotes
                .Include(x => x.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (researchNotes.Count != 0)
            {
                foreach (var note in researchNotes)
                {
                    Context.PimsResearchFileNotes.Remove(note);
                    Context.PimsNotes.Remove(note.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeleteManagementFileNotes(long noteId)
        {
            var managementFileNotes = Context.PimsManagementFileNotes
                .Include(an => an.Note)
                .Where(x => x.NoteId == noteId).ToList();

            if (managementFileNotes.Count != 0)
            {
                foreach (var managementFileNote in managementFileNotes)
                {
                    Context.PimsManagementFileNotes.Remove(managementFileNote);
                    Context.PimsNotes.Remove(managementFileNote.Note);
                }
                return true;
            }
            return false;
        }

        public bool DeletePropertyNotes(long noteId)
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
