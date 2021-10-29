using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePaymentRvblType class, provides an entity for the datamodel to manage a list of lease payment rvbl types.
    /// </summary>
    [MotiTable("PIMS_LEASE_PAY_RVBL_TYPE", "LSPRTY")]
    public class LeasePaymentReceivableType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease payment rvbl type.
        /// </summary>
        [Column("LEASE_PAY_RVBL_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeasePaymentRvblType class.
        /// </summary>
        public LeasePaymentReceivableType() { }

        /// <summary>
        /// Create a new instance of a LeasePaymentRvblType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeasePaymentReceivableType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
