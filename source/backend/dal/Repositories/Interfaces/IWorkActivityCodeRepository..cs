using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IWorkActivityCodeRepository : IRepository<PimsWorkActivityCode>
    {
        IList<PimsWorkActivityCode> GetAllWorkActivityCodes();

        PimsWorkActivityCode GetById(long id);

        PimsWorkActivityCode Add(PimsWorkActivityCode pimsCode);

        PimsWorkActivityCode Update(PimsWorkActivityCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsWorkActivityCode pimsCode);
    }
}
