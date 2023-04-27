using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class CompensationModel : BaseAppModel
    {
        public long Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public bool? IsDraft { get; set; }

        public string FiscalYear { get; set; }

        public DateTime? AgreementDateTime { get; set; }

        public DateTime? ExpropriationNoticeServedDateTime { get; set; }

        public DateTime? ExpropriationVestingDateTime { get; set; }

        public DateTime? GenerationDatetTime { get; set; }

        public string SpecialInstruction { get; set; }

        public string DetailedRemarks { get; set; }

        public bool? IsDisabled { get; set; }

        public List<CompensationFinancialModel> Financials { get; set; }
    }
}
