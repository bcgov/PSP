using System.Collections.Generic;
using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ILeaseTermRepository : IRepository
    {
        void Delete(long leaseTermId);

        PimsLeaseTerm Update(PimsLeaseTerm pimsLeaseTerm);

        PimsLeaseTerm Add(PimsLeaseTerm pimsLeaseTerm);

        IEnumerable<PimsLeaseTerm> GetAllByLeaseId(long leaseId);

        PimsLeaseTerm GetById(long leaseTermId, bool loadPayments = false);
    }
}
