using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ICostTypeCodeRepository : IRepository<PimsCostTypeCode>
    {
        IList<PimsCostTypeCode> GetAllCostTypeCodes();
    }
}
