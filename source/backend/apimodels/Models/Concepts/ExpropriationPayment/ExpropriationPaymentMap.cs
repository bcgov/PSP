using Mapster;
using Pims.Api.Models.Base;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ExpropriationPayment
{
    public class ExpropriationPaymentMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsExpropriationPayment, ExpropriationPaymentModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.InterestHolder, src => src.InterestHolder)
                .Map(dest => dest.ExpropriatingAuthorityId, src => src.ExpropriatingAuthority)
                .Map(dest => dest.ExpropriatingAuthority, src => src.ExpropriatingAuthorityNavigation)
                .Map(dest => dest.AdvancedPaymentServedDate, src => src.AdvPmtServedDt)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.PaymentItems, src => src.PimsExpropPmtPmtItems)
                .Inherits<IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<ExpropriationPaymentModel, PimsExpropriationPayment>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.ExpropriatingAuthority, src => src.ExpropriatingAuthorityId)
                .Map(dest => dest.AdvPmtServedDt, src => src.AdvancedPaymentServedDate)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.PimsExpropPmtPmtItems, src => src.PaymentItems)
                .Inherits<BaseConcurrentModel, IBaseEntity>();
        }
    }
}
