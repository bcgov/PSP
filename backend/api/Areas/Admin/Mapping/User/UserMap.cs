using Mapster;
using System.Linq;
using Pims.Core.Extensions;
using Pims.Dal.Entities;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Admin.Models.User;

namespace Pims.Api.Areas.Admin.Mapping.User
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.User, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.KeycloakUserId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifier)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.MiddleNames, src => src.Person.MiddleNames)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.LastLogin, src => src.LastLogin)
                .Map(dest => dest.Email, src => src.Person.GetEmail())
                .Map(dest => dest.Organizations, src => src.OrganizationsManyToMany.OrderBy(a => a.Organization != null ? a.Organization.ParentId : null))
                .Map(dest => dest.Roles, src => src.Roles)
                .Map(dest => dest.CreatedOn, src => src.CreatedOn)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.User>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.KeycloakUserId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifier)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Map(dest => dest.OrganizationsManyToMany, src => src.Organizations)
                .Map(dest => dest.RolesManyToMany, src => src.Roles)
                .AfterMappingInline((m, e) => UpdateUser(m, e))
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();
        }

        private static void UpdateUser(Model.UserModel model, Entity.User entity)
        {
            entity.Organizations.Where(a => a != null).ForEach(a => a.Id = entity.Id);
            entity.Roles.Where(r => r != null).ForEach(r => r.Id = entity.Id);
            if (model?.Email?.Length > 0)
            {
                entity.Person.ContactMethods.Add(new ContactMethod(entity.Person, null, ContactMethodTypes.WorkEmail, model.Email));
            }
        }
    }
}
