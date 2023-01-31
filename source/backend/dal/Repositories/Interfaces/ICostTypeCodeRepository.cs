using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ICostTypeCodeRepository : IRepository<PimsCostTypeCode>
    {
        IList<PimsCostTypeCode> GetAllCostTypeCodes();

        PimsCostTypeCode GetById(long id);

        PimsCostTypeCode Add(PimsCostTypeCode pimsCode);

        PimsCostTypeCode Update(PimsCostTypeCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsCostTypeCode pimsCode);
    }
}
