using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.CompensationRequisition
{
    public class CompensationRequisitionMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config
                .NewConfig<Entity.PimsCompensationRequisition, CompensationRequisitionModel>()
                .Map(dest => dest.Id, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionFile, src => src.AcquisitionFile)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.YearlyFinancialId, src => src.YearlyFinancialId)
                .Map(dest => dest.YearlyFinancial, src => src.YearlyFinancial)
                .Map(dest => dest.ChartOfAccountsId, src => src.ChartOfAccountsId)
                .Map(dest => dest.ChartOfAccounts, src => src.ChartOfAccounts)
                .Map(dest => dest.ResponsibilityId, src => src.ResponsibilityId)
                .Map(dest => dest.Responsibility, src => src.Responsibility)
                .Map(dest => dest.FinalizedDate, src => src.FinalizedDate)
                .Map(dest => dest.AgreementDate, src => src.AgreementDt)
                .Map(dest => dest.GenerationDate, src => src.GenerationDt)
                .Map(dest => dest.Financials, src => src.PimsCompReqFinancials)
                .Map(dest => dest.IsPaymentInTrust, src => src.IsPaymentInTrust)
                .Map(dest => dest.GstNumber, src => src.GstNumber)
                .Map(dest => dest.FinalizedDate, src => src.FinalizedDate)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.AlternateProjectId, src => src.AlternateProjectId)
                .Map(dest => dest.AlternateProject, src => src.AlternateProject)
                .Map(dest => dest.CompReqAcqPayees, src => src.PimsCompReqAcqPayees)
                .Map(dest => dest.CompReqLeasePayees, src => src.PimsCompReqLeasePayees)
                .Map(dest => dest.CompReqAcquisitionProperties, src => src.PimsPropAcqFlCompReqs)
                .Map(dest => dest.CompReqLeaseProperties, src => src.PimsPropLeaseCompReqs)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config
                .NewConfig<CompensationRequisitionModel, Entity.PimsCompensationRequisition>()
                .Map(dest => dest.CompensationRequisitionId, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.YearlyFinancialId, src => src.YearlyFinancialId)
                .Map(dest => dest.ChartOfAccountsId, src => src.ChartOfAccountsId)
                .Map(dest => dest.ResponsibilityId, src => src.ResponsibilityId)
                .Map(dest => dest.FinalizedDate, src => src.FinalizedDate)
                .Map(dest => dest.AgreementDt, src => src.AgreementDate)
                .Map(dest => dest.GenerationDt, src => src.GenerationDate)
                .Map(dest => dest.PimsCompReqFinancials, src => src.Financials)
                .Map(dest => dest.IsPaymentInTrust, src => src.IsPaymentInTrust)
                .Map(dest => dest.GstNumber, src => src.GstNumber)
                .Map(dest => dest.FinalizedDate, src => src.FinalizedDate)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.AlternateProjectId, src => src.AlternateProjectId)
                .Map(dest => dest.AlternateProject, src => src.AlternateProject)
                .Map(dest => dest.PimsCompReqAcqPayees, src => src.CompReqAcqPayees)
                .Map(dest => dest.PimsCompReqLeasePayees, src => src.CompReqLeasePayees)
                .Map(dest => dest.PimsPropAcqFlCompReqs, src => src.CompReqAcquisitionProperties)
                .Map(dest => dest.PimsPropLeaseCompReqs, src => src.CompReqLeaseProperties)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
