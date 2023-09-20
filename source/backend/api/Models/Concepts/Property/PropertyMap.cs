using Mapster;
using Entity = Pims.Dal.Entities;

namespace Pims.Api.Models.Concepts
{
    public class PropertyMap : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Entity.PimsProperty, PropertyModel>()
                .Map(dest => dest.Id, src => src.Internal_Id)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.Pin, src => src.Pin)
                .Map(dest => dest.PlanNumber, src => src.SurveyPlanNumber)
                .Map(dest => dest.Status, src => src.PropertyStatusTypeCodeNavigation)
                .Map(dest => dest.PropertyType, src => src.PropertyTypeCodeNavigation)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.District, src => src.DistrictCodeNavigation)
                .Map(dest => dest.Region, src => src.RegionCodeNavigation)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Boundary, src => src.Boundary)
                .Map(dest => dest.GeneralLocation, src => src.GeneralLocation)

                .Map(dest => dest.DataSource, src => src.PropertyDataSourceTypeCodeNavigation)
                .Map(dest => dest.DataSourceEffectiveDate, src => src.PropertyDataSourceEffectiveDate)

                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)
                .Map(dest => dest.IsProvincialPublicHwy, src => src.IsProvincialPublicHwy)
                .Map(dest => dest.IsRwyBeltDomPatent, src => src.IsRwyBeltDomPatent)
                .Map(dest => dest.PphStatusTypeCode, src => src.PphStatusTypeCode)
                .Map(dest => dest.PphStatusUpdateUserid, src => src.PphStatusUpdateUserid)
                .Map(dest => dest.PphStatusUpdateTimestamp, src => src.PphStatusUpdateTimestamp)
                .Map(dest => dest.PphStatusUpdateUserGuid, src => src.PphStatusUpdateUserGuid)
                .Map(dest => dest.Notes, src => src.Notes)
                .Map(dest => dest.IsOwned, src => src.IsOwned)
                .Map(dest => dest.IsPropertyOfInterest, src => src.IsPropertyOfInterest)
                .Map(dest => dest.IsVisibleToOtherAgencies, src => src.IsVisibleToOtherAgencies)

                // multi-selects
                .Map(dest => dest.Anomalies, src => src.PimsPropPropAnomalyTypes)
                .Map(dest => dest.Tenures, src => src.PimsPropPropTenureTypes)
                .Map(dest => dest.RoadTypes, src => src.PimsPropPropRoadTypes)

                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.AreaUnit, src => src.PropertyAreaUnitTypeCodeNavigation)

                .Map(dest => dest.IsVolumetricParcel, src => src.IsVolumetricParcel)
                .Map(dest => dest.VolumetricMeasurement, src => src.VolumetricMeasurement)
                .Map(dest => dest.VolumetricUnit, src => src.VolumeUnitTypeCodeNavigation)
                .Map(dest => dest.VolumetricType, src => src.VolumetricTypeCodeNavigation)

                .Map(dest => dest.MunicipalZoning, src => src.MunicipalZoning)

                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription)
                .Map(dest => dest.Zoning, src => src.Zoning)
                .Map(dest => dest.ZoningPotential, src => src.ZoningPotential)

                .Map(dest => dest.Latitude, src => src.Location.Coordinate.Y)
                .Map(dest => dest.Longitude, src => src.Location.Coordinate.X)

                .Map(dest => dest.SurplusDeclarationComment, src => src.SurplusDeclarationComment)
                .Map(dest => dest.SurplusDeclarationDate, src => src.SurplusDeclarationDate)
                .Map(dest => dest.SurplusDeclarationType, src => src.SurplusDeclarationTypeCodeNavigation)

                .Map(dest => dest.ManagementPurposes, src => src.PimsPropPropPurposes)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.IsUtilitiesPayable, src => src.IsUtilitiesPayable)
                .Map(dest => dest.IsTaxesPayable, src => src.IsTaxesPayable)
                .Inherits<Entity.IBaseEntity, Api.Models.BaseModel>();

            config.NewConfig<PropertyModel, Entity.PimsProperty>()
                .Map(dest => dest.Internal_Id, src => src.Id)
                .Map(dest => dest.Pid, src => src.Pid)
                .Map(dest => dest.Pin, src => src.Pin)
                .Map(dest => dest.SurveyPlanNumber, src => src.PlanNumber)
                .Map(dest => dest.PropertyStatusTypeCode, src => src.Status.Id)
                .Map(dest => dest.PropertyTypeCode, src => src.PropertyType.Id)
                .Map(dest => dest.Address, src => src.Address)
                .Map(dest => dest.DistrictCode, src => src.District.Id)
                .Map(dest => dest.RegionCode, src => src.Region.Id)
                .Map(dest => dest.Location, src => src.Location)
                .Map(dest => dest.Boundary, src => src.Boundary)
                .Map(dest => dest.GeneralLocation, src => src.GeneralLocation)

                .Map(dest => dest.Name, src => src.Name)
                .Map(dest => dest.Description, src => src.Description)
                .Map(dest => dest.IsSensitive, src => src.IsSensitive)
                .Map(dest => dest.IsProvincialPublicHwy, src => src.IsProvincialPublicHwy)
                .Map(dest => dest.IsRwyBeltDomPatent, src => src.IsRwyBeltDomPatent)
                .Map(dest => dest.PphStatusTypeCode, src => src.PphStatusTypeCode)

                .Map(dest => dest.Notes, src => src.Notes)
                .Map(dest => dest.IsOwned, src => src.IsOwned)
                .Map(dest => dest.IsVisibleToOtherAgencies, src => src.IsVisibleToOtherAgencies)

                // multi-selects
                .Map(dest => dest.PimsPropPropAnomalyTypes, src => src.Anomalies)
                .Map(dest => dest.PimsPropPropTenureTypes, src => src.Tenures)
                .Map(dest => dest.PimsPropPropRoadTypes, src => src.RoadTypes)

                .Map(dest => dest.LandArea, src => src.LandArea)
                .Map(dest => dest.PropertyAreaUnitTypeCode, src => src.AreaUnit.Id)

                .Map(dest => dest.IsVolumetricParcel, src => src.IsVolumetricParcel)
                .Map(dest => dest.VolumetricMeasurement, src => src.VolumetricMeasurement)
                .Map(dest => dest.VolumeUnitTypeCode, src => src.VolumetricUnit.Id)
                .Map(dest => dest.VolumetricTypeCode, src => src.VolumetricType.Id)

                .Map(dest => dest.MunicipalZoning, src => src.MunicipalZoning)

                .Map(dest => dest.LandLegalDescription, src => src.LandLegalDescription)
                .Map(dest => dest.Zoning, src => src.Zoning)
                .Map(dest => dest.ZoningPotential, src => src.ZoningPotential)

                .Map(dest => dest.PimsPropPropPurposes, src => src.ManagementPurposes)
                .Map(dest => dest.AdditionalDetails, src => src.AdditionalDetails)
                .Map(dest => dest.IsUtilitiesPayable, src => src.IsUtilitiesPayable)
                .Map(dest => dest.IsTaxesPayable, src => src.IsTaxesPayable)

                .Inherits<BaseModel, Entity.IBaseEntity>();
        }
    }
}
