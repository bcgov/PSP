using System;

namespace Pims.Tools.Keycloak.Sync.Models.Pims
{
    /// <summary>
    /// BaseAppModel abstract class, provides the standard tracking properties for models.
    /// </summary>
    public abstract class BaseAppModel : BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - When the item was created.
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// get/set - When the item was updated.
        /// </summary>
        public DateTime UpdatedOn { get; set; }
        #endregion
    }
}
