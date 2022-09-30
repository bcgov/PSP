using System;
using Pims.Api.Models;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented insurance model.
    /// </summary>
    public class InsuranceModel : BaseModel
    {
        /// <summary>
        /// get/set - The insurance's Id.
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The insurance's type.
        /// </summary>
        public TypeModel<string> InsuranceType { get; set; }

        /// <summary>
        /// get/set - The insurance's other insurance type.
        /// </summary>
        public string OtherInsuranceType { get; set; }

        /// <summary>
        /// get/set - The insurance's coverage description.
        /// </summary>
        public string CoverageDescription { get; set; }

        /// <summary>
        /// get/set - The insurance's coverage limit.
        /// </summary>
        public decimal? CoverageLimit { get; set; }

        /// <summary>
        /// get/set - The insurance's expiry date.
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The insurance's is in place flag indicator.
        /// </summary>
        public bool IsInsuranceInPlace { get; set; }
    }
}
