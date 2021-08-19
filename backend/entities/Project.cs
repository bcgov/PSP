using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Project class, provides an entity for the datamodel to manage projects.
    /// </summary>
    [MotiTable("PIMS_PROJECT", "PROJCT")]
    public class Project : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project.
        /// </summary>`
        [Column("PROJECT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project type.
        /// </summary>
        [Column("PROJECT_TYPE_CODE")]
        public string ProjectTypeId { get; set; }

        /// <summary>
        /// get/set - The type of project.
        /// </summary>
        public ProjectType ProjectType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project status.
        /// </summary>
        [Column("PROJECT_STATUS_TYPE_CODE")]
        public string StatusId { get; set; }

        /// <summary>
        /// get/set - The status of project.
        /// </summary>
        public ProjectStatusType Status { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project risk.
        /// </summary>
        [Column("PROJECT_RISK_TYPE_CODE")]
        public string RiskId { get; set; }

        /// <summary>
        /// get/set - The project risk.
        /// </summary>
        public ProjectRiskType Risk { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project tier.
        /// </summary>
        [Column("PROJECT_TIER_TYPE_CODE")]
        public string TierId { get; set; }

        /// <summary>
        /// get/set - The project tier.
        /// </summary>
        public ProjectTierType Tier { get; set; }

        /// <summary>
        /// get - A collection of notes related to this project.
        /// </summary>
        public ICollection<ProjectNote> Notes { get; } = new List<ProjectNote>();

        /// <summary>
        /// get - A collection of workflows.
        /// </summary>
        public ICollection<Workflow> Workflows { get; } = new List<Workflow>();

        /// <summary>
        /// get - A collection of many-to-many workflows.
        /// </summary>
        public ICollection<ProjectWorkflow> WorkflowsManyToMany { get; } = new List<ProjectWorkflow>();

        /// <summary>
        /// get - A collection of properties.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get - A collection of many-to-many properties.
        /// </summary>
        public ICollection<ProjectProperty> PropertiesManyToMany { get; } = new List<ProjectProperty>();

        /// <summary>
        /// get - A collection of project activities associated to properties.  This is also the many-to-many relationship to activities.
        /// </summary>
        public ICollection<ProjectActivity> ProjectActivities { get; } = new List<ProjectActivity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Project class.
        /// </summary>
        public Project() { }

        /// <summary>
        /// Create a new instance of a Project class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="risk"></param>
        /// <param name="tier"></param>
        public Project(ProjectType type, ProjectStatusType status, ProjectRiskType risk, ProjectTierType tier)
        {
            this.ProjectType = type ?? throw new ArgumentNullException(nameof(type));
            this.ProjectTypeId = type.Id;
            this.Status = status ?? throw new ArgumentNullException(nameof(status));
            this.StatusId = status.Id;
            this.Risk = risk ?? throw new ArgumentNullException(nameof(risk));
            this.RiskId = risk.Id;
            this.Tier = tier ?? throw new ArgumentNullException(nameof(tier));
            this.TierId = tier.Id;
        }
        #endregion
    }
}
