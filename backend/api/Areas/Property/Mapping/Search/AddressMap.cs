using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Search;

namespace Pims.Api.Areas.Property.Mapping.Search
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAddress, Model.AddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.StreetAddress1, src => src.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.MunicipalityName)
                .Map(dest => dest.RegionId, src => src.RegionCode)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation.RegionName)
                .Map(dest => dest.DistrictId, src => src.DistrictCode)
                .Map(dest => dest.District, src => src.DistrictCodeNavigation.DistrictName)
                .Map(dest => dest.ProvinceId, src => src.ProvinceStateId)
                .Map(dest => dest.Province, src => src.ProvinceState.ProvinceStateCode)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Map(dest => dest.Country, src => src.Country.CountryCode)
                .Map(dest => dest.Postal, src => src.PostalCode);
        }
    }
}
