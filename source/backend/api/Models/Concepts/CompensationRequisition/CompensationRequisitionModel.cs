using System;
using System.Collections.Generic;

namespace Pims.Api.Models.Concepts
{
    public class CompensationRequisitionModel : BaseAppModel
    {
        public long? Id { get; set; }

        public long AcquisitionFileId { get; set; }

        public AcquisitionFileModel AcquisitionFile { get; set; }

        public bool? IsDraft { get; set; }

        public string FiscalYear { get; set; }

        public long? YearlyFinancialId { get; set; }

        public FinancialCodeModel YearlyFinancial { get; set; }

        public long? ChartOfAccountsId { get; set; }

        public FinancialCodeModel ChartOfAccounts { get; set; }

        public long? ResponsibilityId { get; set; }

        public FinancialCodeModel Responsibility { get; set; }

        public DateTime? FinalizedDate { get; set; }

        public DateTime? AgreementDate { get; set; }

        public DateTime? ExpropriationNoticeServedDate { get; set; }

        public DateTime? ExpropriationVestingDate { get; set; }

        public DateTime? AdvancedPaymentServedDate { get; set; }

        public DateTime? GenerationDate { get; set; }

        public List<CompensationFinancialModel> Financials { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public long? InterestHolderId { get; set; }

        public InterestHolderModel InterestHolder { get; set; }

        public long? AcquisitionFilePersonId { get; set; }

        public AcquisitionFilePersonModel AcquisitionFilePerson { get; set; }

        public string LegacyPayee { get; set; }

        public bool? IsPaymentInTrust { get; set; }

        public string GstNumber { get; set; }

        public string SpecialInstruction { get; set; }

        public string DetailedRemarks { get; set; }

        public bool? IsDisabled { get; set; }

        public long? AlternateProjectId { get; set; }

        public ProjectModel AlternateProject { get; set; }
    }
}
