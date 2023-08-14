using Mapster;
using Pims.Api.Models.Concepts.ExpropriationPayment;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ExpropriationPaymentItemMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsExpropPmtPmtItem, ExpropriationPaymentItemModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.ExpropriationPaymentId, src => src.ExpropriationPaymentId)
                .Map(dest => dest.PaymentItemTypeCode, src => src.PaymentItemTypeCode)
                .Map(dest => dest.PaymentItemType, src => src.PaymentItemTypeCodeNavigation)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.PretaxAmount, src => src.PretaxAmt)
                .Map(dest => dest.TaxAmount, src => src.TaxAmt)
                .Map(dest => dest.TotalAmount, src => src.TotalAmt)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<ExpropriationPaymentItemModel, PimsExpropPmtPmtItem>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.ExpropriationPaymentId, src => src.ExpropriationPaymentId)
                .Map(dest => dest.PaymentItemTypeCode, src => src.PaymentItemTypeCode)
                .Map(dest => dest.IsGstRequired, src => src.IsGstRequired)
                .Map(dest => dest.PretaxAmt, src => src.PretaxAmount)
                .Map(dest => dest.TaxAmt, src => src.TaxAmount)
                .Map(dest => dest.TotalAmt, src => src.TotalAmount)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Inherits<BaseModel, IBaseEntity>();
        }
    }
}
