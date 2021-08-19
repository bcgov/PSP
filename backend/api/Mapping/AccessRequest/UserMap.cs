using Mapster;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.AccessRequest;

namespace Pims.Api.Mapping.AccessRequest
{
    public class AccessRequestUserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.User, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.KeycloakUserId)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifier)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetEmail())
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.User>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.KeycloakUserId)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifier)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
