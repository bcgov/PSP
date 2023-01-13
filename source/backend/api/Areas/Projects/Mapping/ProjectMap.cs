using Mapster;
using Pims.Api.Areas.Projects.Models;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Projects.Mapping
{
    public class ProjectMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProject, ProjectSearchModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Code, src => src.Code)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.Status, src => src.ProjectStatusTypeCodeNavigation.Description)
                .Map(dest => dest.Region, src => src.RegionCode.ToString())
                .Map(dest => dest.LastUpdatedBy, src => src.AppLastUpdateUserid)
                .Map(dest => dest.LastUpdatedDate, src => src.AppLastUpdateTimestamp);
        }
    }
}
