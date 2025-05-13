using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity containing information regarding an disposition file.
/// </summary>
[Table("PIMS_DISPOSITION_FILE")]
[Index("DispositionFileStatusTypeCode", Name = "DISPFL_DISPOSITION_FILE_STATUS_TYPE_CODE_IDX")]
[Index("DispositionFundingTypeCode", Name = "DISPFL_DISPOSITION_FUNDING_TYPE_CODE_IDX")]
[Index("DispositionInitiatingDocTypeCode", Name = "DISPFL_DISPOSITION_INITIATING_DOC_TYPE_CODE_IDX")]
[Index("DispositionStatusTypeCode", Name = "DISPFL_DISPOSITION_STATUS_TYPE_CODE_IDX")]
[Index("DispositionTypeCode", Name = "DISPFL_DISPOSITION_TYPE_CODE_IDX")]
[Index("DspInitiatingBranchTypeCode", Name = "DISPFL_DSP_INITIATING_BRANCH_TYPE_CODE_IDX")]
[Index("DspPhysFileStatusTypeCode", Name = "DISPFL_DSP_PHYS_FILE_STATUS_TYPE_CODE_IDX")]
[Index("ProductId", Name = "DISPFL_PRODUCT_ID_IDX")]
[Index("ProjectId", Name = "DISPFL_PROJECT_ID_IDX")]
[Index("RegionCode", Name = "DISPFL_REGION_CODE_IDX")]
public partial class PimsDispositionFile
{
    /// <summary>
    /// Unique auto-generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    /// <summary>
    /// Code value for the dispostion status.
    /// </summary>
    [Required]
    [Column("DISPOSITION_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionStatusTypeCode { get; set; }

    /// <summary>
    /// Code value for the dispostion file status.
    /// </summary>
    [Required]
    [Column("DISPOSITION_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionFileStatusTypeCode { get; set; }

    /// <summary>
    /// Code value for the disposition type.
    /// </summary>
    [Required]
    [Column("DISPOSITION_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionTypeCode { get; set; }

    /// <summary>
    /// Code value for the disposition funding type.
    /// </summary>
    [Column("DISPOSITION_FUNDING_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionFundingTypeCode { get; set; }

    /// <summary>
    /// Code value for the dispostion initiating document type.
    /// </summary>
    [Column("DISPOSITION_INITIATING_DOC_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionInitiatingDocTypeCode { get; set; }

    /// <summary>
    /// Code value for the dispostion physical file status.
    /// </summary>
    [Column("DSP_PHYS_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DspPhysFileStatusTypeCode { get; set; }

    /// <summary>
    /// Code value for the dispostion initiating branch.
    /// </summary>
    [Column("DSP_INITIATING_BRANCH_TYPE_CODE")]
    [StringLength(20)]
    public string DspInitiatingBranchTypeCode { get; set; }

    /// <summary>
    /// Code value for the Ministry region code.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Foreign key reference to the project table.
    /// </summary>
    [Column("PROJECT_ID")]
    public long? ProjectId { get; set; }

    /// <summary>
    /// Foreign key reference to the product table.
    /// </summary>
    [Column("PRODUCT_ID")]
    public long? ProductId { get; set; }

    /// <summary>
    /// The formatted disposition file number, seeded from the PIMS_DISPOSITION_FILE_NO_SEQ sequence.  Sample formats are D-1, D-2, D-3, etc.
    /// </summary>
    [Column("FILE_NUMBER")]
    [StringLength(20)]
    public string FileNumber { get; set; }

    /// <summary>
    /// Name of the disposition file.
    /// </summary>
    [Column("FILE_NAME")]
    [StringLength(200)]
    public string FileName { get; set; }

    /// <summary>
    /// Provide available reference number for historic program or file number (e.g.? RAEG, Acquisition File, etc.).
    /// </summary>
    [Column("FILE_REFERENCE")]
    [StringLength(200)]
    public string FileReference { get; set; }

    /// <summary>
    /// Required if &quot;Other&quot; disposition type selected.
    /// </summary>
    [Column("OTHER_DISPOSITION_TYPE")]
    [StringLength(200)]
    public string OtherDispositionType { get; set; }

    /// <summary>
    /// Required if &quot;Other&quot; disposition initiating document type selected.
    /// </summary>
    [Column("OTHER_INITIATING_DOC_TYPE")]
    [StringLength(200)]
    public string OtherInitiatingDocType { get; set; }

    /// <summary>
    /// Date the disposition file was assigned.
    /// </summary>
    [Column("ASSIGNED_DT")]
    public DateOnly? AssignedDt { get; set; }

    /// <summary>
    /// Date the disposition file was completed.
    /// </summary>
    [Column("COMPLETED_DT")]
    public DateOnly? CompletedDt { get; set; }

    /// <summary>
    /// Signoff date of the initiating document.
    /// </summary>
    [Column("INITIATING_DOCUMENT_DT")]
    public DateOnly? InitiatingDocumentDt { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any.
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the record was created by the user.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
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

    [ForeignKey("DispositionFileStatusTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDispositionFileStatusType DispositionFileStatusTypeCodeNavigation { get; set; }

    [ForeignKey("DispositionFundingTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDispositionFundingType DispositionFundingTypeCodeNavigation { get; set; }

    [ForeignKey("DispositionInitiatingDocTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDispositionInitiatingDocType DispositionInitiatingDocTypeCodeNavigation { get; set; }

    [ForeignKey("DispositionStatusTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDispositionStatusType DispositionStatusTypeCodeNavigation { get; set; }

    [ForeignKey("DispositionTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDispositionType DispositionTypeCodeNavigation { get; set; }

    [ForeignKey("DspInitiatingBranchTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDspInitiatingBranchType DspInitiatingBranchTypeCodeNavigation { get; set; }

    [ForeignKey("DspPhysFileStatusTypeCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsDspPhysFileStatusType DspPhysFileStatusTypeCodeNavigation { get; set; }

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionAppraisal> PimsDispositionAppraisals { get; set; } = new List<PimsDispositionAppraisal>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionChecklistItem> PimsDispositionChecklistItems { get; set; } = new List<PimsDispositionChecklistItem>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionFileDocument> PimsDispositionFileDocuments { get; set; } = new List<PimsDispositionFileDocument>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionFileNote> PimsDispositionFileNotes { get; set; } = new List<PimsDispositionFileNote>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionFileProperty> PimsDispositionFileProperties { get; set; } = new List<PimsDispositionFileProperty>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionFileTeam> PimsDispositionFileTeams { get; set; } = new List<PimsDispositionFileTeam>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionOffer> PimsDispositionOffers { get; set; } = new List<PimsDispositionOffer>();

    [InverseProperty("DispositionFile")]
    public virtual ICollection<PimsDispositionSale> PimsDispositionSales { get; set; } = new List<PimsDispositionSale>();

    [ForeignKey("ProductId")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsProduct Product { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsProject Project { get; set; }

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsDispositionFiles")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }
}
