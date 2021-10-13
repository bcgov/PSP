using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LessorType class, provides an entity for the datamodel to manage a list of lessor types.
    /// </summary>
    [MotiTable("PIMS_LESSOR_TYPE", "LSSRTYPE")]
    public class LessorType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lessor type.
        /// </summary>
        [Column("LESSOR_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of lease tenants.
        /// </summary>
        public ICollection<LeaseTenant> Leases { get; } = new List<LeaseTenant>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LessorType class.
        /// </summary>
        public LessorType() { }

        /// <summary>
        /// Create a new instance of a LessorType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LessorType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
