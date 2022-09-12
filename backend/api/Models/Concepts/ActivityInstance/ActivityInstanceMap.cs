using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ActivityInstanceMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsActivityInstance, ActivityInstanceModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ActivityInstanceId)
                .Map(dest => dest.ActivityTemplateId, src => src.ActivityTemplateId)
                .Map(dest => dest.ActivityStatusTypeCode, src => src.ActivityInstanceStatusTypeCodeNavigation)
                .Map(dest => dest.ActivityTemplate, src => src.ActivityTemplate)
                .Map(dest => dest.ActivityDataJson, src => src.ActivityDataJson)
                .Map(dest => dest.Description, src => src.Description)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<ActivityInstanceModel, Entity.PimsActivityInstance>()
                .PreserveReference(true)
                .Map(dest => dest.ActivityInstanceId, src => src.Id)
                .Map(dest => dest.ActivityTemplateId, src => src.ActivityTemplateId)
                .Map(dest => dest.ActivityInstanceStatusTypeCode, src => src.ActivityStatusTypeCode.Id)
                .Map(dest => dest.ActivityDataJson, src => src.ActivityDataJson)
                .Map(dest => dest.Description, src => src.Description)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
