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

        [DisplayName("MoTI Region")]
        [CsvHelper.Configuration.Attributes.Name("MoTI Region")]
        public string MotiRegion { get; set; }

        [DisplayName("Start Date")]
        [CsvHelper.Configuration.Attributes.Name("Start Date")]
        public DateTime? StartDate { get; set; }

        [DisplayName("End Date")]
        [CsvHelper.Configuration.Attributes.Name("End Date")]
        public DateTime? EndDate { get; set; }

        [DisplayName("Current Term Start Date")]
        [CsvHelper.Configuration.Attributes.Name("Current Term Start Date")]
        public DateTime? CurrentTermStartDate { get; set; }

        [DisplayName("Current Term End Date")]
        [CsvHelper.Configuration.Attributes.Name("Current Term End Date")]
        public DateTime? CurrentTermEndDate { get; set; }

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
        public string PurposeType { get; set; }

        [DisplayName("Lease Status Type")]
        [CsvHelper.Configuration.Attributes.Name("Lease Status Type")]
        public string StatusType { get; set; }

        [DisplayName("PS File Number")]
        [CsvHelper.Configuration.Attributes.Name("PS File Number")]
        public string PsFileNo { get; set; }

        [DisplayName("Term Start Date")]
        [CsvHelper.Configuration.Attributes.Name("Term Start Date")]
        public DateTime? TermStartDate { get; set; }

        [DisplayName("Term Renewal Date")]
        [CsvHelper.Configuration.Attributes.Name("Term Renewal Date")]
        public DateTime? TermRenewalDate { get; set; }

        [DisplayName("Term Expiry Date")]
        [CsvHelper.Configuration.Attributes.Name("Term Expiry Date")]
        public DateTime? TermExpiryDate { get; set; }

        [DisplayName("Lease Payment Frequency")]
        [CsvHelper.Configuration.Attributes.Name("Lease Payment Frequency")]
        public string LeasePaymentFrequencyType { get; set; }

        [DisplayName("Lease Amount")]
        [CsvHelper.Configuration.Attributes.Name("Lease Amount")]
        public decimal? LeaseAmount { get; set; }

        [DisplayName("Inspection Date")]
        [CsvHelper.Configuration.Attributes.Name("Inspection Date")]
        public DateTime? InspectionDate { get; set; }

        [DisplayName("Inspection Notes")]
        [CsvHelper.Configuration.Attributes.Name("Inspection Notes")]
        public string InspectionNotes { get; set; }

        [DisplayName("Lease Notes")]
        [CsvHelper.Configuration.Attributes.Name("Lease Notes")]
        public string LeaseNotes { get; set; }

        [DisplayName("Is Expired?")]
        [CsvHelper.Configuration.Attributes.Name("Is Expired?")]
        public string IsExpired { get; set; }
        #endregion
    }
}
