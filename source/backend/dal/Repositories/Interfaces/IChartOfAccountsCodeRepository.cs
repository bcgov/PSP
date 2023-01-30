using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface IChartOfAccountsCodeRepository : IRepository<PimsChartOfAccountsCode>
    {
        IList<PimsChartOfAccountsCode> GetAllChartOfAccountCodes();

        PimsChartOfAccountsCode Add(PimsChartOfAccountsCode pimsCode);

    }
}
