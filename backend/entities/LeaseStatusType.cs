using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseStatusType class, provides an entity for the datamodel to manage a list of lease status types.
    /// </summary>
    [MotiTable("PIMS_LEASE_STATUS_TYPE", "LSSTSY")]
    public class LeaseStatusType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease status type.
        /// </summary>
        [Column("LEASE_STATUS_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseStatusType class.
        /// </summary>
        public LeaseStatusType() { }

        /// <summary>
        /// Create a new instance of a LeaseStatusType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseStatusType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
