using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFilePropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyResearchFile, ResearchFilePropertyModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.PropertyResearchFileId)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.ResearchFile, src => src.ResearchFile)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<ResearchFilePropertyModel, Entity.PimsPropertyResearchFile>()
                .Map(dest => dest.PropertyResearchFileId, src => src.Id)
                .Map(dest => dest.PropertyName, src => src.PropertyName)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.ResearchFile, src => src.ResearchFile)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
