using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PRODUCT_HIST")]
    [Index(nameof(ProductHistId), nameof(EndDateHist), Name = "PIMS_PRODCT_H_UK", IsUnique = true)]
    public partial class PimsProductHist
    {
        [Key]
        [Column("_PRODUCT_HIST_ID")]
        public long ProductHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("ID")]
        public long Id { get; set; }
        [Column("PARENT_PROJECT_ID")]
        public long ParentProjectId { get; set; }
        [Required]
        [Column("CODE")]
        [StringLength(20)]
        public string Code { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Column("START_DATE", TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column("COST_ESTIMATE", TypeName = "money")]
        public decimal? CostEstimate { get; set; }
        [Column("COST_ESTIMATE_DATE", TypeName = "datetime")]
        public DateTime? CostEstimateDate { get; set; }
        [Column("OBJECTIVE")]
        [StringLength(2000)]
        public string Objective { get; set; }
        [Column("SCOPE")]
        [StringLength(2000)]
        public string Scope { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
        [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppCreateTimestamp { get; set; }
        [Required]
        [Column("APP_CREATE_USERID")]
        [StringLength(30)]
        public string AppCreateUserid { get; set; }
        [Column("APP_CREATE_USER_GUID")]
        public Guid? AppCreateUserGuid { get; set; }
        [Required]
        [Column("APP_CREATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppCreateUserDirectory { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppLastUpdateUserDirectory { get; set; }
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
        [Column("CODE_DESC_UPPER")]
        [StringLength(220)]
        public string CodeDescUpper { get; set; }
    }
}
