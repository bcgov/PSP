using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACTIVITY")]
    [Index(nameof(ActivityModelId), Name = "ACTVTY_ACTIVITY_MODEL_ID_IDX")]
    [Index(nameof(ProjectId), Name = "ACTVTY_PROJECT_ID_IDX")]
    [Index(nameof(WorkflowId), Name = "ACTVTY_WORKFLOW_ID_IDX")]
    public partial class PimsActivity
    {
        public PimsActivity()
        {
            PimsPropertyActivities = new HashSet<PimsPropertyActivity>();
            PimsTasks = new HashSet<PimsTask>();
        }

        [Key]
        [Column("ACTIVITY_ID")]
        public long ActivityId { get; set; }
        [Column("PROJECT_ID")]
        public long? ProjectId { get; set; }
        [Column("WORKFLOW_ID")]
        public long? WorkflowId { get; set; }
        [Column("ACTIVITY_MODEL_ID")]
        public long ActivityModelId { get; set; }
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
        [InverseProperty(nameof(PimsActivityModel.PimsActivities))]
        public virtual PimsActivityModel ActivityModel { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(PimsProject.PimsActivities))]
        public virtual PimsProject Project { get; set; }
        [ForeignKey(nameof(WorkflowId))]
        [InverseProperty(nameof(PimsProjectWorkflowModel.PimsActivities))]
        public virtual PimsProjectWorkflowModel Workflow { get; set; }
        [InverseProperty(nameof(PimsPropertyActivity.Activity))]
        public virtual ICollection<PimsPropertyActivity> PimsPropertyActivities { get; set; }
        [InverseProperty(nameof(PimsTask.Activity))]
        public virtual ICollection<PimsTask> PimsTasks { get; set; }
    }
}
