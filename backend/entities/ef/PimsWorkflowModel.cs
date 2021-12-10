using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_WORKFLOW_MODEL")]
    [Index(nameof(WorkflowModelTypeCode), Name = "WFLMDL_WORKFLOW_MODEL_TYPE_CODE_IDX")]
    public partial class PimsWorkflowModel
    {
        public PimsWorkflowModel()
        {
            PimsProjectWorkflowModels = new HashSet<PimsProjectWorkflowModel>();
        }

        [Key]
        [Column("WORKFLOW_MODEL_ID")]
        public long WorkflowModelId { get; set; }
        [Required]
        [Column("WORKFLOW_MODEL_TYPE_CODE")]
        [StringLength(20)]
        public string WorkflowModelTypeCode { get; set; }
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

        [ForeignKey(nameof(WorkflowModelTypeCode))]
        [InverseProperty(nameof(PimsWorkflowModelType.PimsWorkflowModels))]
        public virtual PimsWorkflowModelType WorkflowModelTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsProjectWorkflowModel.WorkflowModel))]
        public virtual ICollection<PimsProjectWorkflowModel> PimsProjectWorkflowModels { get; set; }
    }
}
