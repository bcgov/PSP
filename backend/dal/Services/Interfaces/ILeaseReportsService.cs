using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface ILeaseReportsService
    {
        IEnumerable<PimsLease> GetAggregatedLeaseReport(int fiscalYearStart);
    }
}
