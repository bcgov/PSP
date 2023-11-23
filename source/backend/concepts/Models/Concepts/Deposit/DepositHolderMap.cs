using Mapster;
using Pims.Api.Concepts.Models.Concepts.Contact;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Deposit
{
    public class DepositHolderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDepositHolder, ContactModel>()
                .Map(dest => dest.Id, src => src.PersonId != null ? 'P' + src.PersonId.ToString() : 'O' + src.OrganizationId.ToString())
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Organization, src => src.Organization);

            config.NewConfig<ContactModel, Entity.PimsSecurityDepositHolder>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id);

            config.NewConfig<Entity.PimsSecurityDepositReturnHolder, ContactModel>()
                .Map(dest => dest.Id, src => src.PersonId != null ? 'P' + src.PersonId.ToString() : 'O' + src.OrganizationId.ToString())
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Organization, src => src.Organization);

            config.NewConfig<ContactModel, Entity.PimsSecurityDepositReturnHolder>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id);
        }
    }
}
