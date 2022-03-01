
using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class DepositHolderMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsSecurityDepositHolder, Model.ContactModel>()
                .Map(dest => dest.Id, src => src.PersonId != null ? 'P' + src.PersonId.ToString() : 'O' + src.OrganizationId.ToString())
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Organization, src => src.Organization);

            config.NewConfig<Model.ContactModel, Entity.PimsSecurityDepositHolder>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id);

            config.NewConfig<Entity.PimsSecurityDepositReturnHolder, Model.ContactModel>()
                .Map(dest => dest.Id, src => src.PersonId != null ? 'P' + src.PersonId.ToString() : 'O' + src.OrganizationId.ToString())
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Organization, src => src.Organization);

            config.NewConfig<Model.ContactModel, Entity.PimsSecurityDepositReturnHolder>()
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.OrganizationId, src => src.Organization.Id);
        }
    }
}
