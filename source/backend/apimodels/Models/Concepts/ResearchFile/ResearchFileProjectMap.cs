using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.ResearchFile
{
    public class ResearchFileProjectMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFileProject, ResearchFileProjectModel>()
                .Map(dest => dest.Id, src => src.ResearchFileProjectId)
                .Map(dest => dest.Project, src => src.Project)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.FileId, src => src.ResearchFileId)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<ResearchFileProjectModel, Entity.PimsResearchFileProject>()
                .Map(dest => dest.ResearchFileProjectId, src => src.Id)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.ResearchFileId, src => src.FileId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
