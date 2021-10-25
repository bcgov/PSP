using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// BaseAppEntity class, provides a way to inherit shared properties on all entities for application specific types.
    /// Properties are used to track user activities on records.
    /// </summary>
    public abstract class BaseAppEntity : BaseEntity, IBaseAppEntity
    {
        #region Properties
        /// <summary>
        /// get/set - When this record was created.
        /// </summary>
        [Column("APP_CREATE_TIMESTAMP", Order = 93)]
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// get/set - The username who created the record.
        /// </summary>
        [Column("APP_CREATE_USERID", Order = 94)]
        public string CreatedBy { get; set; }

        /// <summary>
        /// get/set - The unique key to identify the user who created the record.
        /// </summary>
        [Column("APP_CREATE_USER_GUID", Order = 94)]
        public Guid? CreatedByKey { get; set; }

        /// <summary>
        /// get/set - The account directory of the user who created the record (IDIR/BCeID).
        /// </summary>
        [Column("APP_CREATE_USER_DIRECTORY", Order = 94)]
        public string CreatedByDirectory { get; set; }

        /// <summary>
        /// get/set - The display name of the user who created this record.
        /// </summary>
        [NotMapped]
        public string CreatedByName { get; set; }

        /// <summary>
        /// get/set - The email of the user who created this record.
        /// </summary>
        [NotMapped]
        public string CreatedByEmail { get; set; }

        /// <summary>
        /// get/set - When this record was updated.
        /// </summary>
        [Column("APP_LAST_UPDATE_TIMESTAMP", Order = 95)]
        public DateTime UpdatedOn { get; set; }

        /// <summary>
        /// get/set - Who updated this entity last.
        /// </summary>
        [Column("APP_LAST_UPDATE_USERID", Order = 96)]
        public string UpdatedBy { get; set; }

        /// <summary>
        /// get/set - The unique key to identify the user who created the record.
        /// </summary>
        [Column("APP_LAST_UPDATE_USER_GUID", Order = 94)]
        public Guid? UpdatedByKey { get; set; }

        /// <summary>
        /// get/set - The account directory of the user who created the record (IDIR/BCeID).
        /// </summary>
        [Column("APP_LAST_UPDATE_USER_DIRECTORY", Order = 94)]
        public string UpdatedByDirectory { get; set; }

        /// <summary>
        /// get/set - The display name of the user who updated this entity last.
        /// </summary>
        [NotMapped]
        public string UpdatedByName { get; set; }

        /// <summary>
        /// get/set - The email of the user who updated this entity last.
        /// </summary>
        [NotMapped]
        public string UpdatedByEmail { get; set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new instance of a BaseEntity class.
        /// Initializes the default values.
        /// </summary>
        protected BaseAppEntity()
        {
            this.CreatedOn = DateTime.UtcNow;
            this.UpdatedOn = this.CreatedOn;
        }
        #endregion
    }
}
