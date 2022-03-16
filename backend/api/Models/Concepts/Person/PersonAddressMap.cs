using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class PersonAddressMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPersonAddress, Model.PersonAddressModel>()
                .Map(dest => dest.Id, src => src.PersonId)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.Person, src => src.Person)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.AddressUsageType, src => src.AddressUsageTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<Model.PersonAddressModel, Entity.PimsPersonAddress>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.PersonId, src => src.Person.Id)
                .Map(dest => dest.AddressId, src => src.Address.Id)
                .Map(dest => dest.AddressUsageTypeCode, src => src.AddressUsageType.Id)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
