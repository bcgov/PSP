using System;

namespace Pims.Dal.Entities
{
    public interface IBaseAppEntity : IBaseEntity
    {
        #region Properties
        /// <summary>
        /// get/set - When this record was created.
        /// </summary>
        DateTime CreatedOn { get; set; }

        /// <summary>
        /// get/set - The username who created the record.
        /// </summary>
        string CreatedBy { get; set; }

        /// <summary>
        /// get/set - The unique key to identify the user who created the record.
        /// </summary>
        Guid? CreatedByKey { get; set; }

        /// <summary>
        /// get/set - The account directory of the user who created the record (IDIR/BCeID).
        /// </summary>
        string CreatedByDirectory { get; set; }

        /// <summary>
        /// get/set - The display name of the user who created this record.
        /// </summary>
        string CreatedByName { get; set; }

        /// <summary>
        /// get/set - The email of the user who created this record.
        /// </summary>
        string CreatedByEmail { get; set; }

        /// <summary>
        /// get/set - When this record was updated.
        /// </summary>
        DateTime UpdatedOn { get; set; }

        /// <summary>
        /// get/set - Who updated this entity last.
        /// </summary>
        string UpdatedBy { get; set; }

        /// <summary>
        /// get/set - The unique key to identify the user who created the record.
        /// </summary>
        Guid? UpdatedByKey { get; set; }

        /// <summary>
        /// get/set - The account directory of the user who created the record (IDIR/BCeID).
        /// </summary>
        string UpdatedByDirectory { get; set; }

        /// <summary>
        /// get/set - The display name of the user who updated this entity last.
        /// </summary>
        string UpdatedByName { get; set; }

        /// <summary>
        /// get/set - The email of the user who updated this entity last.
        /// </summary>
        string UpdatedByEmail { get; set; }
        #endregion
    }
}
