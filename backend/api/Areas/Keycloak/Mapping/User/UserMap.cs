using Mapster;
using Pims.Dal.Entities;
using System;
using System.Linq;
using Entity = Pims.Dal.Entities;
using KModel = Pims.Keycloak.Models;
using Model = Pims.Api.Areas.Keycloak.Models.User;

namespace Pims.Api.Areas.Keycloak.Mapping.User
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
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.MiddleNames, src => src.Person.MiddleNames)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Organizations, src => src.OrganizationsManyToMany.OrderBy(a => a.Organization.ParentId))
                .Map(dest => dest.Roles, src => src.RolesManyToMany)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.UserModel, Entity.User>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.KeycloakUserId, src => src.KeycloakUserId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifier)
                .Map(dest => dest.Person.FirstName, src => src.FirstName)
                .Map(dest => dest.Person.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.Person.Surname, src => src.Surname)
                .Map(dest => dest.OrganizationsManyToMany, src => src.Organizations)
                .Map(dest => dest.RolesManyToMany, src => src.Roles)
                .Inherits<Api.Models.BaseAppModel, Entity.BaseAppEntity>();

            config.NewConfig<Entity.User, KModel.UserModel>()
                .Map(dest => dest.Id, src => src.KeycloakUserId)
                .Map(dest => dest.Username, src => src.BusinessIdentifier)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.LastName, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Enabled, src => !src.IsDisabled)
                .Inherits<Entity.BaseAppEntity, Object>();
        }
    }
}
