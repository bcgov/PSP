using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseInitiatorType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    [MotiTable("PIMS_LEASE_INITIATOR_TYPE", "LINNIT")]
    public class LeaseIntiatorType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease initiator type.
        /// </summary>
        [Column("LEASE_INITIATOR_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseInitiatorType class.
        /// </summary>
        public LeaseIntiatorType() { }

        /// <summary>
        /// Create a new instance of a LeaseInitiatorType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseIntiatorType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
