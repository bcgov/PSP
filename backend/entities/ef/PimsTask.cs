using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_TASK")]
    [Index(nameof(ActivityId), Name = "TASK_ACTIVITY_ID_IDX")]
    [Index(nameof(TaskTemplateId), Name = "TASK_TASK_TEMPLATE_ID_IDX")]
    [Index(nameof(UserId), nameof(ActivityId), nameof(TaskTemplateId), Name = "TASK_TEMPLATE_ACTIVITY_USER_TUC", IsUnique = true)]
    [Index(nameof(UserId), Name = "TASK_USER_ID_IDX")]
    public partial class PimsTask
    {
        [Key]
        [Column("TASK_ID")]
        public long TaskId { get; set; }
        [Column("TASK_TEMPLATE_ID")]
        public long TaskTemplateId { get; set; }
        [Column("ACTIVITY_ID")]
        public long? ActivityId { get; set; }
        [Column("USER_ID")]
        public long UserId { get; set; }
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

        [ForeignKey(nameof(ActivityId))]
        [InverseProperty(nameof(PimsActivity.PimsTasks))]
        public virtual PimsActivity Activity { get; set; }
        [ForeignKey(nameof(TaskTemplateId))]
        [InverseProperty(nameof(PimsTaskTemplate.PimsTasks))]
        public virtual PimsTaskTemplate TaskTemplate { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(PimsUser.PimsTasks))]
        public virtual PimsUser User { get; set; }
    }
}
