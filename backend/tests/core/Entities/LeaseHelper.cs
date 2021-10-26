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
        public static Entity.Lease CreateLease(int pidOrPin, string lFileNo = null, string tenantFirstName = null, string tenantLastName = null, string motiFirstName = null, string motiLastName = null, Address address = null)
        {
            var lease = new Entity.Lease()
            {
                Id = 1,
                LFileNo = lFileNo,
                RowVersion = 1,
            };
            var person = new Entity.Person() { FirstName = tenantFirstName, Surname = tenantLastName, Address = address };
            var organization = new Entity.Organization() { Address = address };
            lease.Properties.Add(new Entity.Property() { PID = pidOrPin });
            lease.MotiName = new Entity.Person() { FirstName = motiFirstName, Surname = motiLastName };
            lease.ProgramType = new Entity.LeaseProgramType() { Id = "testProgramType" };
            lease.PaymentFrequencyType = new Entity.LeasePaymentFrequencyType() { Id = "testFrequencyType" };
            lease.TenantsManyToMany.Add(new LeaseTenant(lease, person, organization, new LessorType("tst", "")));
            return lease;
        }
    }
}
