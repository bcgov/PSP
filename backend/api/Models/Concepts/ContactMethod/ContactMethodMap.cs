using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ContactMethodMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsContactMethod, ContactMethodModel>()
                .Map(dest => dest.Id, src => src.ContactMethodId)
                .Map(dest => dest.ContactMethodType, src => src.ContactMethodTypeCodeNavigation)
                .Map(dest => dest.Value, src => src.ContactMethodValue)
                .Inherits<Entity.IBaseEntity, BaseModel>();
        }
    }
}
