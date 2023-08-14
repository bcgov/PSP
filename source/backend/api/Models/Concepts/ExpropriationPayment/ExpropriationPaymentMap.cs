using Mapster;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
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
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.PaymentItems, src => src.PimsExpropPmtPmtItems)
                .Inherits<IBaseEntity, BaseModel>();

            config.NewConfig<ExpropriationPaymentModel, PimsExpropriationPayment>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.ExpropriatingAuthority, src => src.ExpropriatingAuthorityId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.PimsExpropPmtPmtItems, src => src.PaymentItems)
                .Inherits<BaseModel, IBaseEntity>();
        }
    }
}
