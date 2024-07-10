namespace Pims.Dal.Entities
{
    /// <summary>
    /// An interface for type codes that can be expired based on a date range (EffectiveDate -> ExpiryDate).
    /// </summary>
    public interface IExpiringTypeEntity<T_EffectiveDate, T_ExpiryDate>
    {
        #region Properties

        /// <summary>
        /// get/set - Date the lookup item became effective.
        /// </summary>
        T_EffectiveDate EffectiveDate { get; set; }

        /// <summary>
        /// get/set - Date the lookup item ceased to be in effect. If not set, the lookup item never expires.
        /// </summary>
        T_ExpiryDate ExpiryDate { get; set; }

        #endregion
    }
}
