using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Organization
{
    public class OrganizationAddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganizationAddress, OrganizationAddressModel>()
                .Map(dest => dest.Id, src => src.OrganizationAddressId)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.AddressUsageType, src => src.AddressUsageTypeCodeNavigation)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<OrganizationAddressModel, Entity.PimsOrganizationAddress>()
                .Map(dest => dest.OrganizationAddressId, src => src.Id)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .Map(dest => dest.AddressId, src => src.Address.Id)
                .Map(dest => dest.AddressUsageTypeCode, src => src.AddressUsageType.Id)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
