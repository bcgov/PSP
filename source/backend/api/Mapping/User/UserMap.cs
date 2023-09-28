using System.Linq;
using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.User;

namespace Pims.Api.Mapping.User
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUser, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.KeycloakUserId, src => src.GuidIdentifierValue)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifierValue)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Organizations, src => src.GetOrganizations())
                .Map(dest => dest.Roles, src => src.PimsUserRoles.Select(r => r.Role))
                .Inherits<Entity.IDisableBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.PimsUser>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.GuidIdentifierValue, src => src.KeycloakUserId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifier)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.PimsUserOrganizations, src => src.Organizations)
                .Inherits<Models.BaseAppModel, Entity.IDisableBaseAppEntity>();

            config.NewConfig<Model.UserModel, Entity.PimsUserOrganization>()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.User, src => src);

            config.NewConfig<Model.UserModel, Entity.PimsUserRole>()
                .Map(dest => dest.RoleId, src => src.Id)
                .Map(dest => dest.Role, src => src);
        }
    }
}
