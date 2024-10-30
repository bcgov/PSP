using Mapster;
using Pims.Api.Models.Concepts.Address;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class LeaseStakeholderTypeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLeaseStakeholderType, LeaseStakeholderTypeModel>()
                .Map(dest => dest.Code, src => src.Id)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsPayableRelated, src => src.IsPayableRelated)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Map(dest => dest.DisplayOrder, src => src.DisplayOrder)
                .Inherits<Entity.IBaseEntity, CodeTypeModel>();
        }
    }
}
