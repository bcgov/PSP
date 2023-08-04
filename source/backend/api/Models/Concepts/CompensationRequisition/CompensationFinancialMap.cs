using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class CompensationFinancialMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCompReqFinancial, CompensationFinancialModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.FinancialActivityCodeId, src => src.FinancialActivityCodeId)
                .Map(dest => dest.FinancialActivityCode, src => src.FinancialActivityCode)
                .Map(dest => dest.CompensationId, src => src.CompensationRequisitionId)
                .Map(dest => dest.PretaxAmount, src => src.PretaxAmt)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.TaxAmount, src => src.TaxAmt)
                .Map(dest => dest.TotalAmount, src => src.TotalAmt)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<CompensationFinancialModel, Entity.PimsCompReqFinancial>()
                .PreserveReference(true)
                .Map(dest => dest.CompReqFinancialId, src => src.Id)
                .Map(dest => dest.FinancialActivityCodeId, src => src.FinancialActivityCodeId)
                .Map(dest => dest.CompensationRequisitionId, src => src.CompensationId)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmount)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.TaxAmt, src => src.TaxAmount)
                .Map(dest => dest.TotalAmt, src => src.TotalAmount)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
