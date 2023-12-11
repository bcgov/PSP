using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Models.Concepts;

namespace Pims.Api.Models.Concepts
{
    public class PropertyLeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsPropertyLease, Model.PropertyLeaseModel>()
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.Lease, src => src.Lease)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.AreaUnitType, src => src.AreaUnitTypeCodeNavigation)
                .Map(dest => dest.LeaseArea, src => src.LeaseArea)
                .Map(dest => dest.PropertyName, src => src.Name)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.Id, src => src.Internal_Id);

            config.NewConfig<Model.PropertyLeaseModel, Entity.PimsPropertyLease>()
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.LeaseId, src => src.LeaseId)
                .Map(dest => dest.AreaUnitTypeCode, src => src.AreaUnitType.Id)
                .Map(dest => dest.LeaseArea, src => src.LeaseArea)
                .Map(dest => dest.Name, src => src.PropertyName)
                .Map(dest => dest.ConcurrencyControlNumber, src => src.RowVersion)
                .Map(dest => dest.Internal_Id, src => src.Id);
        }
    }
}
