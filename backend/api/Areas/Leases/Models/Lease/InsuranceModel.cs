using System;

namespace Pims.Api.Areas.Lease.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented insurance model.
    /// </summary>
    public class InsuranceModel
    {
        /// <summary>
        /// get/set - The insurance's Id
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// get/set - The insurance's type
        /// </summary>
        public TypeModel<string> InsuranceType { get; set; }

        /// <summary>
        /// get/set - The insurance's organization
        /// </summary>
        public OrganizationModel InsurerOrganization { get; set; }

        /// <summary>
        /// get/set - The insurance's contact
        /// </summary>
        public PersonModel InsurerContact { get; set; }

        /// <summary>
        /// get/set - The insurance's risk management contact
        /// </summary>
        public PersonModel MotiRiskManagementContact { get; set; }

        /// <summary>
        /// get/set - The insurance's bctfa risk management contact
        /// </summary>
        public PersonModel BctfaRiskManagementContact { get; set; }

        /// <summary>
        /// get/set - The insurance's insurance payee type
        /// </summary>
        public TypeModel<string> InsurancePayeeType { get; set; }

        /// <summary>
        /// get/set - The insurance's other insurance type
        /// </summary>
        public string OtherInsuranceType { get; set; }

        /// <summary>
        /// get/set - The insurance's coverage description
        /// </summary>
        public string CoverageDescription { get; set; }

        /// <summary>
        /// get/set - The insurance's coverage limit
        /// </summary>
        public decimal CoverageLimit { get; set; }

        /// <summary>
        /// get/set - The insurance's insured value
        /// </summary>
        public decimal InsuredValue { get; set; }

        /// <summary>
        /// get/set - The insurance's start date
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// get/set - The insurance's expiry date
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// get/set - The insurance's risk assessment completed date
        /// </summary>
        public DateTime? RiskAssessmentCompletedDate { get; set; }

        /// <summary>
        /// get/set - The insurance's is in place flag indicator
        /// </summary>
        public bool InsuranceInPlace { get; set; }
    }
}
