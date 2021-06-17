using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Project.Models.Workflow;

namespace Pims.Api.Areas.Project.Mapping.Workflow
{
    public class TaskMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Task, Model.TaskModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsOptional, src => src.IsOptional)
                .Inherits<Entity.LookupEntity, Api.Models.LookupModel>();
        }
    }
}
