using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROJECT")]
    [Index(nameof(ProjectRiskTypeCode), Name = "PROJCT_PROJECT_RISK_TYPE_CODE_IDX")]
    [Index(nameof(ProjectStatusTypeCode), Name = "PROJCT_PROJECT_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(ProjectTierTypeCode), Name = "PROJCT_PROJECT_TIER_TYPE_CODE_IDX")]
    [Index(nameof(ProjectTypeCode), Name = "PROJCT_PROJECT_TYPE_CODE_IDX")]
    public partial class PimsProject
    {
        public PimsProject()
        {
            PimsActivities = new HashSet<PimsActivity>();
            PimsProjectNotes = new HashSet<PimsProjectNote>();
            PimsProjectProperties = new HashSet<PimsProjectProperty>();
            PimsProjectWorkflowModels = new HashSet<PimsProjectWorkflowModel>();
        }

        [Key]
        [Column("PROJECT_ID")]
        public long ProjectId { get; set; }
        [Required]
        [Column("PROJECT_TYPE_CODE")]
        [StringLength(20)]
        public string ProjectTypeCode { get; set; }
        [Required]
        [Column("PROJECT_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string ProjectStatusTypeCode { get; set; }
        [Required]
        [Column("PROJECT_RISK_TYPE_CODE")]
        [StringLength(20)]
        public string ProjectRiskTypeCode { get; set; }
        [Required]
        [Column("PROJECT_TIER_TYPE_CODE")]
        [StringLength(20)]
        public string ProjectTierTypeCode { get; set; }
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

        [ForeignKey(nameof(ProjectRiskTypeCode))]
        [InverseProperty(nameof(PimsProjectRiskType.PimsProjects))]
        public virtual PimsProjectRiskType ProjectRiskTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ProjectStatusTypeCode))]
        [InverseProperty(nameof(PimsProjectStatusType.PimsProjects))]
        public virtual PimsProjectStatusType ProjectStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ProjectTierTypeCode))]
        [InverseProperty(nameof(PimsProjectTierType.PimsProjects))]
        public virtual PimsProjectTierType ProjectTierTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ProjectTypeCode))]
        [InverseProperty(nameof(PimsProjectType.PimsProjects))]
        public virtual PimsProjectType ProjectTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsActivity.Project))]
        public virtual ICollection<PimsActivity> PimsActivities { get; set; }
        [InverseProperty(nameof(PimsProjectNote.Project))]
        public virtual ICollection<PimsProjectNote> PimsProjectNotes { get; set; }
        [InverseProperty(nameof(PimsProjectProperty.Project))]
        public virtual ICollection<PimsProjectProperty> PimsProjectProperties { get; set; }
        [InverseProperty(nameof(PimsProjectWorkflowModel.Project))]
        public virtual ICollection<PimsProjectWorkflowModel> PimsProjectWorkflowModels { get; set; }
    }
}
