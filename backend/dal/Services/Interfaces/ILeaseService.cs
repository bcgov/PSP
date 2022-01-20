namespace Pims.Dal.Services
{
    public interface ILeaseService
    {
        bool IsRowVersionEqual(long leaseId, long rowVersion);
        PimsLease Update(PimsLease lease, bool commitTransaction = true);
        PimsLease UpdateLeaseTenants(long leaseId, long rowVersion, ICollection<PimsLeaseTenant> pimsLeaseTenants);
        PimsLease UpdateLeaseImprovements(long leaseId, long rowVersion, ICollection<PimsPropertyImprovement> pimsPropertyImprovements);
        PimsLease UpdatePropertyLeases(long leaseId, long rowVersion, ICollection<PimsPropertyLease> pimsPropertyLeases, bool userOverride = false);
    }
}
