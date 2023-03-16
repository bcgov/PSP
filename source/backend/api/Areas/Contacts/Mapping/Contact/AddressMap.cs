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
                .Map(dest => dest.Province, src => src.Address.ProvinceState)
                .Map(dest => dest.Country, src => src.Address.Country)
                .Map(dest => dest.CountryOther, src => src.Address.OtherCountry)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Map(dest => dest.AddressType, src => src.AddressUsageTypeCodeNavigation);

            config.NewConfig<Entity.PimsOrganizationAddress, Model.AddressModel>()
                .Map(dest => dest.Id, src => src.AddressId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.StreetAddress1, src => src.Address.StreetAddress1)
                .Map(dest => dest.StreetAddress2, src => src.Address.StreetAddress2)
                .Map(dest => dest.StreetAddress3, src => src.Address.StreetAddress3)
                .Map(dest => dest.Municipality, src => src.Address.MunicipalityName)
                .Map(dest => dest.Province, src => src.Address.ProvinceState)
                .Map(dest => dest.Country, src => src.Address.Country)
                .Map(dest => dest.CountryOther, src => src.Address.OtherCountry)
                .Map(dest => dest.Postal, src => src.Address.PostalCode)
                .Map(dest => dest.AddressType, src => src.AddressUsageTypeCodeNavigation);
        }
    }
}
