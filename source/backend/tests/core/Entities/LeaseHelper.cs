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
        public static Entity.PimsLease CreateLease(int pid, string lFileNo = null, string stakeholderFirstName = null, string stakeholserLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null, bool addStakeholder = false, bool addProperty = true,
            PimsLeaseProgramType pimsLeaseProgramType = null, PimsLeasePurposeType pimsLeasePurposeType = null, PimsLeaseStatusType pimsLeaseStatusType = null, PimsLeasePayRvblType pimsLeasePayRvblType = null, PimsLeaseInitiatorType pimsLeaseInitiatorType = null, PimsLeaseResponsibilityType pimsLeaseResponsibilityType = null, PimsLeaseLicenseType pimsLeaseLicenseType = null, PimsRegion region = null)
        {
            var lease = new Entity.PimsLease()
            {
                LeaseId = pid,
                LFileNo = lFileNo,
                ConcurrencyControlNumber = 1,
            };
            var person = new Entity.PimsPerson() { FirstName = stakeholderFirstName ?? "first", Surname = stakeholserLastName ?? "last" };
            person.PimsPersonAddresses.Add(new PimsPersonAddress() { Person = person, Address = address, AddressUsageTypeCode = AddressUsageTypes.Mailing });
            var organization = new Entity.PimsOrganization() { OrganizationName = "Lease organization" };
            organization.PimsOrganizationAddresses.Add(new PimsOrganizationAddress() { Organization = organization, Address = address, AddressUsageTypeCode = AddressUsageTypes.Mailing });
            if (addProperty)
            {
                lease.PimsPropertyLeases.Add(new PimsPropertyLease()
                {
                    Property = EntityHelper.CreateProperty(pid: pid),
                    Lease = lease,
                });
            }
            lease.MotiContact = $"{motiFirstName} {motiLastName}";
            lease.LeaseProgramTypeCodeNavigation = pimsLeaseProgramType ?? new Entity.PimsLeaseProgramType() { Id = "testProgramType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            //lease.LeaseProgramTypeCode = "testProgramType"; TODO: Fix Mappings
            //lease.LeasePurposeTypeCodeNavigation = pimsLeasePurposeType ?? new Entity.PimsLeasePurposeType() { Id = "testPurposeType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            //lease.LeasePurposeTypeCode = "testPurposeType";
            lease.LeaseStatusTypeCode = "testStatusType";
            lease.LeaseStatusTypeCodeNavigation = pimsLeaseStatusType ?? new Entity.PimsLeaseStatusType() { Id = "testStatusType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeasePayRvblTypeCodeNavigation = pimsLeasePayRvblType ?? new Entity.PimsLeasePayRvblType() { Id = "testRvblType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            //lease.LeaseCategoryTypeCodeNavigation = pimsLeaseCategoryType ?? new Entity.PimsLeaseCategoryType() { Id = "testCategoryType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseInitiatorTypeCodeNavigation = pimsLeaseInitiatorType ?? new Entity.PimsLeaseInitiatorType() { Id = "testInitiatorType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseResponsibilityTypeCodeNavigation = pimsLeaseResponsibilityType ?? new Entity.PimsLeaseResponsibilityType() { Id = "testResponsibilityType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
            lease.LeaseLicenseTypeCodeNavigation = pimsLeaseLicenseType ?? new Entity.PimsLeaseLicenseType() { Id = "testType", DbCreateUserid = "test", DbLastUpdateUserid = "test", Description = "desc" };
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
        public static Entity.PimsLease CreateLease(this PimsContext context, int pid, string lFileNo = null, string stakeholderFirstName = null, string stakeholderLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null, bool addStakeholder = false, bool addProperty = true)
        {
            var programType = context.PimsLeaseProgramTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leasePurposeType = context.PimsLeasePurposeTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease purpose type.");
            var leaseStatusType = context.PimsLeaseStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease status type.");
            var leasePayRvblType = context.PimsLeasePayRvblTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease rvbl type.");
            //var leaseCategoryType = context.PimsLeaseCategoryTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease category type.");
                        // TODO: Fix Mappings
            var leaseInitiatorType = context.PimsLeaseInitiatorTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease initiator type.");
            var leaseResponsibilityType = context.PimsLeaseResponsibilityTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease reponsibility type.");
            var leaseLicenseType = context.PimsLeaseLicenseTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease license type.");

            var lease = EntityHelper.CreateLease(pid, lFileNo, stakeholderFirstName, stakeholderLastName, motiFirstName, motiLastName, address, addStakeholder, addProperty, programType, leasePurposeType, leaseStatusType, leasePayRvblType, leaseInitiatorType, leaseResponsibilityType, leaseLicenseType);
            context.PimsLeases.Add(lease);
            return lease;
        }
    }
}
