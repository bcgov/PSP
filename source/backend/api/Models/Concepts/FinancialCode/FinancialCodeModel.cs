using System;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// Provides a model that represents a financial code from various lookup tables.
    /// </summary>
    public class FinancialCodeModel : BaseAppModel
    {
        #region Properties

        /// <summary>
        /// get/set - The model id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The type for this financial code.
        /// </summary>
        public FinancialCodeTypes Type { get; set; }

        /// <summary>
        /// get/set - A unique code for the lookup.
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// get/set - A description of the financial code.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// get/set - The display order for this financial code.
        /// </summary>
        public int? DisplayOrder { get; set; }

        /// <summary>
        /// get/set - The effective date for this financial code.
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// get/set - (Optional) The expiry date for this financial code.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        #endregion
    }
}
