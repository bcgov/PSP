using Pims.Dal.Repositories;
using Pims.Dal.Entities;

namespace Pims.Dal.Services
{
    public class LeaseService : ILeaseService
    {
        readonly ILeaseRepository _leaseRepository;
        public LeaseService(ILeaseRepository leaseRepository)
        {
            _leaseRepository = leaseRepository;
        }
        public bool IsRowVersionEqual(long leaseId, long rowVersion)
        {
            long currentRowVersion = _leaseRepository.GetRowVersion(leaseId);
            return currentRowVersion == rowVersion;
        }

        public PimsLease GetById(long leaseId)
        {
            return _leaseRepository.Get(leaseId);
        }
    }
}
