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
            config.NewConfig<Entity.User, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifier)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetEmail())
                .Map(dest => dest.Organizations, src => String.Join(",", src.Organizations.Select(a => a.Name)))
                .Map(dest => dest.Roles, src => String.Join(",", src.Roles.Select(a => a.Name)))
                .Map(dest => dest.ApprovedBy, src => src.ApprovedBy)
                .Map(dest => dest.IssueOn, src => src.IssueOn)
                .Inherits<Entity.BaseAppEntity, Api.Models.BaseAppModel>();
        }
    }
}
