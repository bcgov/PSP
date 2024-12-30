using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Project
{
    public class ProjectPersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProjectPerson, ProjectPersonModel>()
                .Map(dest => dest.Id, src => src.ProjectPersonId)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Project, src => src.Project)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<ProjectPersonModel, Entity.PimsProjectPerson>()
                .Map(dest => dest.ProjectPersonId, src => src.Id)
                .Map(dest => dest.ProjectId, src => src.ProjectId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
