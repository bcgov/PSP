
using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class HistoricalNumberMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsFileNumber, HistoricalNumberModel>()
                .Map(dest => dest.Id, src => src.FileNumberId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.HistoricalNumber, src => src.FileNumber)
                .Map(dest => dest.HistoricalNumberType, src => src.FileNumberTypeCodeNavigation)
                .Map(dest => dest.OtherHistoricalNumberType, src => src.OtherFileNumberType)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<HistoricalNumberModel, Entity.PimsFileNumber>()
                .Map(dest => dest.FileNumberId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.FileNumber, src => src.HistoricalNumber)
                .Map(dest => dest.FileNumberTypeCode, src => src.HistoricalNumberType.Id)
                .Map(dest => dest.OtherFileNumberType, src => src.OtherHistoricalNumberType)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
