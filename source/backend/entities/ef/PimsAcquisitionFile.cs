using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity containing information regarding an acquisition file.
/// </summary>
[Table("PIMS_ACQUISITION_FILE")]
[Index("AcquisitionFileStatusTypeCode", Name = "ACQNFL_ACQUISITION_FILE_STATUS_TYPE_CODE_IDX")]
[Index("AcquisitionFundingTypeCode", Name = "ACQNFL_ACQUISITION_FUNDING_TYPE_CODE_IDX")]
[Index("AcquisitionTypeCode", Name = "ACQNFL_ACQUISITION_TYPE_CODE_IDX")]
[Index("AcqFileAppraisalTypeCode", Name = "ACQNFL_ACQ_FILE_APPRAISAL_TYPE_CODE_IDX")]
[Index("AcqFileExpropRiskTypeCode", Name = "ACQNFL_ACQ_FILE_EXPROP_RISK_TYPE_CODE_IDX")]
[Index("AcqFileLglSrvyTypeCode", Name = "ACQNFL_ACQ_FILE_LGL_SRVY_TYPE_CODE_IDX")]
[Index("AcqPhysFileStatusTypeCode", Name = "ACQNFL_ACQ_PHYS_FILE_STATUS_TYPE_CODE_IDX")]
[Index("FileNo", Name = "ACQNFL_FILE_NO_IDX")]
[Index("LegacyFileNumber", Name = "ACQNFL_LEGACY_FILE_NUMBER_IDX")]
[Index("PrntAcquisitionFileId", Name = "ACQNFL_PRNT_ACQUISITION_FILE_ID_IDX")]
[Index("ProductId", Name = "ACQNFL_PRODUCT_ID_IDX")]
[Index("ProjectId", Name = "ACQNFL_PROJECT_ID_IDX")]
[Index("RegionCode", Name = "ACQNFL_REGION_CODE_IDX")]
[Index("SubfileInterestTypeCode", Name = "ACQNFL_SUBFILE_INTEREST_TYPE_CODE_IDX")]
public partial class PimsAcquisitionFile
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    /// <summary>
    /// Link to the parent acquisition file.
    /// </summary>
    [Column("PRNT_ACQUISITION_FILE_ID")]
    public long? PrntAcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROJECT table.
    /// </summary>
    [Column("PROJECT_ID")]
    public long? ProjectId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PRODUCT table.
    /// </summary>
    [Column("PRODUCT_ID")]
    public long? ProductId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FILE_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("ACQUISITION_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string AcquisitionFileStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_TYPE table.
    /// </summary>
    [Required]
    [Column("ACQUISITION_TYPE_CODE")]
    [StringLength(20)]
    public string AcquisitionTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FUNDING_TYPE table.
    /// </summary>
    [Column("ACQUISITION_FUNDING_TYPE_CODE")]
    [StringLength(20)]
    public string AcquisitionFundingTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQ_PHYS_FILE_STATUS_TYPE table.
    /// </summary>
    [Column("ACQ_PHYS_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string AcqPhysFileStatusTypeCode { get; set; }

    /// <summary>
    /// Region responsible for oversight of the acquisition.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_SUBFILE_INTEREST_TYPE table.
    /// </summary>
    [Column("SUBFILE_INTEREST_TYPE_CODE")]
    [StringLength(20)]
    public string SubfileInterestTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQ_FILE_APPRAISAL_TYPE table.
    /// </summary>
    [Column("ACQ_FILE_APPRAISAL_TYPE_CODE")]
    [StringLength(20)]
    public string AcqFileAppraisalTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQ_FILE_LGL_SRVY_TYPE table.
    /// </summary>
    [Column("ACQ_FILE_LGL_SRVY_TYPE_CODE")]
    [StringLength(20)]
    public string AcqFileLglSrvyTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQ_FILE_EXPROP_RISK_TYPE table.
    /// </summary>
    [Column("ACQ_FILE_EXPROP_RISK_TYPE_CODE")]
    [StringLength(20)]
    public string AcqFileExpropRiskTypeCode { get; set; }

    /// <summary>
    /// Descriptive name given to the acquisition file.
    /// </summary>
    [Required]
    [Column("FILE_NAME")]
    [StringLength(500)]
    public string FileName { get; set; }

    /// <summary>
    /// File number assigned to the acquisition file.
    /// </summary>
    [Column("FILE_NO")]
    public int FileNo { get; set; }

    /// <summary>
    /// Acquisition file number suffix
    /// </summary>
    [Column("FILE_NO_SUFFIX")]
    public short FileNoSuffix { get; set; }

    /// <summary>
    /// Legacy formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTT region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to &apos;01&apos;).   Required due to some files having t
    /// </summary>
    [Column("LEGACY_FILE_NUMBER")]
    [StringLength(18)]
    public string LegacyFileNumber { get; set; }

    /// <summary>
    /// Legacy stakeholders imported from PAIMS.
    /// </summary>
    [Column("LEGACY_STAKEHOLDER")]
    [StringLength(4000)]
    public string LegacyStakeholder { get; set; }

    /// <summary>
    /// Description of other funding type.
    /// </summary>
    [Column("FUNDING_OTHER")]
    [StringLength(200)]
    public string FundingOther { get; set; }

    /// <summary>
    /// Date of file assignment.
    /// </summary>
    [Column("ASSIGNED_DATE", TypeName = "datetime")]
    public DateTime? AssignedDate { get; set; }

    /// <summary>
    /// Date of file delivery.
    /// </summary>
    [Column("DELIVERY_DATE", TypeName = "datetime")]
    public DateTime? DeliveryDate { get; set; }

    /// <summary>
    /// Legacy Acquisition File ID from the PAIMS system.
    /// </summary>
    [Column("PAIMS_ACQUISITION_FILE_ID")]
    public int? PaimsAcquisitionFileId { get; set; }

    /// <summary>
    /// The maximum allowable compensation for the acquisition file.  This amount should not be exceeded by the total of all assiciated H120&apos;s.
    /// </summary>
    [Column("TOTAL_ALLOWABLE_COMPENSATION", TypeName = "money")]
    public decimal? TotalAllowableCompensation { get; set; }

    /// <summary>
    /// If the user selects ?Other? then they will need to provide a subfile type description, which will be displayed as &apos;Other - &lt;description&gt;
    /// </summary>
    [Column("OTHER_SUBFILE_INTEREST_TYPE")]
    [StringLength(200)]
    public string OtherSubfileInterestType { get; set; }

    /// <summary>
    /// Estimated date by which the acquisition would be completed.
    /// </summary>
    [Column("EST_COMPLETION_DT", TypeName = "datetime")]
    public DateTime? EstCompletionDt { get; set; }

    /// <summary>
    /// Date of possession following acquisition completion.
    /// </summary>
    [Column("POSSESSION_DT", TypeName = "datetime")]
    public DateTime? PossessionDt { get; set; }

    /// <summary>
    /// Comments to provide details about the physical acquisition file.
    /// </summary>
    [Column("PHYSICAL_FILE_DETAILS")]
    [StringLength(2000)]
    public string PhysicalFileDetails { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the user updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// The GUID of the user account that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// The directory of the user account that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AcqFileAppraisalTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcqFileAppraisalType AcqFileAppraisalTypeCodeNavigation { get; set; }

    [ForeignKey("AcqFileExpropRiskTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcqFileExpropRiskType AcqFileExpropRiskTypeCodeNavigation { get; set; }

    [ForeignKey("AcqFileLglSrvyTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcqFileLglSrvyType AcqFileLglSrvyTypeCodeNavigation { get; set; }

    [ForeignKey("AcqPhysFileStatusTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcqPhysFileStatusType AcqPhysFileStatusTypeCodeNavigation { get; set; }

    [ForeignKey("AcquisitionFileStatusTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcquisitionFileStatusType AcquisitionFileStatusTypeCodeNavigation { get; set; }

    [ForeignKey("AcquisitionFundingTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcquisitionFundingType AcquisitionFundingTypeCodeNavigation { get; set; }

    [ForeignKey("AcquisitionTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsAcquisitionType AcquisitionTypeCodeNavigation { get; set; }

    [InverseProperty("PrntAcquisitionFile")]
    public virtual ICollection<PimsAcquisitionFile> InversePrntAcquisitionFile { get; set; } = new List<PimsAcquisitionFile>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcqFileAcqFlTakeTyp> PimsAcqFileAcqFlTakeTyps { get; set; } = new List<PimsAcqFileAcqFlTakeTyp>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcqFileAcqProgress> PimsAcqFileAcqProgresses { get; set; } = new List<PimsAcqFileAcqProgress>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcquisitionChecklistItem> PimsAcquisitionChecklistItems { get; set; } = new List<PimsAcquisitionChecklistItem>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcquisitionFileDocument> PimsAcquisitionFileDocuments { get; set; } = new List<PimsAcquisitionFileDocument>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcquisitionFileForm> PimsAcquisitionFileForms { get; set; } = new List<PimsAcquisitionFileForm>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcquisitionFileNote> PimsAcquisitionFileNotes { get; set; } = new List<PimsAcquisitionFileNote>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcquisitionFileTeam> PimsAcquisitionFileTeams { get; set; } = new List<PimsAcquisitionFileTeam>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAcquisitionOwner> PimsAcquisitionOwners { get; set; } = new List<PimsAcquisitionOwner>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsAgreement> PimsAgreements { get; set; } = new List<PimsAgreement>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsCompensationRequisition> PimsCompensationRequisitions { get; set; } = new List<PimsCompensationRequisition>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsExpropOwnerHistory> PimsExpropOwnerHistories { get; set; } = new List<PimsExpropOwnerHistory>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; } = new List<PimsExpropriationPayment>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsInterestHolder> PimsInterestHolders { get; set; } = new List<PimsInterestHolder>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsPropertyAcquisitionFile> PimsPropertyAcquisitionFiles { get; set; } = new List<PimsPropertyAcquisitionFile>();

    [ForeignKey("PrntAcquisitionFileId")]
    [InverseProperty("InversePrntAcquisitionFile")]
    public virtual PimsAcquisitionFile PrntAcquisitionFile { get; set; }

    [ForeignKey("ProductId")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsProduct Product { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsProject Project { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }

    [ForeignKey("SubfileInterestTypeCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsSubfileInterestType SubfileInterestTypeCodeNavigation { get; set; }
}
