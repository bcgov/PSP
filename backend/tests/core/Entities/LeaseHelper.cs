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
        /// Create a new instance of a Property.
        /// </summary>
        /// <param name="pidOrPin"></param>
        /// <param name="lFileNo"></param>
        /// <param name="tenantName"></param>
        /// <returns></returns>
        public static Entity.PimsLease CreateLease(int pidOrPin, string lFileNo = null, string tenantFirstName = null, string tenantLastName = null, string motiFirstName = null, string motiLastName = null, PimsAddress address = null)
        {
            var lease = new Entity.PimsLease()
            {
                LeaseId = pidOrPin,
                LFileNo = lFileNo,
                ConcurrencyControlNumber = 1,
            };
            var person = new Entity.PimsPerson() { FirstName = tenantFirstName, Surname = tenantLastName };
            person.PimsPersonAddresses.Add(new PimsPersonAddress() { Person = person, Address = address });
            var organization = new Entity.PimsOrganization();
            organization.PimsOrganizationAddresses.Add(new PimsOrganizationAddress() { Organization = organization, Address = address });
            lease.PimsPropertyLeases.Add(new PimsPropertyLease() { Property = new Entity.PimsProperty() { Pid = pidOrPin }, Lease = lease });
            lease.MotiContact = $"{motiFirstName} {motiLastName}";
            lease.LeaseProgramTypeCodeNavigation = new Entity.PimsLeaseProgramType() { Id = "testProgramType" };
            lease.LeasePmtFreqTypeCodeNavigation = new Entity.PimsLeasePmtFreqType() { Id = "testFrequencyType" };
            lease.PimsLeaseTenants.Add(new PimsLeaseTenant(lease, person, organization, new PimsLessorType("tst")));
            return lease;
        }
    }
}
