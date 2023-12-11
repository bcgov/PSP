using System.Linq;
using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Reports.Models.User;

namespace Pims.Api.Areas.Reports.Mapping.User
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUser, Model.UserModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.BusinessIdentifier, src => src.BusinessIdentifierValue)
                .Map(dest => dest.FirstName, src => src.Person.FirstName)
                .Map(dest => dest.Surname, src => src.Person.Surname)
                .Map(dest => dest.Email, src => src.Person.GetWorkEmail())
                .Map(dest => dest.Regions, src => string.Join(",", src.PimsRegionUsers.Select(ur => ur.RegionCodeNavigation.Description)))
                .Map(dest => dest.Roles, src => string.Join(",", src.GetRoles().Select(r => r.Name)))
                .Map(dest => dest.UserType, src => src.UserTypeCodeNavigation.Description)
                .Map(dest => dest.ApprovedBy, src => src.ApprovedById)
                .Map(dest => dest.IssueOn, src => src.IssueDate)
                .Inherits<Entity.IDisableBaseAppEntity, Api.Models.BaseAppModel>();
        }
    }
}
