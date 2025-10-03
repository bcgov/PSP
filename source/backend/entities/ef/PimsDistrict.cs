using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DISTRICT")]
[Index("RegionCode", Name = "DSTRCT_REGION_CODE_IDX")]
public partial class PimsDistrict
{
    /// <summary>
    /// Code value assigned to the district.
    /// </summary>
    [Key]
    [Column("DISTRICT_CODE")]
    public short DistrictCode { get; set; }

    /// <summary>
    /// Code value assigned to the region.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Name of the assigned district.
    /// </summary>
    [Required]
    [Column("DISTRICT_NAME")]
    [StringLength(200)]
    public string DistrictName { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("DistrictCodeNavigation")]
    public virtual ICollection<PimsAddress> PimsAddresses { get; set; } = new List<PimsAddress>();

    [InverseProperty("DistrictCodeNavigation")]
    public virtual ICollection<PimsOrganization> PimsOrganizations { get; set; } = new List<PimsOrganization>();

    [InverseProperty("DistrictCodeNavigation")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsDistricts")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }
}
