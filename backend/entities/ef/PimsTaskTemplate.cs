using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_TASK_TEMPLATE")]
    [Index(nameof(TaskTemplateTypeCode), Name = "TSKTMP_TASK_TEMPLATE_TYPE_CODE_IDX")]
    public partial class PimsTaskTemplate
    {
        public PimsTaskTemplate()
        {
            PimsTaskTemplateActivityModels = new HashSet<PimsTaskTemplateActivityModel>();
            PimsTasks = new HashSet<PimsTask>();
        }

        [Key]
        [Column("TASK_TEMPLATE_ID")]
        public long TaskTemplateId { get; set; }
        [Required]
        [Column("TASK_TEMPLATE_TYPE_CODE")]
        [StringLength(20)]
        public string TaskTemplateTypeCode { get; set; }
        [Required]
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

        [ForeignKey(nameof(TaskTemplateTypeCode))]
        [InverseProperty(nameof(PimsTaskTemplateType.PimsTaskTemplates))]
        public virtual PimsTaskTemplateType TaskTemplateTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsTaskTemplateActivityModel.TaskTemplate))]
        public virtual ICollection<PimsTaskTemplateActivityModel> PimsTaskTemplateActivityModels { get; set; }
        [InverseProperty(nameof(PimsTask.TaskTemplate))]
        public virtual ICollection<PimsTask> PimsTasks { get; set; }
    }
}
