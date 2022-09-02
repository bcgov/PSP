using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACT_INST_PROP_RSRCH_FILE")]
    [Index(nameof(ActivityInstanceId), Name = "AIPRFL_ACTIVITY_INSTANCE_ID_IDX")]
    [Index(nameof(PropertyResearchFileId), nameof(ActivityInstanceId), Name = "AIPRFL_ACT_INST_PROP_RSRCH_FL_TUC", IsUnique = true)]
    [Index(nameof(PropertyResearchFileId), Name = "AIPRFL_PROPERTY_RESEARCH_FILE_ID_IDX")]
    public partial class PimsActInstPropRsrchFile
    {
        [Key]
        [Column("ACT_INST_PROP_RSRCH_FILE_ID")]
        public long ActInstPropRsrchFileId { get; set; }
        [Column("ACTIVITY_INSTANCE_ID")]
        public long ActivityInstanceId { get; set; }
        [Column("PROPERTY_RESEARCH_FILE_ID")]
        public long PropertyResearchFileId { get; set; }
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

        [ForeignKey(nameof(ActivityInstanceId))]
        [InverseProperty(nameof(PimsActivityInstance.PimsActInstPropRsrchFiles))]
        public virtual PimsActivityInstance ActivityInstance { get; set; }
        [ForeignKey(nameof(PropertyResearchFileId))]
        [InverseProperty(nameof(PimsPropertyResearchFile.PimsActInstPropRsrchFiles))]
        public virtual PimsPropertyResearchFile PropertyResearchFile { get; set; }
    }
}
