using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Persons.Models.Person;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class PersonMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            // TODO: add mappings for related data - contact methods
            config.NewConfig<Model.PersonCreateModel, Entity.PimsPerson>()
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Surname, src => src.Surname)
                .Map(dest => dest.FirstName, src => src.FirstName)
                .Map(dest => dest.MiddleNames, src => src.MiddleNames)
                .Map(dest => dest.PreferredName, src => src.PreferredName)
                .Map(dest => dest.Comment, src => src.Comment)
                .Map(dest => dest.PimsPersonAddresses, src => src.Addresses)
                .IgnoreNullValues(true);
        }
    }
}
