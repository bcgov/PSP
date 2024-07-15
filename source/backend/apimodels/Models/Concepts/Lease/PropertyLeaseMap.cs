using Mapster;
using Pims.Api.Models.Base;
using Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts.Lease
{
    public class PropertyLeaseMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<PimsPropertyLease, PropertyLeaseModel>()
                .PreserveReference(true)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.PropertyId, src => src.PropertyId)
                .Map(dest => dest.File, src => src.Lease)
                .Map(dest => dest.FileId, src => src.LeaseId)
                .Map(dest => dest.AreaUnitType, src => src.AreaUnitTypeCodeNavigation)
                .Map(dest => dest.LeaseArea, src => src.LeaseArea)
                .Map(dest => dest.PropertyName, src => src.Name)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Inherits<IBaseEntity, BaseConcurrentModel>();

            config.NewConfig<PropertyLeaseModel, PimsPropertyLease>()
                .PreserveReference(true)
                .Map(dest => dest.Property, src => src.Property)
                .Map(dest => dest.LeaseId, src => src.FileId)
                .Map(dest => dest.AreaUnitTypeCode, src => src.AreaUnitType.Id)
                .Map(dest => dest.LeaseArea, src => src.LeaseArea)
                .Map(dest => dest.Name, src => src.PropertyName)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Inherits<BaseConcurrentModel, IBaseEntity>();
        }
    }
}
