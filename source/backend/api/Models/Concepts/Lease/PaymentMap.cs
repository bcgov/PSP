using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PaymentMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeasePayment, PaymentModel>()
                .Map(dest => dest.Id, src => src.LeasePaymentId)
                .Map(dest => dest.LeaseTermId, src => src.LeaseTermId)
                .Map(dest => dest.LeasePaymentMethodType, src => src.LeasePaymentMethodTypeCodeNavigation)
                .Map(dest => dest.LeasePaymentStatusTypeCode, src => src.LeasePaymentStatusTypeCodeNavigation)
                .Map(dest => dest.AmountGst, src => src.PaymentAmountGst)
                .Map(dest => dest.AmountPreTax, src => src.PaymentAmountPreTax)
                .Map(dest => dest.AmountPst, src => src.PaymentAmountPst)
                .Map(dest => dest.AmountTotal, src => src.PaymentAmountTotal)
                .Map(dest => dest.ReceivedDate, src => src.PaymentReceivedDate)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<PaymentModel, Entity.PimsLeasePayment>()
                .Map(dest => dest.LeasePaymentId, src => src.Id)
                .Map(dest => dest.LeaseTermId, src => src.LeaseTermId)
                .Map(dest => dest.LeasePaymentMethodTypeCode, src => src.LeasePaymentMethodType.Id)
                .Map(dest => dest.LeasePaymentStatusTypeCode, src => src.LeasePaymentStatusTypeCode.Id)
                .Map(dest => dest.PaymentAmountGst, src => src.AmountGst)
                .Map(dest => dest.PaymentAmountPreTax, src => src.AmountPreTax)
                .Map(dest => dest.PaymentAmountPst, src => src.AmountPst)
                .Map(dest => dest.PaymentAmountTotal, src => src.AmountTotal)
                .Map(dest => dest.PaymentReceivedDate, src => src.ReceivedDate)
                .Map(dest => dest.Note, src => src.Note)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
