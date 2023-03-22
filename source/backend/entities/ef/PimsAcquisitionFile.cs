using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACQUISITION_FILE")]
    [Index(nameof(AcquisitionFileStatusTypeCode), Name = "ACQNFL_ACQUISITION_FILE_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(AcquisitionFundingTypeCode), Name = "ACQNFL_ACQUISITION_FUNDING_TYPE_CODE_IDX")]
    [Index(nameof(AcquisitionTypeCode), Name = "ACQNFL_ACQUISITION_TYPE_CODE_IDX")]
    [Index(nameof(AcqPhysFileStatusTypeCode), Name = "ACQNFL_ACQ_PHYS_FILE_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(FileNumber), Name = "ACQNFL_FILE_NUMBER_IDX")]
    [Index(nameof(LegacyFileNumber), Name = "ACQNFL_LEGACY_FILE_NUMBER_IDX")]
    [Index(nameof(ProductId), Name = "ACQNFL_PRODUCT_ID_IDX")]
    [Index(nameof(ProjectId), Name = "ACQNFL_PROJECT_ID_IDX")]
    [Index(nameof(RegionCode), Name = "ACQNFL_REGION_CODE_IDX")]
    public partial class PimsAcquisitionFile
    {
        public PimsAcquisitionFile()
        {
            PimsAcquisitionActivityInstances = new HashSet<PimsAcquisitionActivityInstance>();
            PimsAcquisitionChecklistItems = new HashSet<PimsAcquisitionChecklistItem>();
            PimsAcquisitionFileDocuments = new HashSet<PimsAcquisitionFileDocument>();
            PimsAcquisitionFileForms = new HashSet<PimsAcquisitionFileForm>();
            PimsAcquisitionFileNotes = new HashSet<PimsAcquisitionFileNote>();
            PimsAcquisitionFilePeople = new HashSet<PimsAcquisitionFilePerson>();
            PimsAcquisitionOwners = new HashSet<PimsAcquisitionOwner>();
            PimsPropertyAcquisitionFiles = new HashSet<PimsPropertyAcquisitionFile>();
        }

        [Key]
        [Column("ACQUISITION_FILE_ID")]
        public long AcquisitionFileId { get; set; }
        [Column("PROJECT_ID")]
        public long? ProjectId { get; set; }
        [Column("PRODUCT_ID")]
        public long? ProductId { get; set; }
        [Required]
        [Column("ACQUISITION_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string AcquisitionFileStatusTypeCode { get; set; }
        [Required]
        [Column("ACQUISITION_TYPE_CODE")]
        [StringLength(20)]
        public string AcquisitionTypeCode { get; set; }
        [Column("ACQUISITION_FUNDING_TYPE_CODE")]
        [StringLength(20)]
        public string AcquisitionFundingTypeCode { get; set; }
        [Column("ACQ_PHYS_FILE_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string AcqPhysFileStatusTypeCode { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Column("MINISTRY_PROJECT_NUMBER")]
        [StringLength(20)]
        public string MinistryProjectNumber { get; set; }
        [Column("MINISTRY_PROJECT_NAME")]
        [StringLength(100)]
        public string MinistryProjectName { get; set; }
        [Column("CPS_PRODUCT_CODE")]
        [StringLength(12)]
        public string CpsProductCode { get; set; }
        [Required]
        [Column("FILE_NAME")]
        [StringLength(500)]
        public string FileName { get; set; }
        [Column("FILE_NO")]
        public int FileNo { get; set; }
        [Required]
        [Column("FILE_NUMBER")]
        [StringLength(18)]
        public string FileNumber { get; set; }
        [Column("LEGACY_FILE_NUMBER")]
        [StringLength(18)]
        public string LegacyFileNumber { get; set; }
        [Column("FUNDING_OTHER")]
        [StringLength(200)]
        public string FundingOther { get; set; }
        [Column("ASSIGNED_DATE", TypeName = "datetime")]
        public DateTime? AssignedDate { get; set; }
        [Column("DELIVERY_DATE", TypeName = "datetime")]
        public DateTime? DeliveryDate { get; set; }
        [Column("COMPLETION_DATE", TypeName = "datetime")]
        public DateTime? CompletionDate { get; set; }
        [Column("PAIMS_ACQUISITION_FILE_ID")]
        public int? PaimsAcquisitionFileId { get; set; }
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

        [ForeignKey(nameof(AcqPhysFileStatusTypeCode))]
        [InverseProperty(nameof(PimsAcqPhysFileStatusType.PimsAcquisitionFiles))]
        public virtual PimsAcqPhysFileStatusType AcqPhysFileStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcquisitionFileStatusTypeCode))]
        [InverseProperty(nameof(PimsAcquisitionFileStatusType.PimsAcquisitionFiles))]
        public virtual PimsAcquisitionFileStatusType AcquisitionFileStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcquisitionFundingTypeCode))]
        [InverseProperty(nameof(PimsAcquisitionFundingType.PimsAcquisitionFiles))]
        public virtual PimsAcquisitionFundingType AcquisitionFundingTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(AcquisitionTypeCode))]
        [InverseProperty(nameof(PimsAcquisitionType.PimsAcquisitionFiles))]
        public virtual PimsAcquisitionType AcquisitionTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ProductId))]
        [InverseProperty(nameof(PimsProduct.PimsAcquisitionFiles))]
        public virtual PimsProduct Product { get; set; }
        [ForeignKey(nameof(ProjectId))]
        [InverseProperty(nameof(PimsProject.PimsAcquisitionFiles))]
        public virtual PimsProject Project { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsAcquisitionFiles))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsAcquisitionActivityInstance.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionActivityInstance> PimsAcquisitionActivityInstances { get; set; }
        [InverseProperty(nameof(PimsAcquisitionChecklistItem.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionChecklistItem> PimsAcquisitionChecklistItems { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFileDocument.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionFileDocument> PimsAcquisitionFileDocuments { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFileForm.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionFileForm> PimsAcquisitionFileForms { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFileNote.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionFileNote> PimsAcquisitionFileNotes { get; set; }
        [InverseProperty(nameof(PimsAcquisitionFilePerson.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionFilePerson> PimsAcquisitionFilePeople { get; set; }
        [InverseProperty(nameof(PimsAcquisitionOwner.AcquisitionFile))]
        public virtual ICollection<PimsAcquisitionOwner> PimsAcquisitionOwners { get; set; }
        [InverseProperty(nameof(PimsPropertyAcquisitionFile.AcquisitionFile))]
        public virtual ICollection<PimsPropertyAcquisitionFile> PimsPropertyAcquisitionFiles { get; set; }
    }
}
