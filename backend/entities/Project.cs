using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Project class, provides an entity for the datamodel to manage projects.
    /// </summary>
    [MotiTable("PIMS_PROJECT", "PROJCT")]
    public class Project : BaseEntity
    {
        #region Properties
        #region Identification
        /// <summary>
        /// get/set - The primary key provides a unique identity for the project.
        /// </summary>
        [Column("PROJECT_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A unique identity for the project.
        /// </summary>
        [Column("PROJECT_NUMBER")]
        public string ProjectNumber { get; set; }

        /// <summary>
        /// get/set - The type of project this is.
        /// </summary>
        [Column("PROJECT_TYPE")]
        public ProjectTypes ProjectType { get; set; }

        /// <summary>
        /// get/set - A display name to identify the project.
        /// </summary>
        [Column("NAME")]
        public string Name { get; set; }

        /// <summary>
        /// get/set - A description of the project.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - The reported fiscal year this project.
        /// </summary>
        [Column("REPORTED_FISCAL_YEAR")]
        public int ReportedFiscalYear { get; set; }

        /// <summary>
        /// get/set - The actual or forecasted fiscal year.
        /// </summary>
        [Column("ACTUAL_FISCAL_YEAR")]
        public int ActualFiscalYear { get; set; }
        #endregion

        #region Owning agency
        /// <summary>
        /// get/set - The foreign key to the owning agency.
        /// </summary>
        [Column("AGENCY_ID")]
        public long AgencyId { get; set; }

        /// <summary>
        /// get/set - The owning agency.
        /// </summary>
        public Agency Agency { get; set; }

        /// <summary>
        /// get/set - The project manager name(s).
        /// </summary>
        [Column("MANAGER")]
        public string Manager { get; set; }
        #endregion

        #region Tier
        /// <summary>
        /// get/set - The foreign key to the tier level.
        /// </summary>
        [Column("TIER_LEVEL_ID")]
        public long TierLevelId { get; set; }

        /// <summary>
        /// get/set - The tier level.
        /// </summary>
        public TierLevel TierLevel { get; set; }
        #endregion

        #region Risk
        /// <summary>
        /// get/set - Foreign key to the risk level of the project.
        /// </summary>
        [Column("PROJECT_RISK_ID")]
        public long RiskId { get; set; }

        /// <summary>
        /// get/set - The risk level of the project.
        /// </summary>
        public ProjectRisk Risk { get; set; }
        #endregion

        #region workflow and status
        /// <summary>
        /// get/set - Foreign key to the current workflow the project is in.
        /// </summary>
        [Column("WORKFLOW_ID")]
        public long? WorkflowId { get; set; }

        /// <summary>
        /// get/set - The current workflow the project is in.
        /// </summary>
        public Workflow Workflow { get; set; }

        /// <summary>
        /// get/set - The foreign key to the project status.
        /// </summary>
        [Column("PROJECT_STATUS_ID")]
        public long StatusId { get; set; }

        /// <summary>
        /// get/set - The project status.
        /// </summary>
        public ProjectStatus Status { get; set; }
        #endregion

        /// <summary>
        /// get/set - Additional serialized metadata.
        /// </summary>
        [Column("METADATA")]
        public string Metadata { get; set; }

        #region Dates
        /// <summary>
        /// get/set - When the project was submitted.
        /// </summary>
        [Column("SUBMITTED_ON")]
        public DateTime? SubmittedOn { get; set; }

        /// <summary>
        /// get/set - When the project was approved.
        /// </summary>
        [Column("APPROVED_ON")]
        public DateTime? ApprovedOn { get; set; }

        /// <summary>
        /// get/set - When the project was denied.
        /// </summary>
        [Column("DENIED_ON")]
        public DateTime? DeniedOn { get; set; }

        /// <summary>
        /// get/set - When the project was cancelled.
        /// </summary>
        [Column("CANCELLED_ON")]
        public DateTime? CancelledOn { get; set; }

        /// <summary>
        /// get/set - When the project was completed.
        /// </summary>
        [Column("COMPLETED_ON")]
        public DateTime? CompletedOn { get; set; }
        #endregion

        #region Financials
        /// <summary>
        /// get/set - The net book value.
        /// </summary>
        [Column("NET_BOOK")]
        public decimal? NetBook { get; set; }

        /// <summary>
        /// get/set - The market value.
        /// </summary>
        [Column("MARKET")]
        public decimal? Market { get; set; }

        /// <summary>
        /// get/set - The assessed value.
        /// </summary>
        [Column("ASSESSED")]
        public decimal? Assessed { get; set; }

        /// <summary>
        /// get/set - The appraised value.
        /// </summary>
        [Column("APPRAISED")]
        public decimal? Appraised { get; set; }
        #endregion

        /// <summary>
        /// get - A collection of properties associated to this project.
        /// </summary>
        public ICollection<ProjectProperty> Properties { get; } = new List<ProjectProperty>();

        /// <summary>
        /// get - A collection of tasks associated to this project.
        /// </summary>
        public ICollection<ProjectTask> Tasks { get; } = new List<ProjectTask>();

        /// <summary>
        /// get - A collection of responses from notifications for this project.
        /// </summary>
        public ICollection<ProjectAgencyResponse> Responses { get; } = new List<ProjectAgencyResponse>();

        /// <summary>
        /// get - A collection of notifications sent for this project.
        /// </summary>
        public ICollection<NotificationQueue> Notifications { get; } = new List<NotificationQueue>();

        /// <summary>
        /// get - A collection of notes for this project.
        /// </summary>
        public ICollection<ProjectNote> Notes { get; } = new List<ProjectNote>();

        /// <summary>
        /// get - A collection of snapshots of this project.
        /// </summary>
        public ICollection<ProjectSnapshot> Snapshots { get; } = new List<ProjectSnapshot>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Project class.
        /// </summary>
        public Project() { }

        /// <summary>
        /// Create a new instance of a Project class, initializes with specified arguments
        /// </summary>
        /// <param name="projectNumber"></param>
        /// <param name="name"></param>
        /// <param name="tierLevel"></param>
        public Project(string projectNumber, string name, TierLevel tierLevel)
        {
            if (String.IsNullOrWhiteSpace(projectNumber)) throw new ArgumentException("Argument cannot be null, empty or whitespace.", nameof(projectNumber));
            if (String.IsNullOrWhiteSpace(name)) throw new ArgumentException("Argument cannot be null, empty or whitespace.", nameof(name));

            this.ProjectNumber = projectNumber;
            this.Name = name;
            this.TierLevelId = tierLevel?.Id ?? throw new ArgumentNullException(nameof(tierLevel));
            this.TierLevel = tierLevel;
        }

        /// <summary>
        /// Create a new instance of a Project class, initializes with specified arguments
        /// </summary>
        /// <param name="type"></param>
        /// <param name="projectNumber"></param>
        /// <param name="name"></param>
        /// <param name="tierLevel"></param>
        public Project(ProjectTypes type, string projectNumber, string name, TierLevel tierLevel) : this(projectNumber, name, tierLevel)
        {
            this.ProjectType = type;
        }
        #endregion
    }
}
