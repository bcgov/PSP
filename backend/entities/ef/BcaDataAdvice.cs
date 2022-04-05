using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("BCA_DATA_ADVICE")]
    public partial class BcaDataAdvice
    {
        public BcaDataAdvice()
        {
            BcaAssessmentAreas = new HashSet<BcaAssessmentArea>();
        }

        [Key]
        [Column("DATA_ADVICE_ID")]
        public long DataAdviceId { get; set; }
        [Column("ROLL_YEAR")]
        public int RollYear { get; set; }
        [Column("OWNERSHIP_YEAR")]
        public int OwnershipYear { get; set; }
        [Column("RUN_TYPE")]
        [StringLength(16)]
        public string RunType { get; set; }
        [Column("START_DATE", TypeName = "date")]
        public DateTime StartDate { get; set; }
        [Column("END_DATE", TypeName = "date")]
        public DateTime EndDate { get; set; }
        [Column("TOTAL_FOLIO_COUNT")]
        public int? TotalFolioCount { get; set; }
        [Column("TAXABLE_FOLIO_COUNT")]
        public int? TaxableFolioCount { get; set; }
        [Column("TAX_EXEMPT_FOLIO_COUNT")]
        public int? TaxExemptFolioCount { get; set; }
        [Column("TOTAL_GROSS_LAND_VALUE", TypeName = "money")]
        public decimal? TotalGrossLandValue { get; set; }
        [Column("TOTAL_GROSS_IMPROVEMENT_VALUE", TypeName = "money")]
        public decimal? TotalGrossImprovementValue { get; set; }
        [Column("TOTAL_TAX_EXEMPT_LAND_VALUE", TypeName = "money")]
        public decimal? TotalTaxExemptLandValue { get; set; }
        [Column("TOTAL_TAX_EXEMPT_IMPROVEMENT_VALUE", TypeName = "money")]
        public decimal? TotalTaxExemptImprovementValue { get; set; }
        [Column("TOTAL_NET_LAND_VALUE", TypeName = "money")]
        public decimal? TotalNetLandValue { get; set; }
        [Column("TOTAL_NET_IMPROVEMENT_VALUE", TypeName = "money")]
        public decimal? TotalNetImprovementValue { get; set; }
        [Required]
        [Column("VERSION")]
        [StringLength(50)]
        public string Version { get; set; }
        [Required]
        [Column("REQUEST_ID")]
        [StringLength(32)]
        public string RequestId { get; set; }
        [Required]
        [Column("ORDER_ID")]
        [StringLength(32)]
        public string OrderId { get; set; }
        [Column("RUN_DATE", TypeName = "date")]
        public DateTime RunDate { get; set; }
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

        [InverseProperty(nameof(BcaAssessmentArea.DataAdvice))]
        public virtual ICollection<BcaAssessmentArea> BcaAssessmentAreas { get; set; }
    }
}
