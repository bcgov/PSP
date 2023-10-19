using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyActivity, PropertyActivityModel>()
                .Map(dest => dest.Id, src => src.PimsPropertyActivityId)
                .Map(dest => dest.ActivityTypeCode, src => src.PropMgmtActivityTypeCode)
                .Map(dest => dest.ActivityType, src => src.PropMgmtActivityTypeCodeNavigation)
                .Map(dest => dest.ActivitySubTypeCode, src => src.PropMgmtActivitySubtypeCode)
                .Map(dest => dest.ActivitySubType, src => src.PropMgmtActivitySubtypeCodeNavigation)
                .Map(dest => dest.ActivityStatusTypeCode, src => src.PropMgmtActivityStatusTypeCode)
                .Map(dest => dest.ActivityStatusType, src => src.PropMgmtActivityStatusTypeCodeNavigation)
                .Map(dest => dest.RequestedAddedDate, src => src.RequestAddedDt)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();
        }
    }
}
