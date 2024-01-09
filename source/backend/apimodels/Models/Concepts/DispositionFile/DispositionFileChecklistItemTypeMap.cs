using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Models.Concepts.File;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AcquisitionFile
{
    public class DispositionFileChecklistItemTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsDspChklstItemType, FileChecklistItemTypeModel>()
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
