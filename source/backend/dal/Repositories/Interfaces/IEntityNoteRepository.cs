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

        IEnumerable<PimsNote> GetAllAcquisitionNotesById(long entityId);

        IEnumerable<PimsNote> GetAllProjectNotesById(long entityId);

        bool DeleteActivityNotes(long entityId);

        bool DeleteAcquisitionFileNotes(long entityId);

        bool DeleteProjectNotes(long entityId);
    }
}
