using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectWorkflow class, provides an entity for the datamodel to manage the many-to-many relationship between projects and workflows organizations.
    /// </summary>
    [MotiTable("PIMS_PROJECT_WORKFLOW_MODEL", "PRWKMD")]
    public class ProjectWorkflow : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify this project workflow.
        /// </summary>
        [Column("PROJECT_WORKFLOW_MODEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the project..
        /// </summary>
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }

        /// <summary>
        /// get/set - The project.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// get/set - The foreign key to the the workflow.
        /// </summary>
        [Column("WORKFLOW_MODEL_ID")]
        public long WorkflowId { get; set; }

        /// <summary>
        /// get/set - The workflow.
        /// </summary>
        public Workflow Workflow { get; set; }

        /// <summary>
        /// get/set - Whether this project workflow is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of project activities.
        /// </summary>
        public ICollection<ProjectActivity> ProjectActivities { get; } = new List<ProjectActivity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectWorkflow class.
        /// </summary>
        public ProjectWorkflow() { }

        /// <summary>
        /// Create a new instance of a ProjectWorkflow class.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="workflow"></param>
        public ProjectWorkflow(Project project, Workflow workflow)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.ProjectId = project.Id;
            this.Workflow = workflow ?? throw new ArgumentNullException(nameof(project));
            this.WorkflowId = workflow.Id;
        }
        #endregion
    }
}
