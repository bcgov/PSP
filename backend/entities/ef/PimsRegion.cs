using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_REGION")]
    public partial class PimsRegion
    {
        public PimsRegion()
        {
            PimsAddresses = new HashSet<PimsAddress>();
            PimsDistricts = new HashSet<PimsDistrict>();
            PimsOrganizations = new HashSet<PimsOrganization>();
            PimsProperties = new HashSet<PimsProperty>();
        }

        [Key]
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Required]
        [Column("REGION_NAME")]
        [StringLength(200)]
        public string RegionName { get; set; }
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

        [InverseProperty(nameof(PimsAddress.RegionCodeNavigation))]
        public virtual ICollection<PimsAddress> PimsAddresses { get; set; }
        [InverseProperty(nameof(PimsDistrict.RegionCodeNavigation))]
        public virtual ICollection<PimsDistrict> PimsDistricts { get; set; }
        [InverseProperty(nameof(PimsOrganization.RegionCodeNavigation))]
        public virtual ICollection<PimsOrganization> PimsOrganizations { get; set; }
        [InverseProperty(nameof(PimsProperty.RegionCodeNavigation))]
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
    }
}
