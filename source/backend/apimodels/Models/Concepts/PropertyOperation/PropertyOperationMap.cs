using Mapster;
using Pims.Api.Models.Base;
using Pims.Api.Models.Concepts.PropertyOperation;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Note
{
    public class PropertyOperationMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyOperation, PropertyOperationModel>()
                .Map(dest => dest.Id, src => src.PropertyOperationId)
                .Map(dest => dest.SourcePropertyId, src => src.SourcePropertyId)
                .Map(dest => dest.SourceProperty, src => src.SourceProperty)
                .Map(dest => dest.DestinationPropertyId, src => src.DestinationPropertyId)
                .Map(dest => dest.DestinationProperty, src => src.DestinationProperty)
                .Map(dest => dest.PropertyOperationNo, src => src.PropertyOperationNo)
                .Map(dest => dest.PropertyOperationTypeCode, src => src.PropertyOperationTypeCodeNavigation)
                .Map(dest => dest.OperationDt, src => src.OperationDt)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyOperationModel, Entity.PimsPropertyOperation>()
                .Map(dest => dest.PropertyOperationId, src => src.Id)
                .Map(dest => dest.SourcePropertyId, src => src.SourcePropertyId)
                .Map(dest => dest.DestinationPropertyId, src => src.DestinationPropertyId)
                .Map(dest => dest.DestinationProperty, src => src.DestinationProperty)
                .Map(dest => dest.PropertyOperationNo, src => src.PropertyOperationNo)
                .Map(dest => dest.PropertyOperationTypeCode, src => src.PropertyOperationTypeCode.Id)
                .Map(dest => dest.OperationDt, src => src.OperationDt)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
