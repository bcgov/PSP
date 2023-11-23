using Mapster;
using Pims.Dal.Entities;

namespace Pims.Api.Concepts.Models.Concepts.Lease
{
    public class PropertyImprovementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsPropertyImprovement, PropertyImprovementModel>()
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.Lease, src => src.Lease)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.StructureSize, src => src.StructureSize)
                .Map(dest => dest.ImprovementDescription, src => src.ImprovementDescription)
                .Map(dest => dest.PropertyImprovementTypeCode, src => src.PropertyImprovementTypeCodeNavigation)
                .Map(dest => dest.Id, src => src.Internal_Id);

            config.NewConfig<PropertyImprovementModel, PimsPropertyImprovement>()
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.Lease, src => src.Lease)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.StructureSize, src => src.StructureSize)
                .Map(dest => dest.ImprovementDescription, src => src.ImprovementDescription)
                .Map(dest => dest.PropertyImprovementTypeCode, src => src.PropertyImprovementTypeCode.Id)
                .Map(dest => dest.Internal_Id, src => src.Id);
        }
    }
}
