using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class ContactMethodMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsContactMethod, Model.ContactMethodModel>()
                .Map(dest => dest.Id, src => src.ContactMethodId)
                .Map(dest => dest.ContactMethodType, src => src.ContactMethodTypeCodeNavigation)
                .Map(dest => dest.Value, src => src.ContactMethodValue)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();
        }
    }
}
