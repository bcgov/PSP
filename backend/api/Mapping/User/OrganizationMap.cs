using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.User;

namespace Pims.Api.Mapping.User
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Organization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Parent, src => src.Parent)
                .Map(dest => dest.Children, src => src.Children)
                .Map(dest => dest.Users, src => src.Users)
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.Organization>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.ParentId, src => src.Parent == null ? (long?)null : src.Parent.Id)
                .Map(dest => dest.Parent, src => src.Parent)
                .Map(dest => dest.Children, src => src.Children)
                .Map(dest => dest.Users, src => src.Users)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
