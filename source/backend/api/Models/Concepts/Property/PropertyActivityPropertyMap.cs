using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyActivityPropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropActivity, PropertyActivityPropertyModel>()
                .Map(dest => dest.Id, src => src.PropPropActivityId)
                .Map(dest => dest.PropertyActivityId, src => src.PimsPropertyActivityId)
                .Map(dest => dest.PropertyActivity, src => src.PimsPropertyActivity)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PropertyActivityPropertyModel, Entity.PimsPropPropActivity>()
                .Map(dest => dest.PropPropActivityId, src => src.Id)
                .Map(dest => dest.PimsPropertyActivityId, src => src.PropertyActivityId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
