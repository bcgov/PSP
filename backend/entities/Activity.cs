using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// Activity class, provides an entity for the datamodel to manage activity models.
    /// In the DB schema this represents a type of activity.
    /// This is a little confusing because in the DB schema there is a table called PIMS_ACTIVITY.
    /// However that table is a project/property activity which is linked to this table to define its specific activity.
    /// This is essentially a lookup of all available activities that can be performed in a project on properties.
    /// </summary>
    [MotiTable("PIMS_ACTIVITY_MODEL", "ACTMDL")]
    public class Activity : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify activity.
        /// </summary>`
        [Column("ACTIVITY_MODEL_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - A description of the activity.
        /// </summary>
        [Column("DESCRIPTION")]
        public string Description { get; set; }

        /// <summary>
        /// get/set - Whether this activity is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }

        /// <summary>
        /// get - A collection of project activities.
        /// </summary>
        public ICollection<ProjectActivity> ProjectActivities { get; } = new List<ProjectActivity>();

        /// <summary>
        /// get - A collection of many-to-many tasks.
        /// </summary>
        public ICollection<ActivityTask> TasksManyToMany { get; } = new List<ActivityTask>();

        /// <summary>
        /// get - A collection of tasks.
        /// </summary>
        public ICollection<Task> Tasks { get; } = new List<Task>();
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a Activity class.
        /// </summary>
        public Activity() { }

        /// <summary>
        /// Create a new instance of a Activity class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <param name="risk"></param>
        /// <param name="tier"></param>
        public Activity(string description)
        {
            if (String.IsNullOrWhiteSpace(description)) throw new ArgumentException($"Argument '{nameof(description)}' is required.", nameof(description));

            this.Description = description;
        }
        #endregion
    }
}
