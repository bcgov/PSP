using System;

namespace Pims.Dal.Entities.Models
{
    /// <summary>
    /// LastUpdatedByModel class, provides a model that is used to retrievie last Updated by information.
    /// </summary>
    public class LastUpdatedByModel
    {
        #region Properties

        /// <summary>
        /// get/set - The id of the parent entity/model.
        /// </summary>
        public long ParentId { get; set; }

        /// <summary>
        /// get/set - User ID for the last update.
        /// </summary>
        public string AppLastUpdateUserid { get; set; }

        /// <summary>
        /// get/set - User GUID for the last update.
        /// </summary>
        public Guid? AppLastUpdateUserGuid { get; set; }

        /// <summary>
        /// get/set - The time stamp of the last update.
        /// </summary>
        public DateTime AppLastUpdateTimestamp { get; set; }

        #endregion
    }
}
