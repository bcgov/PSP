using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DSP_CHKLST_SECTION_TYPE")]
    public partial class PimsDspChklstSectionType
    {
        public PimsDspChklstSectionType()
        {
            PimsDspChklstItemTypes = new HashSet<PimsDspChklstItemType>();
        }

        [Key]
        [Column("DSP_CHKLST_SECTION_TYPE_CODE")]
        [StringLength(20)]
        public string DspChklstSectionTypeCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
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

        [InverseProperty(nameof(PimsDspChklstItemType.DspChklstSectionTypeCodeNavigation))]
        public virtual ICollection<PimsDspChklstItemType> PimsDspChklstItemTypes { get; set; }
    }
}
