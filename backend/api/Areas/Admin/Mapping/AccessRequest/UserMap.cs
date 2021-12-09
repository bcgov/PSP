using Mapster;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.AccessRequest;

namespace Pims.Api.Areas.Admin.Mapping.AccessRequest
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUser, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.GuidIdentifierValue)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifierValue)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.PimsUser>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.GuidIdentifierValue, src => src.KeycloakUserId)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifierValue)
                .Inherits<Api.Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }
    }
}
