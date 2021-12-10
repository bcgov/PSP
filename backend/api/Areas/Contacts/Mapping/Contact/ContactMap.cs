using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class ContactMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPerson, Model.ContactModel>()
                .Map(dest => dest.Person, src => src);

            config.NewConfig<Entity.PimsOrganization, Model.ContactModel>()
                .Map(dest => dest.Organization, src => src);
        }
    }
}
