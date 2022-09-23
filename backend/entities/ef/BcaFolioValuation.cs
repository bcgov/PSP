using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Keyless]
    [Table("BCA_FOLIO_VALUATION")]
    [Index(nameof(RollNumber), Name = "BCAVAL_ROLL_NUMBER_IDX")]
    public partial class BcaFolioValuation
    {
        [Column("ROLL_NUMBER")]
        [StringLength(32)]
        public string RollNumber { get; set; }
        [Column("TAX_EXEMPT_CODE")]
        [StringLength(16)]
        public string TaxExemptCode { get; set; }
        [Column("TAX_EXEMPT_DESCRIPTION")]
        [StringLength(255)]
        public string TaxExemptDescription { get; set; }
        [Column("PROPERTY_CLASS_CODE")]
        [StringLength(16)]
        public string PropertyClassCode { get; set; }
        [Column("PROPERTY_CLASS_DESCRIPTION")]
        [StringLength(255)]
        public string PropertyClassDescription { get; set; }
        [Column("LAND_VALUE", TypeName = "money")]
        public decimal? LandValue { get; set; }
        [Column("IMPROVEMENT_VALUE", TypeName = "money")]
        public decimal? ImprovementValue { get; set; }
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

        [ForeignKey(nameof(RollNumber))]
        public virtual BcaFolioRecord RollNumberNavigation { get; set; }
    }
}
