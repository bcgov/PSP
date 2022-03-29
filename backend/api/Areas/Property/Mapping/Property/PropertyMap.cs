using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Property;

namespace Pims.Api.Areas.Property.Mapping.Property
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, Model.PropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.PID, src => src.ParcelIdentity)
                .Map(dest => dest.PIN, src => src.Pin)
                .Map(dest => dest.Status, src => src.PropertyStatusTypeCodeNavigation)
                .Map(dest => dest.PropertyType, src => src.PropertyTypeCodeNavigation)

                // TODO: These navigation properties will be changed to many to many in next DB schema change
                .Map(dest => dest.Anomalies, src => src.PropertyAnomalyTypeCodeNavigation != null ? new[] { src.PropertyAnomalyTypeCodeNavigation } : null)
                .Map(dest => dest.Tenure, src => src.PropertyTenureTypeCodeNavigation != null ? new[] { src.PropertyTenureTypeCodeNavigation } : null)
                .Map(dest => dest.RoadType, src => src.PropertyRoadTypeCodeNavigation != null ? new[] { src.PropertyRoadTypeCodeNavigation } : null)
                .Map(dest => dest.AdjacentLand, src => src.PropertyAdjacentLandTypeCodeNavigation != null ? new[] { src.PropertyAdjacentLandTypeCodeNavigation } : null)

                .Map(dest => dest.DataSource, src => src.PropertyDataSourceTypeCodeNavigation)
                .Map(dest => dest.DataSourceEffectiveDate, src => src.PropertyDataSourceEffectiveDate)

                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)
                .Map(dest => dest.IsProvincialPublicHwy, src => src.IsProvincialPublicHwy)
                .Map(dest => dest.Notes, src => src.Notes)

                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.AreaUnit, src => src.PropertyAreaUnitTypeCodeNavigation)

                .Map(dest => dest.IsVolumetricParcel, src => src.IsVolumetricParcel)
                .Map(dest => dest.VolumetricMeasurement, src => src.VolumetricMeasurement)
                .Map(dest => dest.VolumetricUnit, src => src.VolumetricUnitTypeCodeNavigation)
                .Map(dest => dest.VolumetricType, src => src.VolumetricTypeCodeNavigation)

                .Map(dest => dest.MunicipalZoning, src => src.MunicipalZoning)

                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription)
                .Map(dest => dest.Zoning, src => src.Zoning)
                .Map(dest => dest.ZoningPotential, src => src.ZoningPotential)

                .Map(dest => dest.Latitude, src => src.Location.Coordinate.Y)
                .Map(dest => dest.Longitude, src => src.Location.Coordinate.X)

                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.Leases, src => src.GetLeases())
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();
        }
    }
}
