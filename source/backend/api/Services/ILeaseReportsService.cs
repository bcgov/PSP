using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ILeaseReportsService
    {
        IEnumerable<PimsLease> GetAggregatedLeaseReport(int fiscalYearStart);
    }
}
