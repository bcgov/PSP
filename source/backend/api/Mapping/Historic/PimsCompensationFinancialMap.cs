using Mapster;

namespace Pims.Dal.Entities.Mappers
{
    public class PimsCompensationFinancialMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {

            config.NewConfig<PimsCompensationRequisitionHist, PimsCompensationRequisition>()
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.IsDraft, src => src.IsDraft)
                .Map(dest => dest.FiscalYear, src => src.FiscalYear)
                .Map(dest => dest.YearlyFinancialId, src => src.YearlyFinancialId)
                .Map(dest => dest.ChartOfAccountsId, src => src.ChartOfAccountsId)
                .Map(dest => dest.ResponsibilityId, src => src.ResponsibilityId)
                .Map(dest => dest.FinalizedDate, src => src.FinalizedDate)
                .Map(dest => dest.AgreementDt, src => src.AgreementDt)
                .Map(dest => dest.GenerationDt, src => src.GenerationDt)
                .Map(dest => dest.IsPaymentInTrust, src => src.IsPaymentInTrust)
                .Map(dest => dest.GstNumber, src => src.GstNumber)
                .Map(dest => dest.FinalizedDate, src => src.FinalizedDate)
                .Map(dest => dest.SpecialInstruction, src => src.SpecialInstruction)
                .Map(dest => dest.DetailedRemarks, src => src.DetailedRemarks)
                .Map(dest => dest.AlternateProjectId, src => src.AlternateProjectId);

            config.NewConfig<PimsCompReqFinancialHist, PimsCompReqFinancial>()
                .Map(dest => dest.CompReqFinancialId, src => src.CompReqFinancialId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationRequisitionId)
                .Map(dest => dest.FinancialActivityCodeId, src => src.FinancialActivityCodeId)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmt)
                .Map(dest => dest.TaxAmt, src => src.TaxAmt)
                .Map(dest => dest.TotalAmt, src => src.TotalAmt)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled);
        }
    }
}
