using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Areas.Persons.Models.Person.AddressCreateModel, Entity.PimsPersonAddress>()
                .Map(dest => dest.AddressId, src => src.Id)
                .Map(dest => dest.Address, src => src)
                .IgnoreNullValues(true);

            config.NewConfig<Areas.Persons.Models.Person.AddressCreateModel, Entity.PimsAddress>()
                .Map(dest => dest.AddressId, src => src.Id)
                .Map(dest => dest.StreetAddress1, src => src.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.StreetAddress3)
                .Map(dest => dest.MunicipalityName, src => src.Municipality)
                .Map(dest => dest.RegionCode, src => src.RegionId)
                .Map(dest => dest.DistrictCode, src => src.DistrictId)
                .Map(dest => dest.ProvinceStateId, src => src.ProvinceId)
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Map(dest => dest.PostalCode, src => src.Postal)
                .Map(dest => dest.OtherCountry, src => src.CountryOther)
                .IgnoreNullValues(true);
        }
    }
}
