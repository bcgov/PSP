using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class ResearchMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFile, Model.ResearchFileModel>()
                .Map(dest => dest.Id, src => src.ResearchFileId)
                .Map(dest => dest.ResearchFileStatusTypeCode, src => src.ResearchFileStatusTypeCodeNavigation)
                .Map(dest => dest.ResearchProperties, src => src.PimsPropertyResearchFiles)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RfileNumber, src => src.RfileNumber)
                .Inherits<Entity.IBaseEntity, BaseAppModel>();

            config.NewConfig<Model.ResearchFileModel, Entity.PimsResearchFile>()
                .Map(dest => dest.ResearchFileId, src => src.Id)
                .Map(dest => dest.ResearchFileStatusTypeCodeNavigation, src => src.ResearchFileStatusTypeCode)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RfileNumber, src => src.RfileNumber)
                .Inherits<BaseAppModel, Entity.IBaseEntity>();

            config.NewConfig<Entity.PimsPropertyResearchFile, Model.ResearchFilePropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyResearchFileId)
                .Map(dest => dest.Property, src => src.Property);
        }
    }
}
