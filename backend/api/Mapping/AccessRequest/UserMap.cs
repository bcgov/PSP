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
            config.NewConfig<Entity.PimsUser, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.GuidIdentifierValue)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifierValue)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Inherits<Entity.IDisableBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.PimsUser>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.GuidIdentifierValue, src => src.KeycloakUserId)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifier)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Map(dest => dest.Position, src => src.Position)
                .Inherits<Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }
    }
}
