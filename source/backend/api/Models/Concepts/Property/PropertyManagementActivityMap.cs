using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyManagementActivityMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropActivity, PropertyManagementActivityModel>()
                .Map(dest => dest.Id, src => src.PropPropActivityId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyActivityId, src => src.PimsPropertyActivityId)
                .Map(dest => dest.Activity, src => src.PimsPropertyActivity)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();
        }
    }
}
