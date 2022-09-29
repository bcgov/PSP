using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Search;

namespace Pims.Api.Areas.Lease.Mapping.Search
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, Model.PropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.Pin, src => src.Pin)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.Address, src => src.Address);
        }
    }
}
