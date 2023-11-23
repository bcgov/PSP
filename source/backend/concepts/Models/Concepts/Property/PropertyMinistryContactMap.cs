using Mapster;
using Pims.Api.Concepts.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Property
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
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyMinistryContactModel, Entity.PimsPropActMinContact>()
                .Map(dest => dest.PropActMinContactId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PimsPropertyActivityId, src => src.PropertyActivityId)
                .Map(dest => dest.PimsPropertyActivity, src => src.PropertyActivity)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
