using System;

namespace Pims.Dal.Entities
{
    public interface IFinancialCodeEntity<T_Code> : IBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        T_Code Code { get; set; }

        string Description { get; set; }

        int? DisplayOrder { get; set; }

        DateTime EffectiveDate { get; set; }

        DateTime? ExpiryDate { get; set; }

        #endregion
    }
}
