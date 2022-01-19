using Mapster;
using Pims.Api.Helpers.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Organizations.Mapping.Organization
{
    public class AddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Pims.Api.Models.Contact.AddressModel, Entity.PimsOrganizationAddress>()
                .Map(dest => dest.AddressId, src => src.Id)
                .Map(dest => dest.AddressUsageTypeCode, src => src.AddressTypeId.GetTypeId())
                .Map(dest => dest.Address, src => src)
                .IgnoreNullValues(true);

            config.NewConfig<Pims.Api.Models.Contact.AddressModel, Entity.PimsAddress>()
                .Map(dest => dest.AddressId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
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
