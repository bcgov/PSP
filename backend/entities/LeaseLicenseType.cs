using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    [MotiTable("PIMS_LEASE_LICENSE_TYPE", "LSLITYPE")]
    public class LeaseLicenseType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [Column("LEASE_LICENSE_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        public LeaseLicenseType() { }

        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseLicenseType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
