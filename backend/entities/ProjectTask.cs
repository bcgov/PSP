using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectTask class, provides an entity for the datamodel to manage what tasks are associated and completed for the project.
    /// </summary>
    [MotiTable("PIMS_PROJECT_TASK", "PRJTSK")]
    public class ProjectTask : BaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project task.
        /// </summary>
        [Column("PROJECT_TASK_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the project - PRIMARY KEY.
        /// </summary>
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }

        /// <summary>
        /// get/set - The project.
        /// </summary>
        public Project Project { get; set; }

        /// <summary>
        /// get/set - The foreign key to the task - PRIMARY KEY.
        /// </summary>
        [Column("TASK_ID")]
        public long TaskId { get; set; }

        /// <summary>
        /// get/set - The task.
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// get/set - Whether the task was completed.
        /// </summary>
        [Column("IS_COMPLETED")]
        public bool IsCompleted { get; set; }

        /// <summary>
        /// get/set - The date when the task was completed.
        /// </summary>
        [Column("COMPLETED_ON")]
        public DateTime? CompletedOn { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectTask class.
        /// </summary>
        public ProjectTask() { }

        /// <summary>
        /// Create a new instance of a ProjectTask class, initialize it with link to specified task.
        /// </summary>
        /// <param name="project"></param>
        /// <param name="task"></param>
        public ProjectTask(Project project, Task task)
        {
            this.ProjectId = project?.Id ??
                throw new ArgumentNullException(nameof(project));
            this.Project = project;

            this.TaskId = task?.Id ??
                throw new ArgumentNullException(nameof(task));
            this.Task = task;
        }
        #endregion
    }
}
