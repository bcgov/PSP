using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ProjectActivityTask class, provides an entity for the datamodel to manage project activities tasks.
    /// In the DB schema this table represents a realized task that is associated with a realized project activity.
    /// It provides a link to the actual type of task.
    /// In the EF model there is a separate entity called Task, which is the actual task type.
    /// This entity provides a way to link a project activity and a user with a task type.
    /// </summary>
    [MotiTable("PIMS_TASK", "TASK")]
    public class ProjectActivityTask : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify project activity task.
        /// </summary>`
        [Column("TASK_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the project activity.
        /// </summary>
        [Column("ACTIVITY_ID")]
        public long? ProjectActivityId { get; set; }

        /// <summary>
        /// get/set - The project activity.
        /// </summary>
        public ProjectActivity ProjectActivity { get; set; }

        /// <summary>
        /// get/set - Foreign key to the task.
        /// </summary>
        [Column("TASK_TEMPLATE_ID")]
        public long TaskId { get; set; }

        /// <summary>
        /// get/set - The task.
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// get/set - Foreign key to the user.
        /// </summary>
        [Column("USER_ID")]
        public long UserId { get; set; }

        /// <summary>
        /// get/set - The user.
        /// </summary>
        public User User { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a ProjectActivityTask class.
        /// </summary>
        public ProjectActivityTask() { }

        /// <summary>
        /// Create a new instance of a ProjectActivityTask class.
        /// </summary>
        /// <param name="projectActivity"></param>
        /// <param name="task"></param>
        /// <param name="user"></param>
        public ProjectActivityTask(ProjectActivity projectActivity, Task task, User user)
        {
            this.ProjectActivity = projectActivity ?? throw new ArgumentNullException(nameof(projectActivity));
            this.ProjectActivityId = projectActivity.Id;
            this.Task = task ?? throw new ArgumentNullException(nameof(task));
            this.TaskId = task.Id;
            this.User = user ?? throw new ArgumentNullException(nameof(user));
            this.UserId = user.Id;
        }
        #endregion
    }
}
