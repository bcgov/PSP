using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Persons.Models.Person;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class PersonOrganizationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Model.PersonModel, Entity.PimsPersonOrganization>()
                .Map(dest => dest.Id, src => src.PersonOrganizationId)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.PersonOrganizationRowVersion)
                .Map(dest => dest.OrganizationId, src => src.OrganizationId)
                .IgnoreNullValues(true);
        }
    }
}
