using Mapster;
using Entity = Pims.Dal.Entities;
using Model = Pims.Api.Areas.Property.Models.Property;

namespace Pims.Api.Areas.Property.Mapping.Property
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.Property, Model.PropertyModel>()
                .Map(dest => dest.PID, src => src.ParcelIdentity)
                .Map(dest => dest.PIN, src => src.PIN)
                .Map(dest => dest.PropertyType, src => src.PropertyTypeId)
                .Map(dest => dest.Status, src => src.StatusId)
                .Map(dest => dest.DataSource, src => src.DataSourceId)
                .Map(dest => dest.DataSourceEffectiveDate, src => src.DataSourceEffectiveDate)
                .Map(dest => dest.Classification, src => src.ClassificationId)
                .Map(dest => dest.Tenure, src => src.TenureId)
                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)

                .Map(dest => dest.AreaUnit, src => src.AreaUnitId)
                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription)
                .Map(dest => dest.Zoning, src => src.Zoning)
                .Map(dest => dest.ZoningPotential, src => src.ZoningPotential)

                .Map(dest => dest.Latitude, src => src.Location.Y)
                .Map(dest => dest.Longitude, src => src.Location.X)
                .Map(dest => dest.AddressId, src => src.AddressId)
                .Map(dest => dest.Address, src => src.Address == null ? null : src.Address.ToString().Trim())
                .Map(dest => dest.RegionId, src => src.RegionId)
                .Map(dest => dest.Region, src => src.Region.Name)
                .Map(dest => dest.DistrictId, src => src.DistrictId)
                .Map(dest => dest.District, src => src.District.Name)
                .Map(dest => dest.Province, src => src.Address == null ? null : src.Address.Province.Code)
                .Map(dest => dest.Municipality, src => src.Address == null ? null : src.Address.Municipality)
                .Map(dest => dest.Postal, src => src.Address == null ? null : src.Address.Postal);
        }
    }
}
