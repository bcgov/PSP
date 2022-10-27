using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class ContactMethodMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsContactMethod, Model.ContactMethodModel>()
                .Map(dest => dest.Id, src => src.ContactMethodId)
                .Map(dest => dest.ContactMethodType, src => src.ContactMethodTypeCodeNavigation)
                .Map(dest => dest.Value, src => src.ContactMethodValue)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber);
        }
    }
}
