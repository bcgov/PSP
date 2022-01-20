namespace Pims.Dal.Services
{
    public class LeaseService : ILeaseService
    {
        readonly Repositories.ILeaseService _leaseRepository;
        public LeaseService(Repositories.ILeaseService leaseRepository)
        {
            _leaseRepository = leaseRepository;
        }
        public bool IsRowVersionEqual(long leaseId, long rowVersion)
        {
            long currentRowVersion = _leaseRepository.GetRowVersion(leaseId);
            return currentRowVersion == rowVersion;
        }
    }
}
