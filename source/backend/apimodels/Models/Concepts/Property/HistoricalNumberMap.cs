
using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class HistoricalNumberMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsHistoricalFileNumber, HistoricalNumberModel>()
                .Map(dest => dest.Id, src => src.HistoricalFileNumberId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.HistoricalNumber, src => src.HistoricalFileNumber)
                .Map(dest => dest.HistoricalNumberType, src => src.HistoricalFileNumberTypeCodeNavigation)
                .Map(dest => dest.OtherHistoricalNumberType, src => src.OtherHistFileNumberTypeCode)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<HistoricalNumberModel, Entity.PimsHistoricalFileNumber>()
                .Map(dest => dest.HistoricalFileNumberId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.HistoricalFileNumber, src => src.HistoricalNumber)
                .Map(dest => dest.HistoricalFileNumberTypeCode, src => src.HistoricalNumberType.Id)
                .Map(dest => dest.OtherHistFileNumberTypeCode, src => src.OtherHistoricalNumberType)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseConcurrentModel, Entity.IBaseEntity>();
        }
    }
}
