using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Organization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Code, src => src.Identifier)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.BaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.Organization>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Models.BaseAppModel, Entity.BaseAppEntity>();
        }
    }
}
