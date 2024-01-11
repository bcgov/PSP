using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileSaleMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionSale, DispositionFileSaleModel>()
                .Map(dest => dest.Id, src => src.DispositionSaleId)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.FinalConditionRemovalDate, src => src.FinalConditionRemovalDt)
                .Map(dest => dest.SaleCompletionDate, src => src.SaleCompletionDt)
                .Map(dest => dest.SaleFiscalYear, src => src.SaleFiscalYear)
                .Map(dest => dest.FinalSaleAmount, src => src.SaleFinalAmt)
                .Map(dest => dest.RealtorCommissionAmount, src => src.RealtorCommissionAmt)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.GstCollectedAmount, src => src.GstCollectedAmt)
                .Map(dest => dest.NetBookAmount, src => src.NetBookAmt)
                .Map(dest => dest.TotalCostAmount, src => src.TotalCostAmt)
                .Map(dest => dest.SppAmount, src => src.SppAmt)
                .Map(dest => dest.RemediationAmount, src => src.RemediationAmt)
                .Map(dest => dest.DispositionPurchasers, src => src.PimsDispositionPurchasers)
                .Map(dest => dest.DispositionPurchaserAgents, src => src.PimsDspPurchAgents)
                .Map(dest => dest.DispositionPurchaserSolicitors, src => src.PimsDspPurchSolicitors);

            config.NewConfig<DispositionFileSaleModel, Entity.PimsDispositionSale>()
                .Map(dest => dest.DispositionSaleId, src => src.Id)
                .Map(dest => dest.DispositionFileId, src => src.DispositionFileId)
                .Map(dest => dest.FinalConditionRemovalDt, src => src.FinalConditionRemovalDate)
                .Map(dest => dest.SaleCompletionDt, src => src.SaleCompletionDate)
                .Map(dest => dest.SaleFiscalYear, src => src.SaleFiscalYear)
                .Map(dest => dest.SaleFinalAmt, src => src.FinalSaleAmount)
                .Map(dest => dest.RealtorCommissionAmt, src => src.RealtorCommissionAmount)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.GstCollectedAmt, src => src.GstCollectedAmount)
                .Map(dest => dest.NetBookAmt, src => src.NetBookAmount)
                .Map(dest => dest.TotalCostAmt, src => src.TotalCostAmount)
                .Map(dest => dest.SppAmt, src => src.SppAmount)
                .Map(dest => dest.RemediationAmt, src => src.RemediationAmount)
                .Map(dest => dest.PimsDispositionPurchasers, src => src.DispositionPurchasers)
                .Map(dest => dest.PimsDspPurchAgents, src => src.DispositionPurchaserAgents)
                .Map(dest => dest.PimsDspPurchSolicitors, src => src.DispositionPurchaserSolicitors);
        }
    }
}
