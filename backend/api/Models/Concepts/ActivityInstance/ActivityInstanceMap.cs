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
                .Map(dest => dest.ActivityTemplate, src => src.ActivityTemplate);

            config.NewConfig<ActivityInstanceModel, Entity.PimsActivityInstance>()
                .PreserveReference(true)
                .Map(dest => dest.ActivityInstanceId, src => src.Id)
                .Map(dest => dest.ActivityTemplateId, src => src.ActivityTemplateId);
        }
    }
}
