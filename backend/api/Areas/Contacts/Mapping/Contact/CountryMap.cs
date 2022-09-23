using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class CountryMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsCountry, Model.CountryModel>()
                .Map(dest => dest.CountryId, src => src.CountryId)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.CountryCode, src => src.CountryCode);
        }
    }
}
