using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ILeasePeriodRepository : IRepository
    {
        void Delete(long leasePeriodId);

        PimsLeasePeriod Update(PimsLeasePeriod pimsLeasePeriod);

        PimsLeasePeriod Add(PimsLeasePeriod pimsLeasePeriod);

        IEnumerable<PimsLeasePeriod> GetAllByLeaseId(long leaseId);

        PimsLeasePeriod GetById(long leasePeriodId, bool loadPayments = false);
    }
}
