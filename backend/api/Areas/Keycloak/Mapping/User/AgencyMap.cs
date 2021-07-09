using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class AgencyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Agency, Model.AgencyModel>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.AgencyModel, Entity.Agency>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.ParentId, src => src.ParentId)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();

            config.NewConfig<Entity.UserAgency, Model.AgencyModel>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.Id, src => src.AgencyId)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();


            config.NewConfig<Model.AgencyModel, Entity.UserAgency>()
                .IgnoreNonMapped(true)
                .Map(dest => dest.AgencyId, src => src.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
