using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class OrganizationAddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganizationAddress, Model.OrganizationAddressModel>()
                .Map(dest => dest.Id, src => src.OrganizationAddressId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Organization, src => src.Organization)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.AddressUsageType, src => src.AddressUsageTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.OrganizationAddressModel, Entity.PimsOrganizationAddress>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id)
                .Map(dest => dest.AddressId, src => src.Address.Id)
                .Map(dest => dest.AddressUsageTypeCode, src => src.AddressUsageType.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
