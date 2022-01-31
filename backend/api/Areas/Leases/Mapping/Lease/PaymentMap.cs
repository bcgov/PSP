using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PaymentMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeasePayment, Model.PaymentModel>()
                .Map(dest => dest.Id, src => src.LeasePaymentId)
                .Map(dest => dest.LeaseTermId, src => src.LeaseTermId)
                .Map(dest => dest.LeasePaymentMethodType, src => src.LeasePaymentMethodTypeCodeNavigation)
                .Map(dest => dest.LeasePaymentStatusTypeCode, src => src.LeasePaymentStatusTypeCodeNavigation)
                .Map(dest => dest.AmountGst, src => src.PaymentAmountGst)
                .Map(dest => dest.AmountPreTax, src => src.PaymentAmountPreTax)
                .Map(dest => dest.AmountPst, src => src.PaymentAmountPst)
                .Map(dest => dest.AmountTotal, src => src.PaymentAmountTotal)
                .Map(dest => dest.ReceivedDate, src => src.PaymentReceivedDate)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();
        }
    }
}
