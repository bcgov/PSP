using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Lease
{
    /// <summary>
    /// Provides a insurance-oriented insurance model.
    /// </summary>
    public class InsuranceModel : BaseAuditModel
    {
        /// <summary>
        /// get/set - The insurance's Id.
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// get/set - The corresponding lease this insurance references.
        /// </summary>
        public long LeaseId { get; set; }

        /// <summary>
        /// get/set - The insurance's type.
        /// </summary>
        public CodeTypeModel<string> InsuranceType { get; set; }

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
        public DateOnly? ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The insurance's is in place flag indicator.
        /// </summary>
        public bool IsInsuranceInPlace { get; set; }
    }
}
