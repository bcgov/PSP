using System;
using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IEntityNoteRepository : IRepository
    {
        T Add<T>(T entity)
            where T : class;

        IEnumerable<T> GetAll<T>(Func<T, bool> predicate)
            where T : class;

        IEnumerable<PimsNote> GetAllActivityNotesById(long entityId);

        IEnumerable<PimsNote> GetAllAcquisitionNotesById(long acquisitionId);

        IEnumerable<PimsNote> GetAllLeaseNotesById(long leaseId);

        IEnumerable<PimsNote> GetAllProjectNotesById(long entityId);

        bool DeleteActivityNotes(long entityId);

        bool DeleteAcquisitionFileNotes(long noteId);

        bool DeleteLeaseFileNotes(long noteId);

        bool DeleteProjectNotes(long noteId);
    }
}
