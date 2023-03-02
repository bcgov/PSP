using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsAddress, AddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.StreetAddress1, src => src.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.MunicipalityName)
                .Map(dest => dest.Province, src => src.ProvinceState)
                .Map(dest => dest.ProvinceStateId, src => src.ProvinceStateId)
                .Map(dest => dest.Country, src => src.Country)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation)
                .Map(dest => dest.District, src => src.DistrictCodeNavigation)
                .Map(dest => dest.CountryOther, src => src.OtherCountry)
                .Map(dest => dest.Postal, src => src.PostalCode)
                .Map(dest => dest.Latitude, src => src.Latitude)
                .Map(dest => dest.Longitude, src => src.Longitude)
                .Map(dest => dest.Comment, src => src.Comment)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<Entity.PimsCountry, CodeTypeModel>()
                .Map(dest => dest.Id, src => src.CountryId)
                .Map(dest => dest.Code, src => src.CountryCode)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.NewConfig<Entity.PimsProvinceState, CodeTypeModel>()
                .Map(dest => dest.Id, src => src.ProvinceStateId)
                .Map(dest => dest.Code, src => src.ProvinceStateCode)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.NewConfig<Entity.PimsDistrict, CodeTypeModel>()
                .Map(dest => dest.Code, src => src.DistrictCode)
                .Map(dest => dest.Description, src => src.DistrictName)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder);

            config.NewConfig<AddressModel, Entity.PimsAddress>()
                .Map(dest => dest.AddressId, src => src.Id)
                .Map(dest => dest.StreetAddress1, src => src.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.StreetAddress3)
                .Map(dest => dest.MunicipalityName, src => src.Municipality)
                .Map(dest => dest.ProvinceStateId, src => src.Province.Id)
                .Map(dest => dest.CountryId, src => src.Country.Id)
                .Map(dest => dest.RegionCode, src => src.Region.Code)
                .Map(dest => dest.DistrictCode, src => src.District.Code)
                .Map(dest => dest.OtherCountry, src => src.CountryOther)
                .Map(dest => dest.PostalCode, src => src.Postal)
                .Map(dest => dest.Latitude, src => src.Latitude)
                .Map(dest => dest.Longitude, src => src.Longitude)
                .Map(dest => dest.Comment, src => src.Comment)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
