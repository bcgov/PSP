using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.AccessRequest
{
    public class RegionUserMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsRegionUser, RegionUserModel>()
                .MaxDepth(3)
                .Map(dest => dest.Id, src => src.RegionUserId)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation)
                .Map(dest => dest.User, src => src.User)
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();

            config.NewConfig<RegionUserModel, Entity.PimsRegionUser>()
                .MaxDepth(3)
                .Map(dest => dest.RegionUserId, src => src.Id)
                .Map(dest => dest.UserId, src => src.UserId)
                .Map(dest => dest.User, src => src.User)
                .Map(dest => dest.RegionCode, src => src.RegionCode)
                .Inherits<Api.Models.BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
