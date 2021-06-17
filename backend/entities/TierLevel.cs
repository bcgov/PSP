using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// TierLevel class, provides an entity for the datamodel to manage a list of project tier levels.
    /// </summary>
    [MotiTable("PIMS_TIER_LEVEL", "TRLEVL")]
    public class TierLevel : LookupEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify tier level.
        /// </summary>
        [Column("TIER_LEVEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A description of the tier.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get - Collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a TierLevel class.
        /// </summary>
        public TierLevel() { }

        /// <summary>
        /// Create a new instance of a TierLevel class.
        /// </summary>
        /// <param name="name"></param>
        public TierLevel(string name) : base(name)
        {
        }
        #endregion
    }
}
