using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, Model.PropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.PID, src => src.Pid)
                .Map(dest => dest.PIN, src => src.Pin)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.AreaUnitId, src => src.PropertyAreaUnitTypeCode)
                .Map(dest => dest.AreaUnit, src => src.PropertyAreaUnitTypeCodeNavigation.Description)
                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.SurplusDeclaration, src => src);
        }
    }
}
