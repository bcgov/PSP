using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseSubtype class, provides an entity for the datamodel to manage a list of lease subtypes.
    /// </summary>
    [MotiTable("PIMS_LEASE_SUBTYPE", "LSSTYP")]
    public class LeaseSubtype : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease subtype.
        /// </summary>
        [Column("LEASE_SUBTYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of lease activities.
        /// </summary>
        public ICollection<LeaseActivity> Activities { get; } = new List<LeaseActivity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseSubtype class.
        /// </summary>
        public LeaseSubtype() { }

        /// <summary>
        /// Create a new instance of a LeaseSubtype class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseSubtype(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
