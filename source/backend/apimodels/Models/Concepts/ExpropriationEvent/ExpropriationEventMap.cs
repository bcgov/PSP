using Mapster;
using Pims.Api.Models.Base;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ExpropriationEvent
{
    public class ExpropriationEventMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsExpropOwnerHistory, ExpropriationEventModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.AcquisitionOwner, src => src.AcquisitionOwner)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.InterestHolder, src => src.InterestHolder)
                .Map(dest => dest.EventType, src => src.ExpropOwnerHistoryTypeCodeNavigation)
                .Map(dest => dest.EventDate, src => src.EventDt.ToNullableDateOnly())
                .Inherits<IBaseAppEntity, BaseConcurrentModel>();

            config.NewConfig<ExpropriationEventModel, PimsExpropOwnerHistory>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcquisitionOwnerId, src => src.AcquisitionOwnerId)
                .Map(dest => dest.InterestHolderId, src => src.InterestHolderId)
                .Map(dest => dest.ExpropOwnerHistoryTypeCode, src => src.EventType.Id)
                .Map(dest => dest.EventDt, src => src.EventDate.ToNullableDateTime())
                .Map(dest => dest.IsDisabled, src => false)
                .Inherits<BaseConcurrentModel, IBaseAppEntity>();
        }
    }
}
