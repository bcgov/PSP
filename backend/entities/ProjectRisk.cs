using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectRisk class, provides an entity for the datamodel to manage project risks.
    /// </summary>
    [MotiTable("PIMS_PROJECT_RISK", "PRJRSK")]
    public class ProjectRisk : CodeEntity
    {
        #region Properties
        /// <summary>
        /// get/set -
        /// </summary>
        [Column("PROJECT_RISK_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A description of the risk.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get - A collection of notes for this project.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectRisk class.
        /// </summary>
        public ProjectRisk() { }

        /// <summary>
        /// Create a new instance of a ProjectRisk class, initializes with specified arguments.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="code"></param>
        /// <param name="sortOrder"></param>
        public ProjectRisk(string name, string code, int sortOrder) : base(name, code)
        {
            this.SortOrder = sortOrder;
        }
        #endregion
    }
}
