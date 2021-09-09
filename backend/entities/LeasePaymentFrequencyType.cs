using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePaymentFrequencyType class, provides an entity for the datamodel to manage a list of lease payment frequency types.
    /// </summary>
    [MotiTable("PIMS_LEASE_PMT_FREQ_TYPE", "LSPMTF")]
    public class LeasePaymentFrequencyType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease payment frequency type.
        /// </summary>
        [Column("LEASE_PMT_FREQ_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeasePaymentFrequencyType class.
        /// </summary>
        public LeasePaymentFrequencyType() { }

        /// <summary>
        /// Create a new instance of a LeasePaymentFrequencyType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeasePaymentFrequencyType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
