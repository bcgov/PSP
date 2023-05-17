using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_H120_CATEGORY")]
    [Index(nameof(CostTypeId), Name = "H120CT_COST_TYPE_ID_IDX")]
    [Index(nameof(FinancialActivityId), Name = "H120CT_FINANCIAL_ACTIVITY_ID_IDX")]
    [Index(nameof(WorkActivityId), Name = "H120CT_WORK_ACTIVITY_ID_IDX")]
    public partial class PimsH120Category
    {
        [Key]
        [Column("H120_CATEGORY_ID")]
        public long H120CategoryId { get; set; }
        [Column("FINANCIAL_ACTIVITY_ID")]
        public long FinancialActivityId { get; set; }
        [Column("WORK_ACTIVITY_ID")]
        public long? WorkActivityId { get; set; }
        [Column("COST_TYPE_ID")]
        public long? CostTypeId { get; set; }
        [Column("H120_CATEGORY_NO")]
        public int? H120CategoryNo { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Column("EXPIRY_DATE", TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
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

        [ForeignKey(nameof(CostTypeId))]
        [InverseProperty(nameof(PimsCostTypeCode.PimsH120Categories))]
        public virtual PimsCostTypeCode CostType { get; set; }
        [ForeignKey(nameof(FinancialActivityId))]
        [InverseProperty(nameof(PimsFinancialActivityCode.PimsH120Categories))]
        public virtual PimsFinancialActivityCode FinancialActivity { get; set; }
        [ForeignKey(nameof(WorkActivityId))]
        [InverseProperty(nameof(PimsWorkActivityCode.PimsH120Categories))]
        public virtual PimsWorkActivityCode WorkActivity { get; set; }
    }
}
