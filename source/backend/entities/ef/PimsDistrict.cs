using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISTRICT")]
    [Index(nameof(RegionCode), Name = "DSTRCT_REGION_CODE_IDX")]
    public partial class PimsDistrict
    {
        public PimsDistrict()
        {
            PimsAddresses = new HashSet<PimsAddress>();
            PimsOrganizations = new HashSet<PimsOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
        }

        [Key]
        [Column("DISTRICT_CODE")]
        public short DistrictCode { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Required]
        [Column("DISTRICT_NAME")]
        [StringLength(200)]
        public string DistrictName { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsDistricts))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsAddress.DistrictCodeNavigation))]
        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
        [InverseProperty(nameof(PimsOrganization.DistrictCodeNavigation))]
        public virtual ICollection<PimsOrganization> PimsOrganizations { get; set; }
        [InverseProperty(nameof(PimsProperty.DistrictCodeNavigation))]
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
    }
}
