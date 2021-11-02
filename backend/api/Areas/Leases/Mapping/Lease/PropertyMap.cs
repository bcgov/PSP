using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Property, Model.PropertyModel>()
                .Map(dest => dest.Id, src => src.Id)
                .Map(dest => dest.PID, src => src.PID)
                .Map(dest => dest.PIN, src => src.PIN)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.AreaUnitId, src => src.AreaUnitId)
                .Map(dest => dest.AreaUnit, src => src.AreaUnit.Description)
                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.RowVersion, src => src.RowVersion)
                .Map(dest => dest.SurplusDeclaration, src => src);
        }
    }
}
