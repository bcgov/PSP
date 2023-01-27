using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public interface ILeaseTermService
    {
        PimsLease AddTerm(long leaseId, long leaseRowVersion, PimsLeaseTerm term);

        PimsLease UpdateTerm(long leaseId, long termId, long leaseRowVersion, PimsLeaseTerm term);

        PimsLease DeleteTerm(long leaseId, long leaseRowVersion, PimsLeaseTerm term);
    }
}
