using System;
using System.ComponentModel;

namespace Pims.Api.Areas.Reports.Models.Lease
{
    /// <summary>
    /// Provides a lease-oriented model.
    /// </summary>
    public class LeaseModel
    {
        #region Properties
        [DisplayName("L-File Number")]
        [CsvHelper.Configuration.Attributes.Name("L-File Number")]
        public string LFileNo { get; set; }

        [DisplayName("Historical File #")]
        [CsvHelper.Configuration.Attributes.Name("Historical File #")]
        public string HistoricalFileNo { get; set; }

        [DisplayName("MoTI Region")]
        [CsvHelper.Configuration.Attributes.Name("MoTI Region")]
        public string MotiRegion { get; set; }

        [DisplayName("Agreement Commencement Date")]
        [CsvHelper.Configuration.Attributes.Name("Agreement Commencement Date")]
        public DateOnly? AgreementCommencementDate { get; set; }

        [DisplayName("Agreement Expiry Date")]
        [CsvHelper.Configuration.Attributes.Name("Agreement Expiry Date")]
        public DateOnly? AgreementExpiryDate { get; set; }

        [DisplayName("Termination Date")]
        [CsvHelper.Configuration.Attributes.Name("Termination Date")]
        public DateOnly? TerminationDate { get; set; }

        [DisplayName("Current Renewal Commencement Date")]
        [CsvHelper.Configuration.Attributes.Name("Current Renewal Commencement Date")]
        public DateOnly? CurrentRenewalCommencementDate { get; set; }

        [DisplayName("Current Renewal Expiry Date")]
        [CsvHelper.Configuration.Attributes.Name("Current Renewal Expiry Date")]
        public DateOnly? CurrentRenewalExpiryDate { get; set; }

        [DisplayName("Additional Renewal Options")]
        [CsvHelper.Configuration.Attributes.Name("Additional Renewal Options")]
        public int? AdditionalRenewalOptionsCount { get; set; }

        [DisplayName("Final Renewal Expiry Date")]
        [CsvHelper.Configuration.Attributes.Name("Final Renewal Expiry Date")]
        public DateOnly? FinalRenewalExpiryDate { get; set; }

        [DisplayName("Start Date")]
        [CsvHelper.Configuration.Attributes.Name("Start Date")]
        public DateOnly? StartDate { get; set; }

        [DisplayName("End Date")]
        [CsvHelper.Configuration.Attributes.Name("End Date")]
        public DateOnly? EndDate { get; set; }

        [DisplayName("Current Term Start Date")]
        [CsvHelper.Configuration.Attributes.Name("Current Term Start Date")]
        public DateOnly? CurrentPeriodStartDate { get; set; }

        [DisplayName("Current Term End Date")]
        [CsvHelper.Configuration.Attributes.Name("Current Term End Date")]
        public DateOnly? CurrentTermEndDate { get; set; }

        [DisplayName("Tenant")]
        [CsvHelper.Configuration.Attributes.Name("Tenant")]
        public string TenantName { get; set; }

        [DisplayName("PID")]
        [CsvHelper.Configuration.Attributes.Name("PID")]
        public int? Pid { get; set; }

        [DisplayName("PIN")]
        [CsvHelper.Configuration.Attributes.Name("PIN")]
        public int? Pin { get; set; }

        [DisplayName("Civic Address")]
        [CsvHelper.Configuration.Attributes.Name("Civic Address")]
        public string CivicAddress { get; set; }

        [DisplayName("Lease Area")]
        [CsvHelper.Configuration.Attributes.Name("Lease Area")]
        public float? LeaseArea { get; set; }

        [DisplayName("Unit")]
        [CsvHelper.Configuration.Attributes.Name("Unit")]
        public string AreaUnit { get; set; }

        [DisplayName("Program Name")]
        [CsvHelper.Configuration.Attributes.Name("Program Name")]
        public string ProgramName { get; set; }

        [DisplayName("Lease Type")]
        [CsvHelper.Configuration.Attributes.Name("Lease Type")]
        public string LeaseTypeName { get; set; }

        [DisplayName("Lease Purpose Type")]
        [CsvHelper.Configuration.Attributes.Name("Lease Purpose Type")]
        public string PurposeTypes { get; set; }

        [DisplayName("Lease Status Type")]
        [CsvHelper.Configuration.Attributes.Name("Lease Status Type")]
        public string StatusType { get; set; }

        [DisplayName("Is Expired?")]
        [CsvHelper.Configuration.Attributes.Name("Is Expired?")]
        public string IsExpired { get; set; }

        [DisplayName("Lease Payment Frequency")]
        [CsvHelper.Configuration.Attributes.Name("Lease Payment Frequency")]
        public string LeasePaymentFrequencyType { get; set; }

        [DisplayName("Lease Amount")]
        [CsvHelper.Configuration.Attributes.Name("Lease Amount")]
        public decimal? LeaseAmount { get; set; }

        [DisplayName("Lease Notes")]
        [CsvHelper.Configuration.Attributes.Name("Lease Notes")]
        public string LeaseNotes { get; set; }

        [DisplayName("Financial Gain?")]
        [CsvHelper.Configuration.Attributes.Name("Financial Gain?")]
        public string FinancialGain { get; set; }

        [DisplayName("Public Benefit?")]
        [CsvHelper.Configuration.Attributes.Name("Public Benefit?")]
        public string PublicBenefit { get; set; }
        #endregion
    }
}
