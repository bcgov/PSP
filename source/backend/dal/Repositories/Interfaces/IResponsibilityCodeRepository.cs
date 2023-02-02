using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IResponsibilityCodeRepository : IRepository<PimsResponsibilityCode>
    {
        IList<PimsResponsibilityCode> GetAllResponsibilityCodes();

        PimsResponsibilityCode GetById(long id);

        PimsResponsibilityCode Add(PimsResponsibilityCode pimsCode);

        PimsResponsibilityCode Update(PimsResponsibilityCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsResponsibilityCode pimsCode);
    }
}
