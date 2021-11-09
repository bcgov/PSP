using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PropertyImprovementMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PropertyImprovement, Model.PropertyImprovementModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.PropertyImprovementTypeId, src => src.PropertyImprovementTypeId)
                .Map(dest => dest.PropertyImprovementType, src => src.PropertyImprovementType.Description)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.StructureSize, src => src.StructureSize)
                .Map(dest => dest.Unit, src => src.Unit);
        }
    }
}
