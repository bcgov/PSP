using Mapster;
using Pims.Dal.Entities;
using System;
using System.Linq;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Reports.Models.User;

namespace Pims.Api.Areas.Reports.Mapping.User
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUser, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifierValue)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Organizations, src => String.Join(",", src.GetOrganizations().Select(o => o.OrganizationName)))
                .Map(dest => dest.Roles, src => String.Join(",", src.GetRoles().Select(r => r.Name)))
                .Map(dest => dest.ApprovedBy, src => src.ApprovedById)
                .Map(dest => dest.IssueOn, src => src.IssueDate)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();
        }
    }
}
