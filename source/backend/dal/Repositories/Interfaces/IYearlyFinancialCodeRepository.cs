using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IYearlyFinancialCodeRepository : IRepository<PimsYearlyFinancialCode>
    {
        IList<PimsYearlyFinancialCode> GetAllYearlyFinancialCodes();

        PimsYearlyFinancialCode GetById(long id);

        PimsYearlyFinancialCode Add(PimsYearlyFinancialCode pimsCode);

        PimsYearlyFinancialCode Update(PimsYearlyFinancialCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsYearlyFinancialCode pimsCode);
    }
}
