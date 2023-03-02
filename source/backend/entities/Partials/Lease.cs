using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Lease class, provides an entity for the datamodel to manage leases.
    /// </summary>
    public partial class PimsLease : StandardIdentityBaseAppEntity<long>, IBaseAppEntity
    {
        #region Properties
        [NotMapped]
        public override long Internal_Id { get => this.LeaseId; set => this.LeaseId = value; }

        /// <summary>
        /// get/set - A collection of properties.
        /// </summary>
        public ICollection<PimsProperty> GetProperties() => PimsPropertyLeases?.Select(pl => pl.Property).ToArray();

        /// <summary>
        /// get/set - A collection of Persons associated to this Lease.
        /// </summary>
        public ICollection<PimsPerson> GetPersons() => PimsLeaseTenants?.Where(lt => lt.Person != null).Select(lt => lt.Person).ToArray();

        /// <summary>
        /// get/set - A collection of Organizations associated to this Lease.
        /// </summary>
        public ICollection<PimsOrganization> GetOrganizations() => PimsLeaseTenants?.Where(lt => lt.Person == null && lt.Organization != null).Select(lt => lt.Organization).ToArray();

        /// <summary>
        /// get/set - A collection of Improvements associated to this Lease.
        /// </summary>
        public ICollection<PimsPropertyImprovement> GetImprovements() => PimsPropertyImprovements;
        #endregion
    }
}
