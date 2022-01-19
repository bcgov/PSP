using Pims.Dal.Entities;

namespace Pims.Dal.Repositories
{
    public interface ILeaseRepository : IRepository<PimsLeaseTerm>
    {
        PimsLeaseTerm UpdateTerm(PimsLeaseTerm term);
        PimsLeaseTerm DeleteTerm(long leaseTermId);
    }
}
