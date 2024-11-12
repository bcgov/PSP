using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table defining the take related to a specific acquisition file and property.
/// </summary>
[Table("PIMS_TAKE")]
[Index("AreaUnitTypeCode", Name = "TAKE_AREA_UNIT_TYPE_CODE_IDX")]
[Index("LandActTypeCode", Name = "TAKE_LAND_ACT_TYPE_CODE_IDX")]
[Index("PropertyAcquisitionFileId", Name = "TAKE_PROPERTY_ACQUISITION_FILE_ID_IDX")]
[Index("TakeSiteContamTypeCode", Name = "TAKE_TAKE_SITE_CONTAM_TYPE_CODE_IDX")]
[Index("TakeStatusTypeCode", Name = "TAKE_TAKE_STATUS_TYPE_CODE_IDX")]
[Index("TakeTypeCode", Name = "TAKE_TAKE_TYPE_CODE_IDX")]
public partial class PimsTake
{
    [Key]
    [Column("TAKE_ID")]
    public long TakeId { get; set; }

    [Column("PROPERTY_ACQUISITION_FILE_ID")]
    public long PropertyAcquisitionFileId { get; set; }

    [Required]
    [Column("TAKE_TYPE_CODE")]
    [StringLength(20)]
    public string TakeTypeCode { get; set; }

    [Required]
    [Column("TAKE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string TakeStatusTypeCode { get; set; }

    [Column("TAKE_SITE_CONTAM_TYPE_CODE")]
    [StringLength(20)]
    public string TakeSiteContamTypeCode { get; set; }

    [Column("AREA_UNIT_TYPE_CODE")]
    [StringLength(20)]
    public string AreaUnitTypeCode { get; set; }

    [Column("LAND_ACT_TYPE_CODE")]
    [StringLength(20)]
    public string LandActTypeCode { get; set; }

    /// <summary>
    /// Description of the property take.
    /// </summary>
    [Column("DESCRIPTION")]
    [StringLength(4000)]
    public string Description { get; set; }

    /// <summary>
    /// Date the take was completed.
    /// </summary>
    [Column("COMPLETION_DT")]
    public DateOnly? CompletionDt { get; set; }

    /// <summary>
    /// Is there a new right of way? (default = FALSE)
    /// </summary>
    [Column("IS_NEW_HIGHWAY_DEDICATION")]
    public bool IsNewHighwayDedication { get; set; }

    /// <summary>
    /// Area of the new right-of-way.
    /// </summary>
    [Column("NEW_HIGHWAY_DEDICATION_AREA")]
    public float? NewHighwayDedicationArea { get; set; }

    /// <summary>
    /// Is this being acquired for inventory? (default = TRUE)
    /// </summary>
    [Column("IS_ACQUIRED_FOR_INVENTORY")]
    public bool IsAcquiredForInventory { get; set; }

    /// <summary>
    /// Is there a statutory right of way? (default = FALSE)
    /// </summary>
    [Column("IS_NEW_INTEREST_IN_SRW")]
    public bool IsNewInterestInSrw { get; set; }

    /// <summary>
    /// Area of the statutory right-of-way.
    /// </summary>
    [Column("STATUTORY_RIGHT_OF_WAY_AREA")]
    public float? StatutoryRightOfWayArea { get; set; }

    /// <summary>
    /// End date of the statutory right-of-way.
    /// </summary>
    [Column("SRW_END_DT")]
    public DateOnly? SrwEndDt { get; set; }

    /// <summary>
    /// Is there a license to construct? (default = FALSE)
    /// </summary>
    [Column("IS_NEW_LICENSE_TO_CONSTRUCT")]
    public bool IsNewLicenseToConstruct { get; set; }

    /// <summary>
    /// Area of the license to construct.
    /// </summary>
    [Column("LICENSE_TO_CONSTRUCT_AREA")]
    public float? LicenseToConstructArea { get; set; }

    /// <summary>
    /// End date of the license to construct.
    /// </summary>
    [Column("LTC_END_DT")]
    public DateOnly? LtcEndDt { get; set; }

    /// <summary>
    /// Is there a Section 16? (default = FALSE)
    /// </summary>
    [Column("IS_NEW_LAND_ACT")]
    public bool IsNewLandAct { get; set; }

    /// <summary>
    /// Area of the Section 16 activity.
    /// </summary>
    [Column("LAND_ACT_AREA")]
    public float? LandActArea { get; set; }

    /// <summary>
    /// End date of the Section 16 activity.
    /// </summary>
    [Column("LAND_ACT_END_DT")]
    public DateOnly? LandActEndDt { get; set; }

    /// <summary>
    /// Is there a surplus or severance? (default = FALSE)
    /// </summary>
    [Column("IS_THERE_SURPLUS")]
    public bool IsThereSurplus { get; set; }

    /// <summary>
    /// Surplus/severance area.
    /// </summary>
    [Column("SURPLUS_AREA")]
    public float? SurplusArea { get; set; }

    /// <summary>
    /// Is there an active lease associated with the take?
    /// </summary>
    [Column("IS_ACTIVE_LEASE")]
    public bool IsActiveLease { get; set; }

    /// <summary>
    /// Area of the active lease.
    /// </summary>
    [Column("ACTIVE_LEASE_AREA")]
    public float? ActiveLeaseArea { get; set; }

    /// <summary>
    /// End date of the active lease.
    /// </summary>
    [Column("ACTIVE_LEASE_END_DT")]
    public DateOnly? ActiveLeaseEndDt { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AreaUnitTypeCode")]
    [InverseProperty("PimsTakes")]
    public virtual PimsAreaUnitType AreaUnitTypeCodeNavigation { get; set; }

    [ForeignKey("LandActTypeCode")]
    [InverseProperty("PimsTakes")]
    public virtual PimsLandActType LandActTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyAcquisitionFileId")]
    [InverseProperty("PimsTakes")]
    public virtual PimsPropertyAcquisitionFile PropertyAcquisitionFile { get; set; }

    [ForeignKey("TakeSiteContamTypeCode")]
    [InverseProperty("PimsTakes")]
    public virtual PimsTakeSiteContamType TakeSiteContamTypeCodeNavigation { get; set; }

    [ForeignKey("TakeStatusTypeCode")]
    [InverseProperty("PimsTakes")]
    public virtual PimsTakeStatusType TakeStatusTypeCodeNavigation { get; set; }

    [ForeignKey("TakeTypeCode")]
    [InverseProperty("PimsTakes")]
    public virtual PimsTakeType TakeTypeCodeNavigation { get; set; }
}
