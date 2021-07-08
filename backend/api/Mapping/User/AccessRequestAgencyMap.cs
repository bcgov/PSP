using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.User;

namespace Pims.Api.Mapping.User
{
    public class AccessRequestAgencyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.AccessRequestAgency, Model.AccessRequestAgencyModel>()
                .Map(dest => dest.Id, src => src.AgencyId)
                .Map(dest => dest.Name, src => src.Agency == null ? null : src.Agency.Name)
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.AccessRequestAgencyModel, Entity.AccessRequestAgency>()
                .Map(dest => dest.AgencyId, src => src.Id)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
