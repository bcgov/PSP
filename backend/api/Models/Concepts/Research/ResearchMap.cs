using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class ResearchMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFile, Model.ResearchModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ResearchFileId)
                .Map(dest => dest.ResearchFileStatusTypeCode, src => src.ResearchFileStatusTypeCodeNavigation)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RfileNumber, src => src.RfileNumber)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<Model.ResearchModel, Entity.PimsResearchFile>()
                .Map(dest => dest.ResearchFileId, src => src.Id)
                .Map(dest => dest.ResearchFileStatusTypeCodeNavigation, src => src.ResearchFileStatusTypeCode)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.RfileNumber, src => src.RfileNumber)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
