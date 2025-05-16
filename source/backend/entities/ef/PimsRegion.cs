using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_REGION")]
public partial class PimsRegion
{
    [Key]
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    [Required]
    [Column("REGION_NAME")]
    [StringLength(200)]
    public string RegionName { get; set; }

    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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
