using System.Collections.Generic;
using Pims.Dal.Entities;
using Pims.Dal.Entities.Models;
using Pims.Dal.Exceptions;

namespace Pims.Api.Services
{
    public interface ILeaseService
    {
        bool IsRowVersionEqual(long leaseId, long rowVersion);

        PimsLease GetById(long leaseId);

        Paged<PimsLease> GetPage(LeaseFilter filter, bool? all = false);

        PimsLease Add(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides);

        PimsLease Update(PimsLease lease, IEnumerable<UserOverrideCode> userOverrides);

        IEnumerable<PimsPropertyLease> GetPropertiesByLeaseId(long leaseId);

        IEnumerable<PimsInsurance> GetInsuranceByLeaseId(long leaseId);

        IEnumerable<PimsInsurance> UpdateInsuranceByLeaseId(long leaseId, IEnumerable<PimsInsurance> pimsInsurances);

        IEnumerable<PimsPropertyImprovement> GetImprovementsByLeaseId(long leaseId);

        IEnumerable<PimsPropertyImprovement> UpdateImprovementsByLeaseId(long leaseId, IEnumerable<PimsPropertyImprovement> pimsPropertyImprovements);

        IEnumerable<PimsLeaseTenant> GetTenantsByLeaseId(long leaseId);

        IEnumerable<PimsLeaseTenant> UpdateTenantsByLeaseId(long leaseId, IEnumerable<PimsLeaseTenant> pimsLeaseTenants);
    }
}
