using System;

namespace Pims.Tools.Keycloak.Sync.Models.Pims
{
    /// <summary>
    /// BaseModel abstract class, provides the standard tracking properties for models.
    /// </summary>
    public abstract class BaseModel
    {
        #region Properties
        /// <summary>
        /// get/set - The rowvesion.
        /// </summary>
        public long RowVersion { get; set; }
        #endregion
    }
}
