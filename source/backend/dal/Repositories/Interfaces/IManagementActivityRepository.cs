using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;

namespace Pims.Dal.Repositories
{
    public interface IManagementActivityRepository : IRepository
    {
        int Count();

        Paged<PimsPropertyActivity> GetPageDeep(ManagementActivityFilter filter);
    }
}
