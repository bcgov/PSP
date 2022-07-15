using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface INoteRepository : IRepository
    {
        PimsNote Add(PimsNote note);
        long GetRowVersion(long id);
        int Count();

        IEnumerable<PimsNote> GetActivityNotes(long entityId);

        void DeleteActivityNotes(long noteId);
    }
}
