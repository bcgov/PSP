using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IYearlyFinancialCodeRepository : IRepository<PimsYearlyFinancialCode>
    {
        IList<PimsYearlyFinancialCode> GetAllYearlyFinancialCodes();

        PimsYearlyFinancialCode Add(PimsYearlyFinancialCode pimsCode);

        PimsYearlyFinancialCode Update(PimsYearlyFinancialCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsYearlyFinancialCode pimsCode);
    }
}
