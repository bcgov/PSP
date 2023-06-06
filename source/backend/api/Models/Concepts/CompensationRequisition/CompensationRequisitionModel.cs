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

        public long? YearlyFinancialId { get; set; }

        public FinancialCodeModel YearlyFinancial { get; set; }

        public long? ChartOfAccountsId { get; set; }

        public FinancialCodeModel ChartOfAccounts { get; set; }

        public long? ResponsibilityId { get; set; }

        public FinancialCodeModel Responsibility { get; set; }

        public DateTime? AgreementDate { get; set; }

        public DateTime? ExpropriationNoticeServedDate { get; set; }

        public DateTime? ExpropriationVestingDate { get; set; }

        public DateTime? GenerationDate { get; set; }

        public string SpecialInstruction { get; set; }

        public string DetailedRemarks { get; set; }

        public bool? IsDisabled { get; set; }

        public List<CompensationFinancialModel> Financials { get; set; }

        public List<CompensationPayeeModel> Payees { get; set; }
    }
}
