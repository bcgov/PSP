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

                .Map(dest => dest.PropertyType, src => src.PropertyTypeCodeNavigation)
                .Map(dest => dest.Anomalies, src => src.PropertyAnomalyTypeCodeNavigation)
                .Map(dest => dest.Tenure, src => src.PropertyTenureTypeCodeNavigation)
                .Map(dest => dest.RoadType, src => src.PropertyRoadTypeCodeNavigation)
                .Map(dest => dest.AdjacentLand, src => src.PropertyAdjacentLandTypeCodeNavigation)
                .Map(dest => dest.Status, src => src.PropertyStatusTypeCodeNavigation)

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

                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription)
                .Map(dest => dest.Zoning, src => src.Zoning)
                .Map(dest => dest.ZoningPotential, src => src.ZoningPotential)

                .Map(dest => dest.Latitude, src => src.Location.Coordinate.Y)
                .Map(dest => dest.Longitude, src => src.Location.Coordinate.X)

                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.RegionId, src => src.RegionCode)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation != null ? src.RegionCodeNavigation.RegionName : null)
                .Map(dest => dest.DistrictId, src => src.DistrictCode)
                .Map(dest => dest.District, src => src.DistrictCodeNavigation.DistrictName)
                .Map(dest => dest.Leases, src => src.GetLeases())
                .Inherits<Entity.IBaseAppEntity, Api.Models.BaseAppModel>();
        }
    }
}
