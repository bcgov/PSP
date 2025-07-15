using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyMinistryContactMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropActMinContact, PropertyMinistryContactModel>()
                .Map(dest => dest.Id, src => src.PropActMinContactId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.ManagementActivityId, src => src.PimsManagementActivityId)
                .Map(dest => dest.ManagementActivity, src => src.PimsManagementActivity)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyMinistryContactModel, Entity.PimsPropActMinContact>()
                .Map(dest => dest.PropActMinContactId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.PimsManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.PimsManagementActivity, src => src.ManagementActivity)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
