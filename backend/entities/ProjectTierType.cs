using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectTierType class, provides an entity for the datamodel to manage a list of project tier types.
    /// </summary>
    [MotiTable("PIMS_PROJECT_TIER_TYPE", "PROJTR")]
    public class ProjectTierType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project tier type.
        /// </summary>
        [Column("PROJECT_TIER_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectTierType class.
        /// </summary>
        public ProjectTierType() { }

        /// <summary>
        /// Create a new instance of a ProjectTierType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public ProjectTierType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
