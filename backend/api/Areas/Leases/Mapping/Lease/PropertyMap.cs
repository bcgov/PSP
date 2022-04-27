using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Lease.Models.Lease;

namespace Pims.Api.Areas.Lease.Mapping.Lease
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyLease, Model.PropertyModel>()
                .Map(dest => dest, src => src.Property)
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.PID, src => src.Property.Pid)
                .Map(dest => dest.PIN, src => src.Property.Pin)
                .Map(dest => dest.Name, src => src.Property.Name)
                .Map(dest => dest.AreaUnitType, src => src.AreaUnitTypeCodeNavigation)
                .Map(dest => dest.Address, src => src.Property.Address)
                .Map(dest => dest.IsSensitive, src => src.Property.IsSensitive)
                .Map(dest => dest.Description, src => src.Property.Description)
                .Map(dest => dest.SurplusDeclaration, src => src)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.LandArea, src => src.LeaseArea);

            config.NewConfig<Model.PropertyModel,Entity.PimsPropertyLease>()
                .Map(dest => dest.PropertyId, src => src.Id)
                .Map(dest => dest.Property, src => src)
                .Map(dest => dest.LeaseArea, src => src.LandArea)
                .Map(dest => dest.AreaUnitTypeCode, src => src.AreaUnitType.Id)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion);

            config.NewConfig<Model.PropertyModel, Entity.PimsProperty> ()
                .Map(dest => dest.PropertyId, src => src.Id)
                .Map(dest => dest.Pid, src => src.PID == string.Empty ? "-1" : src.PID)
                .Map(dest => dest.Pin, src => src.PIN)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion);
        }
    }
}
