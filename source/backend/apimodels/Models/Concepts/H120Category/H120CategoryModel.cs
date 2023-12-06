using System;
using Pims.Api.Models.Base;

namespace Pims.Api.Models.Concepts.H120Category
{
    public class H120CategoryModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long? FinancialActivityId { get; set; }

        public long? WorkActivityId { get; set; }

        public long? CostTypeId { get; set; }

        public int? H120CategoryNo { get; set; }

        public string Description { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public bool? IsDisabled { get; set; }
    }
}
