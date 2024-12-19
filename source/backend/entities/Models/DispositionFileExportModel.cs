using System.ComponentModel;

namespace Pims.Dal.Entities.Models
{
    public class DispositionFileExportModel
    {
        [DisplayName("Disposition File #")]
        public string FileNumber { get; set; }

        [DisplayName("Reference #")]
        public string ReferenceNumber { get; set; }

        [DisplayName("Disposition File Name")]
        public string FileName { get; set; }

        [DisplayName("Disposition type")]
        public string DispositionType { get; set; }

        [DisplayName("MOTI Region")]
        public string MotiRegion { get; set; }

        [DisplayName("Team Member(s)")]
        public string TeamMembers { get; set; }

        [DisplayName("Ministry Project")]
        public string Project { get; set; }

        [DisplayName("Civic Address")]
        public string CivicAddress { get; set; }

        [DisplayName("PID")]
        public string Pid { get; set; }

        [DisplayName("PIN")]
        public string Pin { get; set; }

        [DisplayName("General Location")]
        public string GeneralLocation { get; set; }

        [DisplayName("Disposition Status")]
        public string DispositionStatusTypeCode { get; set; }

        [DisplayName("Status (File)")]
        public string DispositionFileStatusTypeCode { get; set; }

        [DisplayName("Funding (File)")]
        public string FileFunding { get; set; }

        [DisplayName("Assigned Date")]
        public string FileAssignedDate { get; set; }

        [DisplayName("Disposition Completed")]
        public string DispositionCompleted { get; set; }

        [DisplayName("Initiation Document")]
        public string InitiatingDocument { get; set; }

        [DisplayName("Initiation Document Date")]
        public string InitiatingDocumentDate { get; set; }

        [DisplayName("Physical File Status")]
        public string PhysicalFileStatus { get; set; }

        [DisplayName("Appraisal Value")]
        public decimal AppraisalValue { get; set; }

        [DisplayName("Appraisal Date")]
        public string AppraisalDate { get; set; }

        [DisplayName("BC Assessment Value")]
        public decimal AssessmentValue { get; set; }

        [DisplayName("BCA Roll Year")]
        public string RollYear { get; set; }

        [DisplayName("List Price")]
        public decimal ListPrice { get; set; }

        [DisplayName("Purchaser Name(s)")]
        public string PurchaserNames { get; set; }

        [DisplayName("Sale Completion Date")]
        public string SaleCompletionDate { get; set; }

        [DisplayName("Fiscal Year of Sale")]
        public string FiscalYearOfSale { get; set; }

        [DisplayName("Final Sale Price")]
        public decimal FinalSalePrice { get; set; }

        [DisplayName("Realtor Commission")]
        public decimal RealtorCommission { get; set; }

        [DisplayName("GST Collected")]
        public decimal GstCollected { get; set; }

        [DisplayName("Net Book Value")]
        public decimal NetBookValue { get; set; }

        [DisplayName("Total Cost of Sale")]
        public decimal TotalCostOfSale { get; set; }

        [DisplayName("Net Before SPP (Surplus Property Program)")]
        public decimal NetBeforeSpp { get; set; }

        [DisplayName("SPP Amount")]
        public decimal SppAmount { get; set; }

        [DisplayName("Net Proceeds After SPP")]
        public decimal NetAfterSpp { get; set; }

        [DisplayName("Remediation Cost")]
        public decimal RemediationCost { get; set; }
    }
}
