namespace Pims.Api.Areas.Property.Mapping.Search
{
    using System.Linq;
    using Mapster;
    using Entity = Pims.Dal.Entities;
    using Model = Pims.Api.Areas.Property.Models.Search;

    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, Model.PropertyModel>()
                .Map(dest => dest.Id, src => src.PropertyId)
                .Map(dest => dest.RowVersion, src => src.ConcurrencyControlNumber)
                .Map(dest => dest.PID, src => src.ParcelIdentity)
                .Map(dest => dest.PIN, src => src.Pin)
                .Map(dest => dest.PropertyTypeId, src => src.PropertyTypeCode)
                .Map(dest => dest.PropertyType, src => src.PropertyTypeCodeNavigation.Description)
                .Map(dest => dest.StatusId, src => src.PropertyStatusTypeCode)
                .Map(dest => dest.Status, src => src.PropertyStatusTypeCodeNavigation.Description)
                .Map(dest => dest.DataSourceId, src => src.PropertyDataSourceTypeCode)
                .Map(dest => dest.DataSource, src => src.PropertyDataSourceTypeCodeNavigation.Description)
                .Map(dest => dest.DataSourceEffectiveDate, src => src.PropertyDataSourceEffectiveDate)
                .Map(dest => dest.ClassificationId, src => src.PropertyClassificationTypeCode)
                .Map(dest => dest.Classification, src => src.PropertyClassificationTypeCodeNavigation.Description)
                .Map(dest => dest.Tenure, src => string.Join(", ", src.PimsPropPropTenureTypes.Select(tt => tt.PropertyTenureTypeCodeNavigation.Description)))
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)

                .Map(dest => dest.AreaUnitId, src => src.PropertyAreaUnitTypeCode)
                .Map(dest => dest.AreaUnit, src => src.PropertyAreaUnitTypeCodeNavigation.Description)
                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription)
                .Map(dest => dest.Zoning, src => src.Zoning)
                .Map(dest => dest.ZoningPotential, src => src.ZoningPotential)

                .Map(dest => dest.Latitude, src => src.Location.Coordinate.Y)
                .Map(dest => dest.Longitude, src => src.Location.Coordinate.X)

                .Map(dest => dest.AddressId, src => src.AddressId)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.RegionId, src => src.RegionCode)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation != null ? src.RegionCodeNavigation.RegionName : null)
                .Map(dest => dest.DistrictId, src => src.DistrictCode)
                .Map(dest => dest.District, src => src.DistrictCodeNavigation != null ? src.DistrictCodeNavigation.DistrictName : null);
        }
    }
}
