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

        [DisplayName("Agreement Commencement Date")]
        [CsvHelper.Configuration.Attributes.Name("Agreement Commencement Date")]
        public DateOnly? AgreementCommencementDate { get; set; }

        [DisplayName("Agreement Expiry Date")]
        [CsvHelper.Configuration.Attributes.Name("Agreement Expiry Date")]
        public DateOnly? AgreementExpiryDate { get; set; }

        [DisplayName("Tenant")]
        [CsvHelper.Configuration.Attributes.Name("Tenant")]
        public string TenantName { get; set; }

        [DisplayName("PID")]
        [CsvHelper.Configuration.Attributes.Name("PID")]
        public int? Pid { get; set; }

        [DisplayName("PIN")]
        [CsvHelper.Configuration.Attributes.Name("PIN")]
        public int? Pin { get; set; }

        [DisplayName("Lease Amount")]
        [CsvHelper.Configuration.Attributes.Name("Lease Amount")]
        public decimal? LeaseAmount { get; set; }
        #endregion
    }
}
