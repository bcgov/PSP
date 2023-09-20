using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyManagementPurposeMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropPropPurpose, PropertyManagementPurposeModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyPurposeTypeCode, src => src.PropertyPurposeTypeCodeNavigation)
                .Inherits<Entity.IBaseAppEntity, BaseAppModel>();

            config.NewConfig<PropertyManagementPurposeModel, Entity.PimsPropPropPurpose>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.PropertyPurposeTypeCode, src => src.PropertyPurposeTypeCode.Id)
                .Inherits<BaseAppModel, Entity.IBaseAppEntity>();
        }
    }
}
