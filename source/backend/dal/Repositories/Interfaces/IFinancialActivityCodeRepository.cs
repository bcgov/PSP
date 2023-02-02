using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IFinancialActivityCodeRepository : IRepository<PimsFinancialActivityCode>
    {
        IList<PimsFinancialActivityCode> GetAllFinancialActivityCodes();

        PimsFinancialActivityCode GetById(long id);

        PimsFinancialActivityCode Add(PimsFinancialActivityCode pimsCode);

        PimsFinancialActivityCode Update(PimsFinancialActivityCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsFinancialActivityCode pimsCode);
    }
}
