using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseCategoryType class, provides an entity for the datamodel to manage a list of lease category types.
    /// </summary>
    [MotiTable("PIMS_LEASE_CATEGORY_TYPE", "LSCATYPE")]
    public class LeaseCategoryType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease category type.
        /// </summary>
        [Column("LEASE_CATEGORY_TYPE_CODE")]
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
        public LeaseCategoryType() { }

        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseCategoryType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
