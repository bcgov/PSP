using Mapster;
using Pims.Api.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Persons.Models.Person;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonAddress, Model.PersonAddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.RowVersion, src => src.Address.ConcurrencyControlNumber)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PersonAddressId, src => src.PersonAddressId)
                .Map(dest => dest.PersonAddressRowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.StreetAddress1, src => src.Address.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.Address.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.Address.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.Address.MunicipalityName)
                .Map(dest => dest.RegionId, src => src.Address.RegionCode)
                .Map(dest => dest.DistrictId, src => src.Address.DistrictCode)
                .Map(dest => dest.ProvinceId, src => src.Address.ProvinceStateId)
                .Map(dest => dest.CountryId, src => src.Address.CountryId)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Map(dest => dest.CountryOther, src => src.Address.OtherCountry)
                .Map(dest => dest.AddressTypeId, src => src.AddressUsageTypeCodeNavigation);

            config.NewConfig<Model.PersonAddressModel, Entity.PimsPersonAddress>()
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.PersonAddressId, src => src.PersonAddressId)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.PersonAddressRowVersion)
                .Map(dest => dest.AddressUsageTypeCode, src => src.AddressTypeId.GetTypeId())
                .Map(dest => dest.AddressId, src => src.Id)
                .Map(dest => dest.Address, src => src)
                .IgnoreNullValues(true);

            config.NewConfig<Model.PersonAddressModel, Entity.PimsAddress>()
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
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>()
                .IgnoreNullValues(true);
        }
    }
}
