using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.File;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Models.Concepts.Lease
{
    public class LeaseChecklistItemTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseChklstItemType, FileChecklistItemTypeModel>()
                .Map(dest => dest.Code, src => src.Id)
                .Map(dest => dest.SectionCode, src => src.ParentId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Hint, src => src.Hint)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();
        }
    }
}
