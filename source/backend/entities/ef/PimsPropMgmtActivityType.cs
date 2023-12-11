using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROP_MGMT_ACTIVITY_TYPE")]
    public partial class PimsPropMgmtActivityType
    {
        public PimsPropMgmtActivityType()
        {
            PimsPropMgmtActivitySubtypes = new HashSet<PimsPropMgmtActivitySubtype>();
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
        }

        [Key]
        [Column("PROP_MGMT_ACTIVITY_TYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivityTypeCode { get; set; }
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

        [InverseProperty(nameof(PimsPropMgmtActivitySubtype.PropMgmtActivityTypeCodeNavigation))]
        public virtual ICollection<PimsPropMgmtActivitySubtype> PimsPropMgmtActivitySubtypes { get; set; }
        [InverseProperty(nameof(PimsPropertyActivity.PropMgmtActivityTypeCodeNavigation))]
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
    }
}
