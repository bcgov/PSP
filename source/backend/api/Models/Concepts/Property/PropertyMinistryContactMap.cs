using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyMinistryContactMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropActMinContact, PropertyMinistryContactModel>()
                .Map(dest => dest.Id, src => src.PropActMinContactId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PropertyActivityId, src => src.PimsPropertyActivityId)
                .Map(dest => dest.PropertyActivity, src => src.PimsPropertyActivity)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<PropertyMinistryContactModel, Entity.PimsPropActMinContact>()
                .Map(dest => dest.PropActMinContactId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PimsPropertyActivityId, src => src.PropertyActivityId)
                .Map(dest => dest.PimsPropertyActivity, src => src.PropertyActivity)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
