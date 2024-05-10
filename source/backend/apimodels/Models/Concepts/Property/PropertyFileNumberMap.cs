using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class PropertyFileNumberMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsFileNumber, PropertyFileNumberModel>()
                .Map(dest => dest.Id, src => src.FileNumberId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.FileNumberTypeCode, src => src.FileNumberTypeCodeNavigation)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.OtherFileNumberType, src => src.OtherFileNumberType)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<PropertyFileNumberModel, Entity.PimsFileNumber>()
                .Map(dest => dest.FileNumberId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.FileNumber, src => src.FileNumber)
                .Map(dest => dest.FileNumberTypeCode, src => src.FileNumberTypeCode.Id)
                .Map(dest => dest.OtherFileNumberType, src => src.OtherFileNumberType)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
