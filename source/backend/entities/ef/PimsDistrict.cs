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
    [Key]
    [Column("DISTRICT_CODE")]
    public short DistrictCode { get; set; }

    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    [Required]
    [Column("DISTRICT_NAME")]
    [StringLength(200)]
    public string DistrictName { get; set; }

    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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
