using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseResponsibilityType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    [MotiTable("PIMS_LEASE_RESPONSIBILITY_TYPE", "LRESPT")]
    public class LeaseResponsibilityType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [Column("LEASE_RESPONSIBILITY_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseResponsibilityType class.
        /// </summary>
        public LeaseResponsibilityType() { }

        /// <summary>
        /// Create a new instance of a LeaseResponsibilityType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseResponsibilityType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
