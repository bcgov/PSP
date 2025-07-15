using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class ManagementActivityInvoiceModel : BaseAuditModel
    {
        #region Properties
        public long Id { get; set; }

        public DateOnly InvoiceDateTime { get; set; }

        public string InvoiceNum { get; set; }

        public string Description { get; set; }

        public decimal PretaxAmount { get; set; }

        public decimal? GstAmount { get; set; }

        public decimal? PstAmount { get; set; }

        public decimal? TotalAmount { get; set; }

        public bool? IsPstRequired { get; set; }

        public bool? IsDisabled { get; set; }

        public long ManagementActivityId { get; set; }

        public ManagementActivityModel ManagementActivity { get; set; }

        #endregion
    }
}
