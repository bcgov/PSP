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
[Index("AcqPhysFileStatusTypeCode", Name = "ACQNFL_ACQ_PHYS_FILE_STATUS_TYPE_CODE_IDX")]
[Index("FileNumber", Name = "ACQNFL_FILE_NUMBER_IDX")]
[Index("LegacyFileNumber", Name = "ACQNFL_LEGACY_FILE_NUMBER_IDX")]
[Index("ProductId", Name = "ACQNFL_PRODUCT_ID_IDX")]
[Index("ProjectId", Name = "ACQNFL_PROJECT_ID_IDX")]
[Index("RegionCode", Name = "ACQNFL_REGION_CODE_IDX")]
public partial class PimsAcquisitionFile
{
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

    /// <summary>
    /// Region responsible for oversight of the acquisition.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

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
    /// Formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to &apos;01&apos;)
    /// </summary>
    [Required]
    [Column("FILE_NUMBER")]
    [StringLength(18)]
    public string FileNumber { get; set; }

    /// <summary>
    /// Legacy formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to &apos;01&apos;).   Required due to some files having t
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
    public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; } = new List<PimsExpropriationPayment>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsInterestHolder> PimsInterestHolders { get; set; } = new List<PimsInterestHolder>();

    [InverseProperty("AcquisitionFile")]
    public virtual ICollection<PimsPropertyAcquisitionFile> PimsPropertyAcquisitionFiles { get; set; } = new List<PimsPropertyAcquisitionFile>();

    [ForeignKey("ProductId")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsProduct Product { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsProject Project { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsAcquisitionFiles")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }
}
