using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DSP_CHKLST_ITEM_STATUS_TYPE")]
    public partial class PimsDspChklstItemStatusType
    {
        public PimsDspChklstItemStatusType()
        {
            PimsDispositionChecklistItems = new HashSet<PimsDispositionChecklistItem>();
        }

        [Key]
        [Column("DSP_CHKLST_ITEM_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DspChklstItemStatusTypeCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [InverseProperty(nameof(PimsDispositionChecklistItem.DspChklstItemStatusTypeCodeNavigation))]
        public virtual ICollection<PimsDispositionChecklistItem> PimsDispositionChecklistItems { get; set; }
    }
}
