using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AccessRequest
{
    public class UserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsUser, UserModel>()
                .Map(dest => dest.Id, src => src.UserId)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifierValue)
                .Map(dest => dest.GuidIdentifierValue, src => src.GuidIdentifierValue)
                .Map(dest => dest.ApprovedById, src => src.ApprovedById)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.UserRoles, src => src.PimsUserRoles)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.IssueDate, src => src.IssueDate)
                .Map(dest => dest.LastLogin, src => src.LastLogin)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<UserModel, Entity.PimsUser>()
                .Map(dest => dest.UserId, src => src.Id)
                .Map(dest => dest.BusinessIdentifierValue, src => src.BusinessIdentifierValue)
                .Map(dest => dest.GuidIdentifierValue, src => src.GuidIdentifierValue)
                .Map(dest => dest.ApprovedById, src => src.ApprovedById)
                .Map(dest => dest.Position, src => src.Position)
                .Map(dest => dest.Note, src => src.Note)
                .Map(dest => dest.PimsUserRoles, src => src.UserRoles)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.IssueDate, src => src.IssueDate)
                .Map(dest => dest.LastLogin, src => src.LastLogin)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
