using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface INoteRepository : IRepository
    {
        PimsNote GetById(long id);

        PimsNote Add(PimsNote note);

        PimsNote Update(PimsNote note);

        long GetRowVersion(long id);

        int Count();
    }
}
