using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IDispositionFileRepository : IRepository
    {
        PimsDispositionFile GetById(long id);

        PimsDispositionFile Add(PimsDispositionFile disposition);

        LastUpdatedByModel GetLastUpdateBy(long id);

        long GetRowVersion(long id);
    }
}
