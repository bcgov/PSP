using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROP_MGMT_ACTIVITY_SUBTYPE")]
    [Index(nameof(PropMgmtActivityTypeCode), Name = "PRACST_PROP_MGMT_ACTIVITY_TYPE_CODE_IDX")]
    public partial class PimsPropMgmtActivitySubtype
    {
        public PimsPropMgmtActivitySubtype()
        {
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
        }

        [Key]
        [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivitySubtypeCode { get; set; }
        [Required]
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

        [ForeignKey(nameof(PropMgmtActivityTypeCode))]
        [InverseProperty(nameof(PimsPropMgmtActivityType.PimsPropMgmtActivitySubtypes))]
        public virtual PimsPropMgmtActivityType PropMgmtActivityTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsPropertyActivity.PropMgmtActivitySubtypeCodeNavigation))]
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
    }
}
