using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Workflow class, provides an entity for the datamodel to manage workflows.
    /// </summary>
    [MotiTable("PIMS_WORKFLOW_MODEL", "WFLMDL")]
    public class Workflow : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify workflow.
        /// </summary>
        [Column("WORKFLOW_MODEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the workflow type.
        /// </summary>
        [Column("WORKFLOW_MODEL_TYPE_CODE")]
        public string WorkflowTypeId { get; set; }

        /// <summary>
        /// get/set - The type of workflow.
        /// </summary>
        public WorkflowType WorkflowType { get; set; }

        /// <summary>
        /// get/set - Whether the workflow is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of projects.
        /// </summary>
        public ICollection<Project> Projects { get; } = new List<Project>();

        /// <summary>
        /// get - A collection of many-to-many projects.
        /// </summary>
        public ICollection<ProjectWorkflow> ProjectsManyToMany { get; } = new List<ProjectWorkflow>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Workflow class.
        /// </summary>
        public Workflow() { }

        /// <summary>
        /// Create a new instance of a Workflow class.
        /// </summary>
        /// <param name="workflowTypeId"></param>
        public Workflow(string workflowTypeId)
        {
            if (string.IsNullOrWhiteSpace(workflowTypeId)) throw new ArgumentException($"Argument '{nameof(workflowTypeId)}' is required.", nameof(workflowTypeId));

            this.WorkflowTypeId = workflowTypeId;
        }
        #endregion
    }
}
