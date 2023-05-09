using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ResearchFileProjectMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsResearchFileProject, ResearchFileProjectModel>()
                .Map(dest => dest.Id, src => src.ResearchFileProjectId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Project, src => src.Project)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.FileId, src => src.ResearchFileId)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<ResearchFileProjectModel, Entity.PimsResearchFileProject>()
                .Map(dest => dest.ResearchFileProjectId, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.ResearchFileId, src => src.FileId)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
