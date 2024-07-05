using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Models.Concepts.Lease
{
    public class LeaseChecklistItemMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseChecklistItem, FileChecklistItemModel>()
                .Map(dest => dest.Id, src => src.LeaseChecklistItemId)
                .Map(dest => dest.FileId, src => src.LeaseId)
                .Map(dest => dest.ItemType, src => src.LeaseChklstItemTypeCodeNavigation)
                .Map(dest => dest.StatusTypeCode, src => src.ChklstItemStatusTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<FileChecklistItemModel, Entity.PimsLeaseChecklistItem>()
                .Map(dest => dest.LeaseChecklistItemId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.FileId)
                .Map(dest => dest.LeaseChklstItemTypeCode, src => src.ItemType.Code)
                .Map(dest => dest.ChklstItemStatusTypeCode, src => src.StatusTypeCode.Id)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
