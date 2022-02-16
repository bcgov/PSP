using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Services
{
    public interface ILeaseReportsService
    {
        IEnumerable<PimsLease> GetAggregatedLeaseReport(int fiscalYearStart);
    }
}
