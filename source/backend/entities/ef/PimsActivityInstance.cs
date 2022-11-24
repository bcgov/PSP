using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACTIVITY_INSTANCE")]
    [Index(nameof(ActivityInstanceStatusTypeCode), Name = "ACTINS_ACTIVITY_INSTANCE_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(ActivityTemplateId), Name = "ACTINS_ACTIVITY_TEMPLATE_ID_IDX")]
    public partial class PimsActivityInstance
    {
        public PimsActivityInstance()
        {
            PimsAcquisitionActivityInstances = new HashSet<PimsAcquisitionActivityInstance>();
            PimsActInstPropAcqFiles = new HashSet<PimsActInstPropAcqFile>();
            PimsActInstPropRsrchFiles = new HashSet<PimsActInstPropRsrchFile>();
            PimsActivityInstanceDocuments = new HashSet<PimsActivityInstanceDocument>();
            PimsActivityInstanceNotes = new HashSet<PimsActivityInstanceNote>();
            PimsLeaseActivityInstances = new HashSet<PimsLeaseActivityInstance>();
            PimsResearchActivityInstances = new HashSet<PimsResearchActivityInstance>();
        }

        [Key]
        [Column("ACTIVITY_INSTANCE_ID")]
        public long ActivityInstanceId { get; set; }
        [Column("ACTIVITY_TEMPLATE_ID")]
        public long? ActivityTemplateId { get; set; }
        [Required]
        [Column("ACTIVITY_INSTANCE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string ActivityInstanceStatusTypeCode { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [Column("ACTIVITY_DATA_JSON")]
        public string ActivityDataJson { get; set; }
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

        [ForeignKey(nameof(ActivityInstanceStatusTypeCode))]
        [InverseProperty(nameof(PimsActivityInstanceStatusType.PimsActivityInstances))]
        public virtual PimsActivityInstanceStatusType ActivityInstanceStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ActivityTemplateId))]
        [InverseProperty(nameof(PimsActivityTemplate.PimsActivityInstances))]
        public virtual PimsActivityTemplate ActivityTemplate { get; set; }
        [InverseProperty(nameof(PimsAcquisitionActivityInstance.ActivityInstance))]
        public virtual ICollection<PimsAcquisitionActivityInstance> PimsAcquisitionActivityInstances { get; set; }
        [InverseProperty(nameof(PimsActInstPropAcqFile.ActivityInstance))]
        public virtual ICollection<PimsActInstPropAcqFile> PimsActInstPropAcqFiles { get; set; }
        [InverseProperty(nameof(PimsActInstPropRsrchFile.ActivityInstance))]
        public virtual ICollection<PimsActInstPropRsrchFile> PimsActInstPropRsrchFiles { get; set; }
        [InverseProperty(nameof(PimsActivityInstanceDocument.ActivityInstance))]
        public virtual ICollection<PimsActivityInstanceDocument> PimsActivityInstanceDocuments { get; set; }
        [InverseProperty(nameof(PimsActivityInstanceNote.ActivityInstance))]
        public virtual ICollection<PimsActivityInstanceNote> PimsActivityInstanceNotes { get; set; }
        [InverseProperty(nameof(PimsLeaseActivityInstance.ActivityInstance))]
        public virtual ICollection<PimsLeaseActivityInstance> PimsLeaseActivityInstances { get; set; }
        [InverseProperty(nameof(PimsResearchActivityInstance.ActivityInstance))]
        public virtual ICollection<PimsResearchActivityInstance> PimsResearchActivityInstances { get; set; }
    }
}
