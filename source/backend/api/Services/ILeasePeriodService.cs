using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Api.Services
{
    public interface ILeasePeriodService
    {
        IEnumerable<PimsLeasePeriod> GetPeriods(long leaseId);

        PimsLeasePeriod AddPeriod(long leaseId, PimsLeasePeriod period);

        PimsLeasePeriod UpdatePeriod(long leaseId, long periodId, PimsLeasePeriod period);

        bool DeletePeriod(long leaseId, PimsLeasePeriod period);
    }
}
