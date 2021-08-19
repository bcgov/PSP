using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectActivity class, provides an entity for the datamodel to manage project activities.
    /// In the DB schema this represents a realized activity being performed in a project on properties.
    /// In the EF structure there is a separate entity called Activity because it is the actual activity type.
    /// This entity is a linking table between a project, activity, workflow and properties.
    /// </summary>
    [MotiTable("PIMS_ACTIVITY", "ACTVTY")]
    public class ProjectActivity : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify activity.
        /// </summary>`
        [Column("ACTIVITY_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project.
        /// </summary>
        [Column("PROJECT_ID")]
        public long? ProjectId { get; set; }

        /// <summary>
        /// get/set - The project.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project workflow.
        /// This property links to the many-to-many table between a project and a workflow.
        /// </summary>
        [Column("WORKFLOW_ID")]
        public long? ProjectWorkflowId { get; set; }

        /// <summary>
        /// get/set - The project workflow.
        /// </summary>
        public ProjectWorkflow ProjectWorkflow { get; set; }

        /// <summary>
        /// get/set - Foreign key to the activity.
        /// </summary>
        [Column("ACTIVITY_MODEL_ID")]
        public long ActivityId { get; set; }

        /// <summary>
        /// get/set - The activity.
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// get - A collection of project activity tasks.  Also the many-to-many relationship to tasks.
        /// </summary>
        public ICollection<ProjectActivityTask> ProjectActivityTasks { get; } = new List<ProjectActivityTask>();

        /// <summary>
        /// get - Collection of properties associated to this project activity.
        /// </summary>
        public ICollection<Property> Properties { get; } = new List<Property>();

        /// <summary>
        /// get - Collection of many-to-many properties associated to this project activity.
        /// </summary>
        public ICollection<PropertyProjectActivity> PropertiesManyToMany { get; } = new List<PropertyProjectActivity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectActivity class.
        /// </summary>
        public ProjectActivity() { }

        /// <summary>
        /// Create a new instance of a ProjectActivity class.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="projectWorkflow"></param>
        /// <param name="activity"></param>
        public ProjectActivity(Project project, ProjectWorkflow projectWorkflow, Activity activity)
        {
            this.Project = project ?? throw new ArgumentNullException(nameof(project));
            this.ProjectId = project.Id;
            this.ProjectWorkflow = projectWorkflow ?? throw new ArgumentNullException(nameof(projectWorkflow));
            this.ProjectWorkflowId = projectWorkflow.Id;
            this.Activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.ActivityId = activity.Id;
        }
        #endregion
    }
}
