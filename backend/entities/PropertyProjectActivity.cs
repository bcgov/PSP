using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// PropertyProjectActivity class, provides an entity for the datamodel to manage property project activities.
    /// </summary>
    [MotiTable("PIMS_PROPERTY_ACTIVITY", "PRPACT")]
    public class PropertyProjectActivity : BaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - Primary key to identify the property project activity.
        /// </summary>
        [Column("PROPERTY_ACTIVITY_ID")]
        public long Id { get; set; }

        /// <summary>
        /// get/set - The foreign key to the property.
        /// </summary>
        [Column("PROPERTY_ID")]
        public long? PropertyId { get; set; }

        /// <summary>
        /// get/set - The property.
        /// </summary>
        public Property Property { get; set; }

        /// <summary>
        /// get/set - The foreign key to the project activity.
        /// </summary>
        [Column("ACTIVITY_ID")]
        public long? ProjectActivityId { get; set; }

        /// <summary>
        /// get/set - The project activity.
        /// </summary>
        public ProjectActivity ProjectActivity { get; set; }

        /// <summary>
        /// get/set - Whether this user organization is disabled.
        /// </summary>
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a new instance of a PropertyProjectActivity class.
        /// </summary>
        public PropertyProjectActivity() { }

        /// <summary>
        /// Create a new instance of a PropertyProjectActivity class.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="projectActivity"></param>
        public PropertyProjectActivity(Property property, ProjectActivity projectActivity)
        {
            this.Property = property ?? throw new ArgumentNullException(nameof(property));
            this.PropertyId = property.Id;
            this.ProjectActivity = projectActivity ?? throw new ArgumentNullException(nameof(projectActivity));
            this.ProjectActivityId = projectActivity.Id;
        }
        #endregion
    }
}
