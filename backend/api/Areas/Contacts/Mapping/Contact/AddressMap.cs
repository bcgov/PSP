using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonAddress, Model.AddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.StreetAddress1, src => src.Address.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.Address.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.Address.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.Address.MunicipalityName)
                .Map(dest => dest.RegionId, src => src.Address.RegionCode)
                .Map(dest => dest.Region, src => src.Address.RegionCodeNavigation.RegionName)
                .Map(dest => dest.DistrictId, src => src.Address.DistrictCode)
                .Map(dest => dest.District, src => src.Address.DistrictCodeNavigation.DistrictName)
                .Map(dest => dest.ProvinceId, src => src.Address.ProvinceStateId)
                .Map(dest => dest.Province, src => src.Address.ProvinceState.Description)
                .Map(dest => dest.ProvinceCode, src => src.Address.ProvinceState.ProvinceStateCode)
                .Map(dest => dest.CountryId, src => src.Address.CountryId)
                .Map(dest => dest.Country, src => src.Address.Country.Description)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Map(dest => dest.AddressType, src => src.AddressUsageTypeCodeNavigation);
        }
    }
}
