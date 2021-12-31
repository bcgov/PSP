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
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.PropertyImprovementTypeId, src => src.PropertyImprovementTypeCode)
                .Map(dest => dest.PropertyImprovementType, src => src.PropertyImprovementTypeCodeNavigation.Description)
                .Map(dest => dest.Description, src => src.ImprovementDescription)
                .Map(dest => dest.StructureSize, src => src.StructureSize)
                .Map(dest => dest.Address, src => src.Address);
        }
    }
}
