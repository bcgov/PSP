using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AcquisitionFileChecklistItemMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAcquisitionChecklistItem, AcquisitionFileChecklistItemModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.ItemType, src => src.AcqChklstItemTypeCodeNavigation)
                .Map(dest => dest.StatusTypeCode, src => src.AcqChklstItemStatusTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<AcquisitionFileChecklistItemModel, Entity.PimsAcquisitionChecklistItem>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.AcquisitionFileId, src => src.AcquisitionFileId)
                .Map(dest => dest.AcqChklstItemTypeCode, src => src.ItemType.Code)
                .Map(dest => dest.AcqChklstItemStatusTypeCode, src => src.StatusTypeCode.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
