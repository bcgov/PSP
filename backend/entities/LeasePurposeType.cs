using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeasePurposeType class, provides an entity for the datamodel to manage a list of lease purpose types.
    /// </summary>
    [MotiTable("PIMS_LEASE_PURPOSE_TYPE", "LSPRTY")]
    public class LeasePurposeType : TypeEntity<int>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease purpose type.
        /// </summary>
        [Column("LEASE_PURPOSE_TYPE_CODE")]
        public override int Id { get; set; }

        /// <summary>
        /// get - Collection of leases.
        /// </summary>
        public ICollection<Lease> Leases { get; } = new List<Lease>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeasePurposeType class.
        /// </summary>
        public LeasePurposeType() { }

        /// <summary>
        /// Create a new instance of a LeasePurposeType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeasePurposeType(int id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
