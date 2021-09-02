using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Property;

namespace Pims.Api.Areas.Property.Mapping.Property
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Address, Model.AddressModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.AddressTypeId, src => src.AddressTypeId)
                .Map(dest => dest.AddressType, src => src.AddressType.Description)
                .Map(dest => dest.StreetAddress1, src => src.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.Municipality)
                .Map(dest => dest.RegionId, src => src.RegionId)
                .Map(dest => dest.Region, src => src.Region.Name)
                .Map(dest => dest.DistrictId, src => src.DistrictId)
                .Map(dest => dest.District, src => src.District.Name)
                .Map(dest => dest.ProvinceId, src => src.ProvinceId)
                .Map(dest => dest.Province, src => src.Province.Code)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Map(dest => dest.Country, src => src.Country.Code)
                .Map(dest => dest.Postal, src => src.Postal);
        }
    }
}
