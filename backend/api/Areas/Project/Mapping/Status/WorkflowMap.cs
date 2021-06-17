using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Project.Models.Status;

namespace Pims.Api.Areas.Project.Mapping.Status
{
    public class WorkflowMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Workflow, Model.WorkflowModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Inherits<Entity.CodeEntity, Api.Models.CodeModel>();
        }
    }
}
