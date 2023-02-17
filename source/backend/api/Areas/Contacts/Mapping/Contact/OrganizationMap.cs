using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Contact.Models.Contact;

namespace Pims.Api.Areas.Contact.Mapping.Contact
{
    public class OrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsOrganization, Model.OrganizationModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Alias, src => src.OrganizationAlias)
                .Map(dest => dest.IncorporationNumber, src => src.IncorporationNumber)
                .Map(dest => dest.Addresses, src => src.PimsOrganizationAddresses)
                .Map(dest => dest.ContactMethods, src => src.PimsContactMethods)
                .Map(dest => dest.Persons, src => src.GetPersons())
                .Map(dest => dest.Comment, src => src.Comment);
        }
    }
}
