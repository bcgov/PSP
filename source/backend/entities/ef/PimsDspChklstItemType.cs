using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DSP_CHKLST_ITEM_TYPE")]
    [Index(nameof(DspChklstSectionTypeCode), Name = "DSPCIT_DSP_CHKLST_SECTION_TYPE_CODE_IDX")]
    public partial class PimsDspChklstItemType
    {
        public PimsDspChklstItemType()
        {
            PimsDispositionChecklistItems = new HashSet<PimsDispositionChecklistItem>();
        }

        [Key]
        [Column("DSP_CHKLST_ITEM_TYPE_CODE")]
        [StringLength(20)]
        public string DspChklstItemTypeCode { get; set; }
        [Required]
        [Column("DSP_CHKLST_SECTION_TYPE_CODE")]
        [StringLength(20)]
        public string DspChklstSectionTypeCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Column("HINT")]
        [StringLength(200)]
        public string Hint { get; set; }
        [Column("IS_REQUIRED")]
        public bool? IsRequired { get; set; }
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
        [Column("EFFECTIVE_DATE", TypeName = "date")]
        public DateTime EffectiveDate { get; set; }
        [Column("EXPIRY_DATE", TypeName = "date")]
        public DateTime? ExpiryDate { get; set; }
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

        [ForeignKey(nameof(DspChklstSectionTypeCode))]
        [InverseProperty(nameof(PimsDspChklstSectionType.PimsDspChklstItemTypes))]
        public virtual PimsDspChklstSectionType DspChklstSectionTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsDispositionChecklistItem.DspChklstItemTypeCodeNavigation))]
        public virtual ICollection<PimsDispositionChecklistItem> PimsDispositionChecklistItems { get; set; }
    }
}
