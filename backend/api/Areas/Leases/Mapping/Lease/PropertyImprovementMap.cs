using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PropertyImprovementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyImprovement, Model.PropertyImprovementModel>()
                .Map(dest => dest.Id, src => src.PropertyImprovementId)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.PropertyImprovementTypeId, src => src.PropertyImprovementTypeCode)
                .Map(dest => dest.PropertyImprovementType, src => src.PropertyImprovementTypeCodeNavigation.Description)
                .Map(dest => dest.Description, src => src.ImprovementDescription)
                .Map(dest => dest.StructureSize, src => src.StructureSize)
                .Map(dest => dest.Address, src => src.Address);

            config.NewConfig<Model.PropertyImprovementModel, Entity.PimsPropertyImprovement>()
                .Map(dest => dest.PropertyImprovementId, src => src.Id)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.PropertyImprovementTypeCode, src => src.PropertyImprovementTypeId)
                .Map(dest => dest.ImprovementDescription, src => src.Description)
                .Map(dest => dest.StructureSize, src => src.StructureSize)
                .Map(dest => dest.Address, src => src.Address);
        }
    }
}
