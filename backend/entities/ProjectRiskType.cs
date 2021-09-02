using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectRiskType class, provides an entity for the datamodel to manage a list of project risk types.
    /// </summary>
    [MotiTable("PIMS_PROJECT_RISK_TYPE", "PRJRSK")]
    public class ProjectRiskType : TypeEntity<string>
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project risk type.
        /// </summary>
        [Column("PROJECT_RISK_TYPE_CODE")]
        public override string Id { get; set; }

        /// <summary>
        /// get - Collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectRiskType class.
        /// </summary>
        public ProjectRiskType() { }

        /// <summary>
        /// Create a new instance of a ProjectRiskType class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="description"></param>
        public ProjectRiskType(string id, string description) : base(id, description)
        {
        }
        #endregion
    }
}
