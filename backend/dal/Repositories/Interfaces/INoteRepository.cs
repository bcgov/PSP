using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface INoteRepository : IRepository
    {
        PimsNote Add(PimsNote note);
        long GetRowVersion(long id);
        int Count();

        IEnumerable<PimsNote> GetActivityNotes();

        void DeleteActivityNotes(int noteId);
    }
}
