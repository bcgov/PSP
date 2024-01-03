using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyActivityInvoiceModel : BaseAuditModel
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

        public long PropertyActivityId { get; set; }

        public PropertyActivityModel PropertyActivity { get; set; }

        #endregion
    }
}
