using System;
using System.Collections.Generic;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.AcquisitionFile;
using Pims.Api.Models.Concepts.FinancialCode;
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

        public DateOnly? GenerationDate { get; set; }

        public List<CompensationFinancialModel> Financials { get; set; }

        public bool? IsPaymentInTrust { get; set; }

        public string GstNumber { get; set; }

        public string SpecialInstruction { get; set; }

        public string DetailedRemarks { get; set; }

        public long? AlternateProjectId { get; set; }

        public ProjectModel AlternateProject { get; set; }

        public IEnumerable<CompReqPayeeModel> CompReqPayees { get; set; }

        public IEnumerable<CompReqLeaseStakeholderModel> CompReqLeaseStakeholders { get; set; }

        public IEnumerable<CompReqAcquisitionPropertyModel> CompReqAcquisitionProperties { get; set; }

        public IEnumerable<CompReqLeasePropertyModel> CompReqLeaseProperties { get; set; }
    }
}
