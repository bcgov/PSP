using Mapster;
using Pims.Api.Helpers.Extensions;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class ContactMethodMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Pims.Api.Models.Contact.ContactMethodModel, Entity.PimsContactMethod>()
                .Map(dest => dest.ContactMethodId, src => src.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.ContactMethodTypeCode, src => src.ContactMethodTypeCode.GetTypeId())
                .Map(dest => dest.ContactMethodValue, src => src.Value)
                .IgnoreNullValues(true);
        }
    }
}
