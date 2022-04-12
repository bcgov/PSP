using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, PropertyModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.Pin, src => src.Pin)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.District, src => src.DistrictCodeNavigation)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation)
                .Map(dest => dest.Location, src => src.Location)
                .Inherits<Entity.IBaseEntity, BaseModel>();

            config.NewConfig<PropertyModel, Entity.PimsProperty>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.Pin, src => src.Pin)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.DistrictCode, src => src.District.Code)
                .Map(dest => dest.RegionCode, src => src.Region.Code)
                .Map(dest => dest.Location, src => src.Location)
                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
