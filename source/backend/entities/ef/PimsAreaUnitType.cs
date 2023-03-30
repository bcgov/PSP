using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_AREA_UNIT_TYPE")]
    public partial class PimsAreaUnitType
    {
        public PimsAreaUnitType()
        {
            PimsProperties = new HashSet<PimsProperty>();
            PimsPropertyLeases = new HashSet<PimsPropertyLease>();
            PimsTakes = new HashSet<PimsTake>();
        }

        [Key]
        [Column("AREA_UNIT_TYPE_CODE")]
        [StringLength(20)]
        public string AreaUnitTypeCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
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

        [InverseProperty(nameof(PimsProperty.PropertyAreaUnitTypeCodeNavigation))]
        public virtual ICollection<PimsProperty> PimsProperties { get; set; }
        [InverseProperty(nameof(PimsPropertyLease.AreaUnitTypeCodeNavigation))]
        public virtual ICollection<PimsPropertyLease> PimsPropertyLeases { get; set; }
        [InverseProperty(nameof(PimsTake.AreaUnitTypeCodeNavigation))]
        public virtual ICollection<PimsTake> PimsTakes { get; set; }
    }
}
