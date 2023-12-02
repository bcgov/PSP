using Mapster;
using Pims.Api.Helpers.Extensions;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Areas.Persons.Mapping.Person
{
    public class ContactMethodMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsContactMethod, Pims.Api.Models.Contact.ContactMethodModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.ContactMethodTypeCode, src => src.ContactMethodTypeCodeNavigation)
                .Map(dest => dest.Value, src => src.ContactMethodValue)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<Pims.Api.Models.Contact.ContactMethodModel, Entity.PimsContactMethod>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.ContactMethodTypeCode, src => src.ContactMethodTypeCode.GetTypeId())
                .Map(dest => dest.ContactMethodValue, src => src.Value)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>()
                .IgnoreNullValues(true);
        }
    }
}
