using Mapster;
using Pims.Dal.Helpers.Extensions;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Property;

namespace Pims.Api.Areas.Property.Mapping.Property
{
    public class LeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsLease, Model.LeaseModel>()
                .Map(dest => dest.Id, src => src.LeaseId)
                .Map(dest => dest.ExpiryDate, src => src.GetExpiryDate())
                .Map(dest => dest.TenantName, src => src.GetFullName());
        }
    }
}
