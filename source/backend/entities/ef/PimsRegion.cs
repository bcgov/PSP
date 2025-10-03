using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_REGION")]
public partial class PimsRegion
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Name assigned to the region.
    /// </summary>
    [Required]
    [Column("REGION_NAME")]
    [StringLength(200)]
    public string RegionName { get; set; }

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

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; } = new List<PimsAccessRequest>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; } = new List<PimsAcquisitionFile>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsAddress> PimsAddresses { get; set; } = new List<PimsAddress>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsDispositionFile> PimsDispositionFiles { get; set; } = new List<PimsDispositionFile>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsDistrict> PimsDistricts { get; set; } = new List<PimsDistrict>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsLease> PimsLeases { get; set; } = new List<PimsLease>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsOrganization> PimsOrganizations { get; set; } = new List<PimsOrganization>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsProject> PimsProjects { get; set; } = new List<PimsProject>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();

    [InverseProperty("RegionCodeNavigation")]
    public virtual ICollection<PimsRegionUser> PimsRegionUsers { get; set; } = new List<PimsRegionUser>();
}
