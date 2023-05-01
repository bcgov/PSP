using System;

namespace Pims.Dal.Entities
{
    /// <summary>
    /// An interface for type codes that can be expired based on a date range (EffectiveDate -> ExpiryDate).
    /// </summary>
    public interface IExpiringTypeEntity
    {
        #region Properties

        /// <summary>
        /// get/set - Date the lookup item became effective.
        /// </summary>
        DateTime EffectiveDate { get; set; }

        /// <summary>
        /// get/set - Date the lookup item ceased to be in effect. If not set, the lookup item never expires.
        /// </summary>
        DateTime? ExpiryDate { get; set; }

        #endregion
    }
}
