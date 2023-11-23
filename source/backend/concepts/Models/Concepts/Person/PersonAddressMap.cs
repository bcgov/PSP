using Mapster;
using Pims.Api.Concepts.Models.Base;
using Pims.Api.Concepts.Models.Concepts.Person;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PersonAddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonAddress, PersonAddressModel>()
                .Map(dest => dest.Id, src => src.PersonAddressId)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.AddressUsageType, src => src.AddressUsageTypeCodeNavigation)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PersonAddressModel, Entity.PimsPersonAddress>()
                .Map(dest => dest.PersonAddressId, src => src.Id)
                .Map(dest => dest.PersonId, src => src.PersonId)
                .Map(dest => dest.AddressId, src => src.Address.Id)
                .Map(dest => dest.AddressUsageTypeCode, src => src.AddressUsageType.Id)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
