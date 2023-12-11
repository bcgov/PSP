using System.Linq;
using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using KModel = Pims.Keycloak.Models;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
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
                .Map(dest => dest.MiddleNames, src => src.Person.MiddleNames)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Organizations, src => src.PimsUserOrganizations.OrderBy(o => o.Organization != null ? o.Organization.PrntOrganizationId : null))
                .Map(dest => dest.Roles, src => src.PimsUserRoles)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.PimsUser>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.GuidIdentifierValue, src => src.KeycloakUserId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifier)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Map(dest => dest.PimsUserOrganizations, src => src.Organizations)
                .Map(dest => dest.PimsUserRoles, src => src.Roles)
                .Inherits<Api.Models.BaseAppModel, Entity.IDisableBaseAppEntity>();

            config.NewConfig<Entity.PimsUser, KModel.UserModel>()
                .Map(dest => dest.Username, src => src.BusinessIdentifierValue)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.LastName, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail());
        }
    }
}
