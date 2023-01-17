using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACTIVITY_TEMPLATE")]
    [Index(nameof(ActivityTemplateTypeCode), Name = "ACTTMP_ACTIVITY_TEMPLATE_TYPE_CODE_IDX")]
    public partial class PimsActivityTemplate
    {
        public PimsActivityTemplate()
        {
            PimsActivityInstances = new HashSet<PimsActivityInstance>();
            PimsActivityTemplateDocuments = new HashSet<PimsActivityTemplateDocument>();
        }

        [Key]
        [Column("ACTIVITY_TEMPLATE_ID")]
        public long ActivityTemplateId { get; set; }
        [Required]
        [Column("ACTIVITY_TEMPLATE_TYPE_CODE")]
        [StringLength(20)]
        public string ActivityTemplateTypeCode { get; set; }
        [Required]
        [Column("ACTIVITY_TEMPLATE_JSON")]
        public string ActivityTemplateJson { get; set; }
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

        [ForeignKey(nameof(ActivityTemplateTypeCode))]
        [InverseProperty(nameof(PimsActivityTemplateType.PimsActivityTemplates))]
        public virtual PimsActivityTemplateType ActivityTemplateTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsActivityInstance.ActivityTemplate))]
        public virtual ICollection<PimsActivityInstance> PimsActivityInstances { get; set; }
        [InverseProperty(nameof(PimsActivityTemplateDocument.ActivityTemplate))]
        public virtual ICollection<PimsActivityTemplateDocument> PimsActivityTemplateDocuments { get; set; }
    }
}
