using Mapster;
using Pims.Api.Models.Base;
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
                .Map(dest => dest.ImprovementDescription, src => src.ImprovementDescription)
                .Map(dest => dest.PropertyImprovementTypeCode, src => src.PropertyImprovementTypeCodeNavigation)
                .Inherits<IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyImprovementModel, PimsPropertyImprovement>()
                .Map(dest => dest.PropertyImprovementId, src => src.Id)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyImprovementTypeCode, src => src.PropertyImprovementTypeCode.Id)
                .Map(dest => dest.ImprovementDescription, src => src.ImprovementDescription)
                .Inherits<BaseConcurrentModel, IBaseEntity>();
        }
    }
}
