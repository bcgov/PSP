using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the attributes of a property.
/// </summary>
[Table("PIMS_PROPERTY")]
[Index("AddressId", Name = "PRPRTY_ADDRESS_ID_IDX")]
[Index("Boundary", Name = "PRPRTY_BOUNDARY_IDX")]
[Index("DistrictCode", Name = "PRPRTY_DISTRICT_CODE_IDX")]
[Index("Location", Name = "PRPRTY_LOCATION_IDX")]
[Index("Pid", Name = "PRPRTY_PID_IDX")]
[Index("PphStatusTypeCode", Name = "PRPRTY_PPH_STATUS_TYPE_CODE_IDX")]
[Index("PropertyAreaUnitTypeCode", Name = "PRPRTY_PROPERTY_AREA_UNIT_TYPE_CODE_IDX")]
[Index("PropertyDataSourceTypeCode", Name = "PRPRTY_PROPERTY_DATA_SOURCE_TYPE_CODE_IDX")]
[Index("PropertyStatusTypeCode", Name = "PRPRTY_PROPERTY_STATUS_TYPE_CODE_IDX")]
[Index("PropertyTypeCode", Name = "PRPRTY_PROPERTY_TYPE_CODE_IDX")]
[Index("RegionCode", Name = "PRPRTY_REGION_CODE_IDX")]
[Index("SurplusDeclarationTypeCode", Name = "PRPRTY_SURPLUS_DECLARATION_TYPE_CODE_IDX")]
[Index("SurveyPlanNumber", Name = "PRPRTY_SURVEY_PLAN_NUMBER_IDX")]
[Index("VolumetricTypeCode", Name = "PRPRTY_VOLUMETRIC_TYPE_CODE_IDX")]
[Index("VolumeUnitTypeCode", Name = "PRPRTY_VOLUME_UNIT_TYPE_CODE_IDX")]
public partial class PimsProperty
{
    /// <summary>
    /// Generated surrogate primary key
    /// </summary>
    [Key]
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Foreign key to the proprty type table.
    /// </summary>
    [Required]
    [Column("PROPERTY_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the address table.
    /// </summary>
    [Column("ADDRESS_ID")]
    public long? AddressId { get; set; }

    /// <summary>
    /// Foreign key to the region table.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Foreign key to the district table.
    /// </summary>
    [Column("DISTRICT_CODE")]
    public short DistrictCode { get; set; }

    /// <summary>
    /// Foreign key to the property area unit type table.
    /// </summary>
    [Column("PROPERTY_AREA_UNIT_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyAreaUnitTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the property data source type table.
    /// </summary>
    [Required]
    [Column("PROPERTY_DATA_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyDataSourceTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the property status type table.
    /// </summary>
    [Required]
    [Column("PROPERTY_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the surplus declaration type table.
    /// </summary>
    [Required]
    [Column("SURPLUS_DECLARATION_TYPE_CODE")]
    [StringLength(20)]
    public string SurplusDeclarationTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the volumetric type table.
    /// </summary>
    [Column("VOLUMETRIC_TYPE_CODE")]
    [StringLength(20)]
    public string VolumetricTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the volume unit type table.
    /// </summary>
    [Column("VOLUME_UNIT_TYPE_CODE")]
    [StringLength(20)]
    public string VolumeUnitTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the provincial public highway status type table.
    /// </summary>
    [Column("PPH_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string PphStatusTypeCode { get; set; }

    /// <summary>
    /// Date the property was officially registered
    /// </summary>
    [Column("PROPERTY_DATA_SOURCE_EFFECTIVE_DATE")]
    public DateOnly PropertyDataSourceEffectiveDate { get; set; }

    /// <summary>
    /// Property ID
    /// </summary>
    [Column("PID")]
    public int? Pid { get; set; }

    /// <summary>
    /// Property number
    /// </summary>
    [Column("PIN")]
    public int? Pin { get; set; }

    /// <summary>
    /// The (ARCS/ORCS) number identifying the Property File.
    /// </summary>
    [Column("FILE_NUMBER")]
    public int? FileNumber { get; set; }

    /// <summary>
    /// A suffix to distinguish between Property Files with the same number.
    /// </summary>
    [Column("FILE_NUMBER_SUFFIX")]
    [StringLength(2)]
    public string FileNumberSuffix { get; set; }

    /// <summary>
    /// Area occupied by property
    /// </summary>
    [Column("LAND_AREA")]
    public float? LandArea { get; set; }

    /// <summary>
    /// Legal description of property
    /// </summary>
    [Column("LAND_LEGAL_DESCRIPTION")]
    [StringLength(2000)]
    public string LandLegalDescription { get; set; }

    /// <summary>
    /// Spatial bundary of land
    /// </summary>
    [Column("BOUNDARY", TypeName = "geometry")]
    public Geometry Boundary { get; set; }

    /// <summary>
    /// Geospatial location (pin) of property
    /// </summary>
    [Column("LOCATION", TypeName = "geometry")]
    public Geometry Location { get; set; }

    /// <summary>
    /// Descriptive location of the property, primarily for H120 activities.
    /// </summary>
    [Column("GENERAL_LOCATION")]
    [StringLength(2000)]
    public string GeneralLocation { get; set; }

    /// <summary>
    /// Property/Land Parcel survey plan number
    /// </summary>
    [Column("SURVEY_PLAN_NUMBER")]
    [StringLength(250)]
    public string SurveyPlanNumber { get; set; }

    /// <summary>
    /// Comment regarding the surplus declaration
    /// </summary>
    [Column("SURPLUS_DECLARATION_COMMENT")]
    [StringLength(2000)]
    public string SurplusDeclarationComment { get; set; }

    /// <summary>
    /// Date the property was declared surplus
    /// </summary>
    [Column("SURPLUS_DECLARATION_DATE", TypeName = "datetime")]
    public DateTime? SurplusDeclarationDate { get; set; }

    /// <summary>
    /// Notes about the property
    /// </summary>
    [Column("NOTES")]
    [StringLength(4000)]
    public string Notes { get; set; }

    /// <summary>
    /// Municipal zoning that applies this property.
    /// </summary>
    [Column("MUNICIPAL_ZONING")]
    [StringLength(100)]
    public string MunicipalZoning { get; set; }

    /// <summary>
    /// Is there a volumetric measurement for this parcel?
    /// </summary>
    [Column("IS_VOLUMETRIC_PARCEL")]
    public bool? IsVolumetricParcel { get; set; }

    /// <summary>
    /// Volumetric measurement of the parcel.
    /// </summary>
    [Column("VOLUMETRIC_MEASUREMENT")]
    public float? VolumetricMeasurement { get; set; }

    /// <summary>
    /// Is the property currently owned?
    /// </summary>
    [Column("IS_OWNED")]
    public bool IsOwned { get; set; }

    /// <summary>
    /// If the property was the source of a subdivision operation or the target of a consolidation operation, the property is marked as retired.
    /// </summary>
    [Column("IS_RETIRED")]
    public bool? IsRetired { get; set; }

    /// <summary>
    /// Userid that updated the Provincial Public Highway status.
    /// </summary>
    [Column("PPH_STATUS_UPDATE_USERID")]
    [StringLength(30)]
    public string PphStatusUpdateUserid { get; set; }

    /// <summary>
    /// Date / time that the Provincial Public Highway status was updated.
    /// </summary>
    [Column("PPH_STATUS_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime? PphStatusUpdateTimestamp { get; set; }

    /// <summary>
    /// GUID of the user that updated the PPH status.
    /// </summary>
    [Column("PPH_STATUS_UPDATE_USER_GUID")]
    public Guid? PphStatusUpdateUserGuid { get; set; }

    /// <summary>
    /// Indicates if this property is original federal vs. provincial ownership.
    /// </summary>
    [Column("IS_RWY_BELT_DOM_PATENT")]
    public bool? IsRwyBeltDomPatent { get; set; }

    /// <summary>
    /// Additional details about the property.
    /// </summary>
    [Column("ADDITIONAL_DETAILS")]
    [StringLength(4000)]
    public string AdditionalDetails { get; set; }

    /// <summary>
    /// Indicates if the utilities are being paid.
    /// </summary>
    [Column("IS_UTILITIES_PAYABLE")]
    public bool? IsUtilitiesPayable { get; set; }

    /// <summary>
    /// Indicates if the property taxes are being paid.
    /// </summary>
    [Column("IS_TAXES_PAYABLE")]
    public bool? IsTaxesPayable { get; set; }

    /// <summary>
    /// Name of the Indian band.
    /// </summary>
    [Column("BAND_NAME")]
    [StringLength(80)]
    public string BandName { get; set; }

    /// <summary>
    /// Name of the Indian reserve.
    /// </summary>
    [Column("RESERVE_NAME")]
    [StringLength(100)]
    public string ReserveName { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("PimsProperties")]
    public virtual PimsAddress Address { get; set; }

    [ForeignKey("DistrictCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsDistrict DistrictCodeNavigation { get; set; }

    [InverseProperty("Property")]
    public virtual ICollection<PimsDispositionFileProperty> PimsDispositionFileProperties { get; set; } = new List<PimsDispositionFileProperty>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsHistoricalFileNumber> PimsHistoricalFileNumbers { get; set; } = new List<PimsHistoricalFileNumber>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropPropActivity> PimsPropPropActivities { get; set; } = new List<PimsPropPropActivity>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropPropAnomalyType> PimsPropPropAnomalyTypes { get; set; } = new List<PimsPropPropAnomalyType>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropPropPurpose> PimsPropPropPurposes { get; set; } = new List<PimsPropPropPurpose>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropPropRoadType> PimsPropPropRoadTypes { get; set; } = new List<PimsPropPropRoadType>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropPropTenureType> PimsPropPropTenureTypes { get; set; } = new List<PimsPropPropTenureType>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropertyAcquisitionFile> PimsPropertyAcquisitionFiles { get; set; } = new List<PimsPropertyAcquisitionFile>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropertyContact> PimsPropertyContacts { get; set; } = new List<PimsPropertyContact>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; } = new List<PimsPropertyLease>();

    [InverseProperty("DestinationProperty")]
    public virtual ICollection<PimsPropertyOperation> PimsPropertyOperationDestinationProperties { get; set; } = new List<PimsPropertyOperation>();

    [InverseProperty("SourceProperty")]
    public virtual ICollection<PimsPropertyOperation> PimsPropertyOperationSourceProperties { get; set; } = new List<PimsPropertyOperation>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropertyOrganization> PimsPropertyOrganizations { get; set; } = new List<PimsPropertyOrganization>();

    [InverseProperty("Property")]
    public virtual ICollection<PimsPropertyResearchFile> PimsPropertyResearchFiles { get; set; } = new List<PimsPropertyResearchFile>();

    [ForeignKey("PphStatusTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsPphStatusType PphStatusTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyAreaUnitTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsAreaUnitType PropertyAreaUnitTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyDataSourceTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsDataSourceType PropertyDataSourceTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyStatusTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsPropertyStatusType PropertyStatusTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsPropertyType PropertyTypeCodeNavigation { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }

    [ForeignKey("SurplusDeclarationTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsSurplusDeclarationType SurplusDeclarationTypeCodeNavigation { get; set; }

    [ForeignKey("VolumeUnitTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsVolumeUnitType VolumeUnitTypeCodeNavigation { get; set; }

    [ForeignKey("VolumetricTypeCode")]
    [InverseProperty("PimsProperties")]
    public virtual PimsVolumetricType VolumetricTypeCodeNavigation { get; set; }
}
