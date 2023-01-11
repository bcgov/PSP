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

        IEnumerable<PimsNote> GetActivityNotes(long entityId);

        IEnumerable<PimsNote> GetAcquisitionFileNotes(long entityId);

        void DeleteActivityNotes(long entityId);

        void DeleteAcquisitionFileNotes(long entityId);
    }
}
