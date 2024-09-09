using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.DispositionFile
{
    public class DispositionFileChecklistItemMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDispositionChecklistItem, FileChecklistItemModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.FileId, src => src.DispositionFileId)
                .Map(dest => dest.ItemType, src => src.DspChklstItemTypeCodeNavigation)
                .Map(dest => dest.StatusTypeCode, src => src.ChklstItemStatusTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<FileChecklistItemModel, Entity.PimsDispositionChecklistItem>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.DispositionFileId, src => src.FileId)
                .Map(dest => dest.DspChklstItemTypeCode, src => src.ItemType.Code)
                .Map(dest => dest.ChklstItemStatusTypeCode, src => src.StatusTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
