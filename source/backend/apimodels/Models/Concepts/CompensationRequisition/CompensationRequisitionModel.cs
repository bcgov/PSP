using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.FinancialCode;
using Pims.Api.Models.Concepts.InterestHolder;
using Pims.Api.Models.Concepts.Project;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompensationRequisitionModel : BaseAuditModel
    {
        public long? Id { get; set; }

        public long? AcquisitionFileId { get; set; }

        public AcquisitionFileModel AcquisitionFile { get; set; }

        public long? LeaseId { get; set; }

        public bool? IsDraft { get; set; }

        public string FiscalYear { get; set; }

        public long? YearlyFinancialId { get; set; }

        public FinancialCodeModel YearlyFinancial { get; set; }

        public long? ChartOfAccountsId { get; set; }

        public FinancialCodeModel ChartOfAccounts { get; set; }

        public long? ResponsibilityId { get; set; }

        public FinancialCodeModel Responsibility { get; set; }

        public DateOnly? FinalizedDate { get; set; }

        public DateOnly? AgreementDate { get; set; }

        public DateOnly? ExpropriationNoticeServedDate { get; set; }

        public DateOnly? ExpropriationVestingDate { get; set; }

        public DateOnly? AdvancedPaymentServedDate { get; set; }

        public DateOnly? GenerationDate { get; set; }

        public List<CompensationFinancialModel> Financials { get; set; }

        public long? AcquisitionOwnerId { get; set; }

        public AcquisitionFileOwnerModel AcquisitionOwner { get; set; }

        public long? InterestHolderId { get; set; }

        public InterestHolderModel InterestHolder { get; set; }

        public long? AcquisitionFileTeamId { get; set; }

        public AcquisitionFileTeamModel AcquisitionFileTeam { get; set; }

        public string LegacyPayee { get; set; }

        public bool? IsPaymentInTrust { get; set; }

        public string GstNumber { get; set; }

        public string SpecialInstruction { get; set; }

        public string DetailedRemarks { get; set; }

        public long? AlternateProjectId { get; set; }

        public ProjectModel AlternateProject { get; set; }

        public IEnumerable<CompReqLeaseStakeholderModel> CompReqLeaseStakeholder { get; set; }

        public IEnumerable<CompReqAcquisitionPropertyModel> CompReqAcquisitionProperties { get; set; }

        public IEnumerable<CompReqLeasePropertyModel> CompReqLeaseProperties { get; set; }
    }
}
