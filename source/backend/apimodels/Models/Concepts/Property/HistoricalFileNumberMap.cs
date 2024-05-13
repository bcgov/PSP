using Mapster;
using Pims.Api.Models.Base;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Property
{
    public class HistoricalFileNumberMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsHistoricalFileNumber, HistoricalFileNumberModel>()
                .Map(dest => dest.Id, src => src.HistoricalFileNumberId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.HistoricalFileNumberTypeCode, src => src.HistoricalFileNumberTypeCodeNavigation)
                .Map(dest => dest.HistoricalFileNumber, src => src.HistoricalFileNumber)
                .Map(dest => dest.OtherHistFileNumberTypeCode, src => src.OtherHistFileNumberTypeCode)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<Entity.IBaseAppEntity, BaseAuditModel>();

            config.NewConfig<HistoricalFileNumberModel, Entity.PimsHistoricalFileNumber>()
                .Map(dest => dest.HistoricalFileNumberId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.HistoricalFileNumber, src => src.HistoricalFileNumber)
                .Map(dest => dest.HistoricalFileNumberTypeCode, src => src.HistoricalFileNumberTypeCode.Id)
                .Map(dest => dest.OtherHistFileNumberTypeCode, src => src.OtherHistFileNumberTypeCode)
                .Map(dest => dest.IsDisabled, src => src.IsDisabled)
                .Inherits<BaseAuditModel, Entity.IBaseAppEntity>();
        }
    }
}
