using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFileMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFile, ResearchFileModel>()
                .Map(dest => dest.Id, src => src.ResearchFileId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RfileNumber, src => src.RfileNumber)
                .Map(dest => dest.RoadAlias, src => src.RoadAlias)
                .Map(dest => dest.RoadName, src => src.RoadName)
                .Map(dest => dest.ResearchFileStatusTypeCode, src => src.ResearchFileStatusTypeCodeNavigation)
                .Map(dest => dest.ResearchProperties, src => src.PimsPropertyResearchFiles)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<ResearchFileModel, Entity.PimsResearchFile>()
                .Map(dest => dest.ResearchFileId, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RfileNumber, src => src.RfileNumber)
                .Map(dest => dest.RoadAlias, src => src.RoadAlias)
                .Map(dest => dest.RoadName, src => src.RoadName)
                .Map(dest => dest.ResearchFileStatusTypeCodeNavigation, src => src.ResearchFileStatusTypeCode)
                .Map(dest => dest.PimsPropertyResearchFiles, src => src.ResearchProperties)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
