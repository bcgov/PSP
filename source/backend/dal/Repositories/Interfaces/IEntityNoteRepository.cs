using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IEntityNoteRepository : IRepository
    {
        T Add<T>(T entity)
            where T : class;

        IEnumerable<PimsNote> GetAllAcquisitionNotesById(long acquisitionId);

        IEnumerable<PimsNote> GetAllDispositionNotesById(long dispositionId);

        IEnumerable<PimsNote> GetAllLeaseNotesById(long leaseId);

        IEnumerable<PimsNote> GetAllProjectNotesById(long entityId);

        IEnumerable<PimsNote> GetAllResearchNotesById(long entityId);

        bool DeleteAcquisitionFileNotes(long noteId);

        bool DeleteDispositionFileNotes(long noteId);

        bool DeleteLeaseFileNotes(long noteId);

        bool DeleteProjectNotes(long noteId);

        bool DeleteResearchNotes(long noteId);
    }
}
