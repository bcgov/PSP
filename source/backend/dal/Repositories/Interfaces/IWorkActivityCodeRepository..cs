using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IWorkActivityCodeRepository : IRepository<PimsWorkActivityCode>
    {
        IList<PimsWorkActivityCode> GetAllWorkActivityCodes();
    }
}
