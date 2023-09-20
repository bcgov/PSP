using System;
using System.Linq;
using NetTopologySuite.Geometries;
using Pims.Dal;
using Pims.Dal.Constants;
using Pims.Dal.Entities;
using Pims.Dal.Helpers;
using Entity = Pims.Dal.Entities;

namespace Pims.Core.Test
{
    /// <summary>
    /// EntityHelper static class, provides helper methods to create test entities.
    /// </summary>
    public static partial class EntityHelper
    {
        /// <summary>
        /// Create a new instance of a Lease.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsLease CreateLease(int pid, string lFileNo = null, string tenantFirstName = null, string tenantLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null, bool addTenant = false, bool addProperty = true,
            PimsLeaseProgramType pimsLeaseProgramType = null, PimsLeasePurposeType pimsLeasePurposeType = null, PimsLeaseStatusType pimsLeaseStatusType = null, PimsLeasePayRvblType pimsLeasePayRvblType = null, PimsLeaseCategoryType pimsLeaseCategoryType = null, PimsLeaseInitiatorType pimsLeaseInitiatorType = null, PimsLeaseResponsibilityType pimsLeaseResponsibilityType = null, PimsLeaseLicenseType pimsLeaseLicenseType = null, PimsRegion region = null)
        {
            var lease = new Entity.PimsLease()
            {
                LeaseId = pid,
                LFileNo = lFileNo,
                ConcurrencyControlNumber = 1,
            };
            var person = new Entity.PimsPerson() { FirstName = tenantFirstName, Surname = tenantLastName };
            person.PimsPersonAddresses.Add(new PimsPersonAddress() { Person = person, Address = address });
            var organization = new Entity.PimsOrganization();
            organization.PimsOrganizationAddresses.Add(new PimsOrganizationAddress() { Organization = organization, Address = address });
            if (addProperty)
            {
                lease.PimsPropertyLeases.Add(new PimsPropertyLease()
                {
                    Property = new Entity.PimsProperty()
                    {
                        Pid = pid,
                        Location = GeometryHelper.CreatePoint(0, 0, SpatialReference.BCALBERS),
                    },
                    Lease = lease,
                });
            }
            lease.MotiContact = $"{motiFirstName} {motiLastName}";
            lease.LeaseProgramTypeCodeNavigation = pimsLeaseProgramType ?? new Entity.PimsLeaseProgramType() { Id = "testProgramType" };
            lease.LeaseProgramTypeCode = "testProgramType";
            lease.LeasePurposeTypeCodeNavigation = pimsLeasePurposeType ?? new Entity.PimsLeasePurposeType() { Id = "testPurposeType" };
            lease.LeasePurposeTypeCode = "testPurposeType";
            lease.LeaseStatusTypeCodeNavigation = pimsLeaseStatusType ?? new Entity.PimsLeaseStatusType() { Id = "testStatusType" };
            lease.LeasePayRvblTypeCodeNavigation = pimsLeasePayRvblType ?? new Entity.PimsLeasePayRvblType() { Id = "testRvblType" };
            lease.LeaseCategoryTypeCodeNavigation = pimsLeaseCategoryType ?? new Entity.PimsLeaseCategoryType() { Id = "testCategoryType" };
            lease.LeaseInitiatorTypeCodeNavigation = pimsLeaseInitiatorType ?? new Entity.PimsLeaseInitiatorType() { Id = "testInitiatorType" };
            lease.LeaseResponsibilityTypeCodeNavigation = pimsLeaseResponsibilityType ?? new Entity.PimsLeaseResponsibilityType() { Id = "testResponsibilityType" };
            lease.LeaseLicenseTypeCodeNavigation = pimsLeaseLicenseType ?? new Entity.PimsLeaseLicenseType() { Id = "testType" };
            if (region != null)
            {
                lease.RegionCodeNavigation = region;
            }
            if (addTenant)
            {
                lease.PimsLeaseTenants.Add(new PimsLeaseTenant(lease, person, organization, new PimsLessorType("tst")));
            }
            return lease;
        }

        /// <summary>
        /// Create a new instance of a Lease.
        /// </summary>
        /// <returns></returns>
        public static Entity.PimsLease CreateLease(this PimsContext context, int pid, string lFileNo = null, string tenantFirstName = null, string tenantLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null, bool addTenant = false, bool addProperty = true)
        {
            var programType = context.PimsLeaseProgramTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leasePurposeType = context.PimsLeasePurposeTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease purpose type.");
            var leaseStatusType = context.PimsLeaseStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease status type.");
            var leasePayRvblType = context.PimsLeasePayRvblTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease rvbl type.");
            var leaseCategoryType = context.PimsLeaseCategoryTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease category type.");
            var leaseInitiatorType = context.PimsLeaseInitiatorTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease initiator type.");
            var leaseResponsibilityType = context.PimsLeaseResponsibilityTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease reponsibility type.");
            var leaseLicenseType = context.PimsLeaseLicenseTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease license type.");

            var lease = EntityHelper.CreateLease(pid, lFileNo, tenantFirstName, tenantLastName, motiFirstName, motiLastName, address, addTenant, addProperty, programType, leasePurposeType, leaseStatusType, leasePayRvblType, leaseCategoryType, leaseInitiatorType, leaseResponsibilityType, leaseLicenseType);
            context.PimsLeases.Add(lease);
            return lease;
        }
    }
}
