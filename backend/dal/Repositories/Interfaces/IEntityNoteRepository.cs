using System;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    public interface IEntityNoteRepository : IRepository
    {
        T Add<T>(T entity) where T : class;

        IEnumerable<T> GetAll<T>(Func<T, bool> predicate) where T : class;
    }
}
