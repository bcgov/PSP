namespace Pims.Dal.Services
{
    public class LeaseService : ILeaseService
    {
        readonly IPimsRepository _pimsRepository;
        public LeaseService(IPimsRepository pimsRepository)
        {
            _pimsRepository = pimsRepository;
        }
        public bool IsRowVersionEqual(long leaseId, long rowVersion)
        {
            long currentRowVersion = _pimsRepository.Lease.GetRowVersion(leaseId);
            return currentRowVersion == rowVersion;
        }
    }
}
