using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFileRepository : IRepository
    {
        PimsDispositionFile GetById(long id);

        LastUpdatedByModel GetLastUpdateBy(long id);

        long GetRowVersion(long id);
    }
}
