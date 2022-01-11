using Pims.Dal;
using Pims.Dal.Entities;
using System;
using System.Linq;
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
            PimsLeaseProgramType pimsLeaseProgramType = null, PimsLeasePmtFreqType pimsLeasePmtFreqType = null, PimsLeasePurposeType pimsLeasePurposeType = null, PimsLeaseStatusType pimsLeaseStatusType = null, PimsLeasePayRvblType pimsLeasePayRvblType = null, PimsLeaseCategoryType pimsLeaseCategoryType = null, PimsLeaseInitiatorType pimsLeaseInitiatorType = null, PimsLeaseResponsibilityType pimsLeaseResponsibilityType = null, PimsLeaseLicenseType pimsLeaseLicenseType = null)
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
                lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new Entity.PimsProperty() { Pid = pid }, Lease = lease });
            }
            lease.MotiContact = $"{motiFirstName} {motiLastName}";
            lease.LeaseProgramTypeCodeNavigation = pimsLeaseProgramType ?? new Entity.PimsLeaseProgramType() { Id = "testProgramType" };
            lease.LeasePmtFreqTypeCodeNavigation = pimsLeasePmtFreqType ?? new Entity.PimsLeasePmtFreqType() { Id = "testFrequencyType" };
            lease.LeasePurposeTypeCodeNavigation = pimsLeasePurposeType ?? new Entity.PimsLeasePurposeType() { Id = "testPurposeType" };
            lease.LeaseStatusTypeCodeNavigation = pimsLeaseStatusType ?? new Entity.PimsLeaseStatusType() { Id = "testStatusType" };
            lease.LeasePayRvblTypeCodeNavigation = pimsLeasePayRvblType ?? new Entity.PimsLeasePayRvblType() { Id = "testRvblType" };
            lease.LeaseCategoryTypeCodeNavigation = pimsLeaseCategoryType ?? new Entity.PimsLeaseCategoryType() { Id = "testCategoryType" };
            lease.LeaseInitiatorTypeCodeNavigation = pimsLeaseInitiatorType ?? new Entity.PimsLeaseInitiatorType() { Id = "testInitiatorType" };
            lease.LeaseResponsibilityTypeCodeNavigation = pimsLeaseResponsibilityType ?? new Entity.PimsLeaseResponsibilityType() { Id = "testResponsibilityType" };
            lease.LeaseLicenseTypeCodeNavigation = pimsLeaseLicenseType ?? new Entity.PimsLeaseLicenseType() { Id = "testType" };
            if(addTenant)
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
            var paymentFrequencyType = context.PimsLeasePmtFreqTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leasePurposeType = context.PimsLeasePurposeTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leaseStatusType = context.PimsLeaseStatusTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leasePayRvblType = context.PimsLeasePayRvblTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leaseCategoryType = context.PimsLeaseCategoryTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leaseInitiatorType = context.PimsLeaseInitiatorTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leaseResponsibilityType = context.PimsLeaseResponsibilityTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            var leaseLicenseType = context.PimsLeaseLicenseTypes.FirstOrDefault() ?? throw new InvalidOperationException("Unable to find lease program type.");
            
            var lease = EntityHelper.CreateLease(pid, lFileNo, tenantFirstName, tenantLastName, motiFirstName, motiLastName, address, addTenant, addProperty, programType, paymentFrequencyType, leasePurposeType, leaseStatusType, leasePayRvblType, leaseCategoryType, leaseInitiatorType, leaseResponsibilityType, leaseLicenseType);
            context.PimsLeases.Add(lease);
            return lease;
        }
    }
}
