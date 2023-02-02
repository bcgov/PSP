using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IChartOfAccountsCodeRepository : IRepository<PimsChartOfAccountsCode>
    {
        IList<PimsChartOfAccountsCode> GetAllChartOfAccountCodes();

        PimsChartOfAccountsCode GetById(long id);

        PimsChartOfAccountsCode Add(PimsChartOfAccountsCode pimsCode);

        PimsChartOfAccountsCode Update(PimsChartOfAccountsCode pimsCode);

        long GetRowVersion(long id);

        bool IsDuplicate(PimsChartOfAccountsCode pimsCode);
    }
}
