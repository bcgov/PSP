using System;
using System.Linq;
using Pims.Dal;
using Pims.Dal.Entities;
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
        public static PimsLease CreateLease(int pid, string lFileNo = null, string stakeholderFirstName = null, string stakeholderLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null, bool addStakeholder = false, bool addProperty = true,
            PimsLeaseProgramType pimsLeaseProgramType = null, PimsLeaseStatusType pimsLeaseStatusType = null, PimsLeasePayRvblType pimsLeasePayRvblType = null, PimsLeaseInitiatorType pimsLeaseInitiatorType = null, PimsLeaseResponsibilityType pimsLeaseResponsibilityType = null, PimsLeaseLicenseType pimsLeaseLicenseType = null, PimsRegion region = null, bool generateTypeIds = false)
        {
            var lease = new PimsLease()
            {
                LeaseId = pid,
                LFileNo = lFileNo,
                ConcurrencyControlNumber = 1,
            };
            var person = new PimsPerson() { FirstName = stakeholderFirstName ?? "first", Surname = stakeholderLastName ?? "last" };
            person.PimsPersonAddresses.Add(new PimsPersonAddress() { Person = person, Address = address, AddressUsageTypeCode = AddressUsageTypes.Mailing });
            var organization = new PimsOrganization() { OrganizationName = "Lease organization" };
            organization.PimsOrganizationAddresses.Add(new PimsOrganizationAddress() { Organization = organization, Address = address, AddressUsageTypeCode = AddressUsageTypes.Mailing });
            if (addProperty)
            {
                lease.PimsPropertyLeases.Add(new PimsPropertyLease()
                {
                    Property = CreateProperty(pid: pid),
                    Lease = lease,
                });
            }
            lease.MotiContact = $"{motiFirstName} {motiLastName}";
            lease.LeaseProgramTypeCodeNavigation = pimsLeaseProgramType ?? new PimsLeaseProgramType() { Id = generateTypeIds ? Guid.NewGuid().ToString() : "testProgramType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseProgramTypeCode = "testProgramType";
            lease.LeaseStatusTypeCode = "testStatusType";
            lease.LeaseStatusTypeCodeNavigation = pimsLeaseStatusType ?? new PimsLeaseStatusType() { Id = generateTypeIds ? Guid.NewGuid().ToString() : "testStatusType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeasePayRvblTypeCodeNavigation = pimsLeasePayRvblType ?? new PimsLeasePayRvblType() { Id = generateTypeIds ? Guid.NewGuid().ToString() : "testRvblType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseInitiatorTypeCodeNavigation = pimsLeaseInitiatorType ?? new PimsLeaseInitiatorType() { Id = generateTypeIds ? Guid.NewGuid().ToString() : "testInitiatorType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseResponsibilityTypeCodeNavigation = pimsLeaseResponsibilityType ?? new PimsLeaseResponsibilityType() { Id = generateTypeIds ? Guid.NewGuid().ToString() : "testResponsibilityType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseLicenseTypeCodeNavigation = pimsLeaseLicenseType ?? new PimsLeaseLicenseType() { Id = generateTypeIds ? Guid.NewGuid().ToString() : "testType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            if (region != null)
            {
                lease.RegionCodeNavigation = region;
            }
            if (addStakeholder)
            {
                lease.PimsLeaseStakeholders.Add(new PimsLeaseStakeholder(lease, person, organization, new PimsLessorType("tst") { DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }, new PimsLeaseStakeholderType("TENANT") { DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" }));
            }
            return lease;
        }

        /// <summary>
        /// Create a new instance of a Lease.
        /// </summary>
        /// <returns></returns>
        public static PimsLease CreateLease(this PimsContext context, int pid, string lFileNo = null, string stakeholderFirstName = null, string stakeholderLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null, bool addStakeholder = false, bool addProperty = true)
        {
            var programType = context.PimsLeaseProgramTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leaseStatusType = context.PimsLeaseStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease status type.");
            var leasePayRvblType = context.PimsLeasePayRvblTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease rvbl type.");
            var leaseInitiatorType = context.PimsLeaseInitiatorTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease initiator type.");
            var leaseResponsibilityType = context.PimsLeaseResponsibilityTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease reponsibility type.");
            var leaseLicenseType = context.PimsLeaseLicenseTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease license type.");

            var lease = CreateLease(pid, lFileNo, stakeholderFirstName, stakeholderLastName, motiFirstName, motiLastName, address, addStakeholder, addProperty, programType, leaseStatusType, leasePayRvblType, leaseInitiatorType, leaseResponsibilityType, leaseLicenseType);
            context.PimsLeases.Add(lease);

            return lease;
        }
    }
}
