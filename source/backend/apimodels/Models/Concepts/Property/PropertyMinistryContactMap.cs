using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyMinistryContactMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsMgmtActMinContact, PropertyMinistryContactModel>()
                .Map(dest => dest.Id, src => src.MgmtActMinContactId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementActivity, src => src.ManagementActivity)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyMinistryContactModel, Entity.PimsMgmtActMinContact>()
                .Map(dest => dest.MgmtActMinContactId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.ManagementActivityId, src => src.ManagementActivityId)
                .Map(dest => dest.ManagementActivity, src => src.ManagementActivity)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
