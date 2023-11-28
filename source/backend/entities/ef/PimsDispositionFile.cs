using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_DISPOSITION_FILE")]
    [Index(nameof(DispositionFileStatusTypeCode), Name = "DISPFL_DISPOSITION_FILE_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(DispositionFundingTypeCode), Name = "DISPFL_DISPOSITION_FUNDING_TYPE_CODE_IDX")]
    [Index(nameof(DispositionInitiatingDocTypeCode), Name = "DISPFL_DISPOSITION_INITIATING_DOC_TYPE_CODE_IDX")]
    [Index(nameof(DispositionStatusTypeCode), Name = "DISPFL_DISPOSITION_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(DispositionTypeCode), Name = "DISPFL_DISPOSITION_TYPE_CODE_IDX")]
    [Index(nameof(DspInitiatingBranchTypeCode), Name = "DISPFL_DSP_INITIATING_BRANCH_TYPE_CODE_IDX")]
    [Index(nameof(DspPhysFileStatusTypeCode), Name = "DISPFL_DSP_PHYS_FILE_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(RegionCode), Name = "DISPFL_REGION_CODE_IDX")]
    public partial class PimsDispositionFile
    {
        public PimsDispositionFile()
        {
            PimsDispositionFileTeams = new HashSet<PimsDispositionFileTeam>();
            PimsPropertyDispositionFiles = new HashSet<PimsPropertyDispositionFile>();
        }

        [Key]
        [Column("DISPOSITION_FILE_ID")]
        public long DispositionFileId { get; set; }
        [Required]
        [Column("DISPOSITION_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DispositionStatusTypeCode { get; set; }
        [Required]
        [Column("DISPOSITION_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DispositionFileStatusTypeCode { get; set; }
        [Required]
        [Column("DISPOSITION_TYPE_CODE")]
        [StringLength(20)]
        public string DispositionTypeCode { get; set; }
        [Column("DISPOSITION_FUNDING_TYPE_CODE")]
        [StringLength(20)]
        public string DispositionFundingTypeCode { get; set; }
        [Column("DISPOSITION_INITIATING_DOC_TYPE_CODE")]
        [StringLength(20)]
        public string DispositionInitiatingDocTypeCode { get; set; }
        [Column("DSP_PHYS_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string DspPhysFileStatusTypeCode { get; set; }
        [Column("DSP_INITIATING_BRANCH_TYPE_CODE")]
        [StringLength(20)]
        public string DspInitiatingBranchTypeCode { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Column("FILE_NUMBER")]
        [StringLength(20)]
        public string FileNumber { get; set; }
        [Column("FILE_NAME")]
        [StringLength(200)]
        public string FileName { get; set; }
        [Column("FILE_REFERENCE")]
        [StringLength(200)]
        public string FileReference { get; set; }
        [Column("OTHER_DISPOSITION_TYPE")]
        [StringLength(200)]
        public string OtherDispositionType { get; set; }
        [Column("OTHER_INITIATING_DOC_TYPE")]
        [StringLength(200)]
        public string OtherInitiatingDocType { get; set; }
        [Column("ASSIGNED_DT", TypeName = "date")]
        public DateTime? AssignedDt { get; set; }
        [Column("COMPLETED_DT", TypeName = "date")]
        public DateTime? CompletedDt { get; set; }
        [Column("INITIATING_DOCUMENT_DT", TypeName = "date")]
        public DateTime? InitiatingDocumentDt { get; set; }
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

        [ForeignKey(nameof(DispositionFileStatusTypeCode))]
        [InverseProperty(nameof(PimsDispositionFileStatusType.PimsDispositionFiles))]
        public virtual PimsDispositionFileStatusType DispositionFileStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DispositionFundingTypeCode))]
        [InverseProperty(nameof(PimsDispositionFundingType.PimsDispositionFiles))]
        public virtual PimsDispositionFundingType DispositionFundingTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DispositionInitiatingDocTypeCode))]
        [InverseProperty(nameof(PimsDispositionInitiatingDocType.PimsDispositionFiles))]
        public virtual PimsDispositionInitiatingDocType DispositionInitiatingDocTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DispositionStatusTypeCode))]
        [InverseProperty(nameof(PimsDispositionStatusType.PimsDispositionFiles))]
        public virtual PimsDispositionStatusType DispositionStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DispositionTypeCode))]
        [InverseProperty(nameof(PimsDispositionType.PimsDispositionFiles))]
        public virtual PimsDispositionType DispositionTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DspInitiatingBranchTypeCode))]
        [InverseProperty(nameof(PimsDspInitiatingBranchType.PimsDispositionFiles))]
        public virtual PimsDspInitiatingBranchType DspInitiatingBranchTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(DspPhysFileStatusTypeCode))]
        [InverseProperty(nameof(PimsDspPhysFileStatusType.PimsDispositionFiles))]
        public virtual PimsDspPhysFileStatusType DspPhysFileStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsDispositionFiles))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsDispositionFileTeam.DispositionFile))]
        public virtual ICollection<PimsDispositionFileTeam> PimsDispositionFileTeams { get; set; }
        [InverseProperty(nameof(PimsPropertyDispositionFile.DispositionFile))]
        public virtual ICollection<PimsPropertyDispositionFile> PimsPropertyDispositionFiles { get; set; }
    }
}
