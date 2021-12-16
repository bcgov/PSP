﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROJECT_WORKFLOW_MODEL")]
    [Index(nameof(ProjectId), Name = "PRWKMD_PROJECT_ID_IDX")]
    [Index(nameof(ProjectId), nameof(WorkflowModelId), Name = "PRWKMD_PROJECT_WORKFLOW_MODEL_TUC", IsUnique = true)]
    [Index(nameof(WorkflowModelId), Name = "PRWKMD_WORKFLOW_MODEL_ID_IDX")]
    public partial class PimsProjectWorkflowModel
    {
        public PimsProjectWorkflowModel()
        {
            PimsActivities = new HashSet<PimsActivity>();
        }

        [Key]
        [Column("PROJECT_WORKFLOW_MODEL_ID")]
        public long ProjectWorkflowModelId { get; set; }
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }
        [Column("WORKFLOW_MODEL_ID")]
        public long WorkflowModelId { get; set; }
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

        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(PimsProject.PimsProjectWorkflowModels))]
        public virtual PimsProject Project { get; set; }
        [ForeignKey(nameof(WorkflowModelId))]
        [InverseProperty(nameof(PimsWorkflowModel.PimsProjectWorkflowModels))]
        public virtual PimsWorkflowModel WorkflowModel { get; set; }
        [InverseProperty(nameof(PimsActivity.Workflow))]
        public virtual ICollection<PimsActivity> PimsActivities { get; set; }
    }
}
