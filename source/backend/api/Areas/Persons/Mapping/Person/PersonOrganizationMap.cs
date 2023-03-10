using Mapster;
using Pims.Api.Areas.Persons.Models.Person;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Persons.Models.Person;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class PersonOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Model.PersonModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.Internal_Id, src => src.PersonOrganizationId)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.PersonOrganizationRowVersion)
                .Map(dest => dest.PersonId, src => src.Id)
                .Map(dest => dest.OrganizationId, src => GetLinkedOrganizationId(src))
                .IgnoreNullValues(true);
        }

        private static long? GetLinkedOrganizationId(PersonModel src)
        {
            return src != null && src.Organization != null ? src.Organization.Id : null;
        }
    }
}
