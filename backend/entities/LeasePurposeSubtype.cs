using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePurposeSubtype class, provides an entity for the datamodel to manage a list of lease purpose subtypes.
    /// </summary>
    [MotiTable("PIMS_LEASE_PURPOSE_SUBTYPE", "LSPRST")]
    public class LeasePurposeSubtype : TypeEntity<int>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease purpose subtype.
        /// </summary>
        [Column("LEASE_PURPOSE_SUBTYPE_CODE")]
        public override int Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeasePurposeSubtype class.
        /// </summary>
        public LeasePurposeSubtype() { }

        /// <summary>
        /// Create a new instance of a LeasePurposeSubtype class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeasePurposeSubtype(int id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
