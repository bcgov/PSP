using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class ActivityTemplateMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsActivityTemplate, ActivityTemplateModel>()
                .PreserveReference(true)
                .Map(dest => dest.Id, src => src.ActivityTemplateId)
                .Map(dest => dest.ActivityTemplateTypeCode, src => src.ActivityTemplateTypeCodeNavigation);

        }
    }
}
