using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// LeaseType class, provides an entity for the datamodel to manage a list of lease types.
    /// </summary>
    [MotiTable("PIMS_LEASE_TYPE", "LSTYPE")]
    public class LeaseType : TypeEntity<int>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify lease type.
        /// </summary>
        [Column("LEASE_TYPE_CODE")]
        public override int Id { get; set; }

        /// <summary>
        /// get - Collection of lease activities.
        /// </summary>
        public ICollection<LeaseActivity> Activities { get; } = new List<LeaseActivity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        public LeaseType() { }

        /// <summary>
        /// Create a new instance of a LeaseType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public LeaseType(int id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
