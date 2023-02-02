using System;

namespace Pims.Dal.Entities
{
    public interface IFinancialCodeEntity : IIdentityEntity<long>, IBaseAppEntity
    {
        #region Properties

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        string Code { get; set; }

        string Description { get; set; }

        int? DisplayOrder { get; set; }

        DateTime EffectiveDate { get; set; }

        DateTime? ExpiryDate { get; set; }

        #endregion
    }
}
