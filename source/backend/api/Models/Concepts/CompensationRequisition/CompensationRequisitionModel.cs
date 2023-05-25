using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class CompensationRequisitionModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public bool? IsDraft { get; set; }

        public string FiscalYear { get; set; }

        public DateTime? AgreementDate { get; set; }

        public DateTime? ExpropriationNoticeServedDate { get; set; }

        public DateTime? ExpropriationVestingDate { get; set; }

        public DateTime? GenerationDate { get; set; }

        public string SpecialInstruction { get; set; }

        public string DetailedRemarks { get; set; }

        public bool? IsDisabled { get; set; }

        public List<CompensationFinancialModel> Financials { get; set; }
    }
}
