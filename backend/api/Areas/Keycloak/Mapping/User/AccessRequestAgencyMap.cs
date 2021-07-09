using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
{
    public class AccessRequestAgencyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequestAgency, Model.AccessRequestAgencyModel>()
                .Map(dest => dest.Id, src => src.AgencyId)
                .Map(dest => dest.Name, src => src.Agency == null ? null : src.Agency.Name)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestAgencyModel, Entity.AccessRequestAgency>()
                .Map(dest => dest.AgencyId, src => src.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
