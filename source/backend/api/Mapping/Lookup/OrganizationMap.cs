using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Lookup;

namespace Pims.Api.Mapping.Lookup
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.Name, src => src.OrganizationName)
                .Map(dest => dest.Code, src => src.OrganizationIdentifier)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Type, src => src.GetType().Name)
                .Inherits<Entity.IDisableBaseAppEntity, Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationModel, Entity.PimsOrganization>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.OrganizationName, src => src.Name)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Models.BaseAppModel, Entity.IDisableBaseAppEntity>();
        }
    }
}
