using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonAddress, AddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.StreetAddress1, src => src.Address.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.Address.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.Address.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.Address.MunicipalityName)
                .Map(dest => dest.Province, src => src.Address.ProvinceState)
                .Map(dest => dest.Country, src => src.Address.Country)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<Entity.PimsOrganizationAddress, AddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.StreetAddress1, src => src.Address.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.Address.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.Address.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.Address.MunicipalityName)
                .Map(dest => dest.Province, src => src.Address.ProvinceState)
                .Map(dest => dest.Country, src => src.Address.Country)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();
        }
    }
}
