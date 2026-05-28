using Mapster;
using Pims.Api.Models.Base;
using Pims.Core.Extensions;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyImprovementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsPropertyImprovement, PropertyImprovementModel>()
                .Map(dest => dest.Id, src => src.PropertyImprovementId)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.ImprovementTypeCode, src => src.PropertyImprovementTypeCodeNavigation)
                .Map(dest => dest.ImprovementName, src => src.ImprovementName)
                .Map(dest => dest.ImprovementDescription, src => src.ImprovementDescription)
                .Map(dest => dest.ImprovementStatusCode, src => src.PropImprvmntStatusTypeCodeNavigation)
                .Map(dest => dest.ImprovementDate, src => src.ImprovementDate.ToNullableDateOnly())
                .Inherits<IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyImprovementModel, PimsPropertyImprovement>()
                .Map(dest => dest.PropertyImprovementId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyImprovementTypeCode, src => src.ImprovementTypeCode.Id)
                .Map(dest => dest.ImprovementName, src => src.ImprovementName)
                .Map(dest => dest.ImprovementDescription, src => src.ImprovementDescription)
                .Map(dest => dest.PropImprvmntStatusTypeCode, src => src.ImprovementStatusCode.Id)
                .Map(dest => dest.ImprovementDate, src => src.ImprovementDate.ToNullableDateTime())
                .Inherits<BaseConcurrentModel, IBaseEntity>();
        }
    }
}
