using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_TASK_TEMPLATE_ACTIVITY_MODEL")]
    [Index(nameof(ActivityModelId), Name = "TSKTAM_ACTIVITY_MODEL_ID_IDX")]
    [Index(nameof(TaskTemplateId), nameof(ActivityModelId), Name = "TSKTAM_TASK_TEMPLATE_ACTIVITY_MODEL_TUC", IsUnique = true)]
    [Index(nameof(TaskTemplateId), Name = "TSKTAM_TASK_TEMPLATE_ID_IDX")]
    public partial class PimsTaskTemplateActivityModel
    {
        [Key]
        [Column("TASK_TEMPLATE_ACTIVITY_MODEL_ID")]
        public long TaskTemplateActivityModelId { get; set; }
        [Column("TASK_TEMPLATE_ID")]
        public long TaskTemplateId { get; set; }
        [Column("ACTIVITY_MODEL_ID")]
        public long ActivityModelId { get; set; }
        [Required]
        [Column("IS_MANDATORY")]
        public bool? IsMandatory { get; set; }
        [Column("IMPLEMENTATION_ORDER")]
        public short ImplementationOrder { get; set; }
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

        [ForeignKey(nameof(ActivityModelId))]
        [InverseProperty(nameof(PimsActivityModel.PimsTaskTemplateActivityModels))]
        public virtual PimsActivityModel ActivityModel { get; set; }
        [ForeignKey(nameof(TaskTemplateId))]
        [InverseProperty(nameof(PimsTaskTemplate.PimsTaskTemplateActivityModels))]
        public virtual PimsTaskTemplate TaskTemplate { get; set; }
    }
}
