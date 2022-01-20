using Pims.Dal.Entities;
using System.Collections.Generic;

namespace Pims.Dal.Repositories
{
    public interface ILeaseTermRepository : IRepository
    {
        void Delete(long leaseTermId);
        PimsLeaseTerm Update(PimsLeaseTerm pimsLeaseTerm);
        PimsLeaseTerm Add(PimsLeaseTerm pimsLeaseTerm);
        IEnumerable<PimsLeaseTerm> GetByLeaseId(long leaseId);
        PimsLeaseTerm GetById(long leaseTermId, bool loadPayments = false);
    }
}
