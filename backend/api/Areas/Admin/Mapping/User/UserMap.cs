using Mapster;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;
using System.Linq;

namespace Pims.Api.Areas.Admin.Mapping.User
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUser, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.GuidIdentifierValue)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifierValue)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.MiddleNames, src => src.Person.MiddleNames)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.LastLogin, src => src.LastLogin)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Organizations, src => src.GetOrganizations().OrderBy(o => o != null ? o.PrntOrganizationId : null))
                .Map(dest => dest.Roles, src => src.GetRoles())
                .Map(dest => dest.AppCreateTimestamp, src => src.AppCreateTimestamp)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.PimsUser>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.GuidIdentifierValue, src => src.KeycloakUserId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifier)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Map(dest => dest.PimsUserOrganizations, src => src.Organizations)
                .Map(dest => dest.PimsUserRoles, src => src.Roles)
                .AfterMappingInline((m, e) => UpdateUser(m, e))
                .Inherits<Api.Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }

        private static void UpdateUser(Model.UserModel model, Entity.PimsUser entity)
        {
            entity.PimsUserOrganizations.Where(a => a != null).ForEach(a => {
                a.UserId = entity.Id;
                a.RoleId = model.Roles.FirstOrDefault()?.Id ?? 0;
            });
            entity.PimsUserRoles.Where(r => r?.RoleId != null).ForEach(r => r.UserId = entity.Id);
            if (model?.Email?.Length > 0)
            {
                entity.Person.PimsContactMethods.Add(new PimsContactMethod(entity.Person, null, ContactMethodTypes.WorkEmail, model.Email));
            }
        }
    }
}
