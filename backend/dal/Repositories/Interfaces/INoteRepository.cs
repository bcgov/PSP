using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface INoteRepository : IRepository
    {
        PimsNote Add(PimsNote note);
        long GetRowVersion(long id);
    }
}
