using System.ComponentModel;

namespace Pims.Api.Models.Concepts
{
    /// <summary>
    /// model represents a single row fo rthe lease payment report.
    /// </summary>
    public class LeasePaymentReportModel
    {
        #region Properties

        /// <summary>
        /// get/set - Lease region.
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// get/set - PIMS L File Number.
        /// </summary>
        ///
        [DisplayName("L-File Number")]
        [CsvHelper.Configuration.Attributes.Name("L-File Number")]
        public string LFileNumber { get; set; }

        /// <summary>
        /// get/set - legacy lis number.
        /// </summary>
        ///
        [DisplayName("LIS number")]
        [CsvHelper.Configuration.Attributes.Name("LIS number")]
        public string LisNumber { get; set; }

        /// <summary>
        /// get/set - legacy ps file name.
        /// </summary>
        ///
        [DisplayName("PS File number")]
        [CsvHelper.Configuration.Attributes.Name("PS File number")]
        public string PsFileNumber { get; set; }

        /// <summary>
        /// get/set - Lease status description.
        /// </summary>
        ///
        [DisplayName("Lease status")]
        [CsvHelper.Configuration.Attributes.Name("Lease status")]
        public string LeaseStatus { get; set; }

        /// <summary>
        /// get/set - The value of the program name.
        /// </summary>
        ///
        [DisplayName("Properties")]
        [CsvHelper.Configuration.Attributes.Name("Properties")]
        public string PropertyList { get; set; }

        /// <summary>
        /// get/set - Tenant list, separated by commas. Only display if type is "tenant".
        /// </summary>
        ///
        [DisplayName("Tenant(s)")]
        [CsvHelper.Configuration.Attributes.Name("Tenant(s)")]
        public string TenantList { get; set; }

        /// <summary>
        /// get/set - Either Payable or Receivable.
        /// </summary>
        ///
        [DisplayName("Payable/Receivable")]
        [CsvHelper.Configuration.Attributes.Name("Payable/Receivable")]
        public string PayableOrReceivable { get; set; }

        /// <summary>
        /// get/set - Program, including other description.
        /// </summary>
        public string Program { get; set; }

        /// <summary>
        /// get/set - Purpose, including other description.
        /// </summary>
        public string Purpose { get; set; }

        /// <summary>
        /// get/set - Start date of the term parent for this lease.
        /// </summary>
        ///
        [DisplayName("Term start")]
        [CsvHelper.Configuration.Attributes.Name("Term start")]
        public string TermStart { get; set; }

        /// <summary>
        /// get/set - Expiry date of the term parent for this lease.
        /// </summary>
        ///
        [DisplayName("Term expiry")]
        [CsvHelper.Configuration.Attributes.Name("Term expiry")]
        public string TermExpiry { get; set; }

        /// <summary>
        /// get/set - Whether or not this term is exercised (Yes/No).
        /// </summary>
        ///
        [DisplayName("Term exercised")]
        [CsvHelper.Configuration.Attributes.Name("Term exercised")]
        public string IsTermExercised { get; set; }

        /// <summary>
        /// get/set - How often this payment occurs within the term.
        /// </summary>
        ///
        [DisplayName("Payment frequency")]
        [CsvHelper.Configuration.Attributes.Name("Payment frequency")]
        public string PaymentFrequency { get; set; }

        /// <summary>
        /// get/set - A text description of when the payment is due.
        /// </summary>
        ///
        [DisplayName("Payment due")]
        [CsvHelper.Configuration.Attributes.Name("Payment due")]
        public string PaymentDueString { get; set; }

        /// <summary>
        /// get/set - The expected payment total, including GST.
        /// </summary>
        ///
        [DisplayName("Expected payment")]
        [CsvHelper.Configuration.Attributes.Name("Expected payment")]
        public decimal ExpectedPayment { get; set; }

        /// <summary>
        /// get/set - The actual payment total, including GST.
        /// </summary>
        ///
        [DisplayName("Payment total")]
        [CsvHelper.Configuration.Attributes.Name("Payment total")]
        public decimal PaymentTotal { get; set; }

        /// <summary>
        /// get/set - For this payment, if the payment is partial, complete, etc.
        /// </summary>
        ///
        [DisplayName("Payment status")]
        [CsvHelper.Configuration.Attributes.Name("Payment status")]
        public string PaymentStatus { get; set; }

        /// <summary>
        /// get/set - The payment amount, not including GST.
        /// </summary>
        ///
        [DisplayName("Payment amount (pretax)")]
        [CsvHelper.Configuration.Attributes.Name("Payment amount (pretax)")]
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// get/set - The GST portion of the payment.
        /// </summary>
        ///
        [DisplayName("Payment GST")]
        [CsvHelper.Configuration.Attributes.Name("Payment GST")]
        public decimal PaymentGst { get; set; }

        /// <summary>
        /// get/set - The date the payment was received.
        /// </summary>
        ///
        [DisplayName("Payment received date")]
        [CsvHelper.Configuration.Attributes.Name("Payment received date")]
        public string PaymentReceivedDate { get; set; }

        /// <summary>
        /// get/set - Most recent payment made for the file.
        /// </summary>
        ///
        [DisplayName("Latest payment date")]
        [CsvHelper.Configuration.Attributes.Name("Latest payment date")]
        public string LatestPaymentDate { get; set; }
        #endregion
    }
}
