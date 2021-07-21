using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Task class, provides an entity for the datamodel to manage tasks.
    /// In the DB schema this table represents a type of task that will be performed as part of an activity.
    /// In the DB schema there is a table called PIMS_TASK which can be confusing because that table is actually a project/project activity task.
    /// Which is linked to this table to identify the type of task being performed.
    /// This is essentially a lookup of available tasks for any given activity.
    /// </summary>
    [MotiTable("PIMS_TASK_TEMPLATE", "TSKTMP")]
    public class Task : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify task.
        /// </summary>`
        [Column("TASK_TEMPLATE_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - Foreign key to the task type.
        /// </summary>
        [Column("TASK_TEMPLATE_TYPE_CODE")]
        public string TaskTypeId { get; set; }

        /// <summary>
        /// get/set - The type of task.
        /// </summary>
        public TaskType TaskType { get; set; }

        /// <summary>
        /// get/set - Foreign key to the task status.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of project activity tasks.
        /// </summary>
        public ICollection<ProjectActivityTask> ProjectActivityTasks { get; } = new List<ProjectActivityTask>();

        /// <summary>
        /// get - A collection of many-to-many activities.
        /// </summary>
        public ICollection<ActivityTask> ActivitiesManyToMany { get; } = new List<ActivityTask>();

        /// <summary>
        /// get - A collection of activities.
        /// </summary>
        public ICollection<Activity> Activities { get; } = new List<Activity>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Task class.
        /// </summary>
        public Task() { }

        /// <summary>
        /// Create a new instance of a Task class.
        /// </summary>
        /// <param name="type"></param>
        public Task(TaskType type)
        {
            this.TaskType = type ?? throw new ArgumentNullException(nameof(type));
            this.TaskTypeId = type.Id;
        }
        #endregion
    }
}
