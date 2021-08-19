using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// ActivityTasks class, provides the many-to-many relationship between activities and tasks.
    /// </summary>
    [MotiTable("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL", "TSKTAM")]
    public class ActivityTask : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the activity task.
        /// </summary>
        [Column("TASK_TEMPLATE_ACTIVITY_MODEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the activity.
        /// </summary>
        [Column("ACTIVITY_MODEL_ID")]
        [ForeignKey("PIMS_ACTIVITY_MODEL_PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_FK")]
        public long ActivityId { get; set; }

        /// <summary>
        /// get/set - The activity.
        /// </summary>
        public Activity Activity { get; set; }

        /// <summary>
        /// get/set - The foreign key to the task.
        /// </summary>
        [Column("TASK_TEMPLATE_ID")]
        [ForeignKey("PIMS_TASK_TEMPLATE_PIMS_TASK_TEMPLATE_ACTIVITY_MODEL_FK")]
        public long TaskId { get; set; }

        /// <summary>
        /// get/set - The task.
        /// </summary>
        public Task Task { get; set; }

        /// <summary>
        /// get/set - Whether this task is mandatory for this activity.
        /// </summary>
        [Column("IS_MANDATORY")]
        public bool IsRequired { get; set; }

        /// <summary>
        /// get/set - The order the tasks will be sorted for implementation.
        /// </summary>
        [Column("IMPLEMENTATION_ORDER")]
        public int DisplayOrder { get; set; }

        /// <summary>
        /// get/set - Whether this task for the activity is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a ActivityTasks object.
        /// </summary>
        public ActivityTask() { }

        /// <summary>
        /// Creates a new instance of a ActivityTasks object, initializes with specified arguments.
        /// </summary>
        /// <param name="activity"></param>
        /// <param name="task"></param>
        /// <param name="isRequired"></param>
        public ActivityTask(Activity activity, Task task, bool isRequired = false)
        {
            this.Activity = activity ?? throw new ArgumentNullException(nameof(activity));
            this.ActivityId = activity.Id;
            this.Task = task ?? throw new ArgumentNullException(nameof(task));
            this.TaskId = task.Id;
            this.IsRequired = isRequired;
        }
        #endregion
    }
}
