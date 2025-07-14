using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing information about the management file.
/// </summary>
[Table("PIMS_MANAGEMENT_FILE")]
[Index("AcquisitionFundingTypeCode", Name = "MGMTFL_ACQUISITION_FUNDING_TYPE_CODE_IDX")]
[Index("FileName", Name = "MGMTFL_FILE_NAME_TUC", IsUnique = true)]
[Index("ManagementFilePurposeTypeCode", Name = "MGMTFL_MANAGEMENT_FILE_PURPOSE_TYPE_CODE_IDX")]
[Index("ManagementFileStatusTypeCode", Name = "MGMTFL_MANAGEMENT_FILE_STATUS_TYPE_CODE_IDX")]
[Index("ProductId", Name = "MGMTFL_PRODUCT_ID_IDX")]
[Index("ProjectId", Name = "MGMTFL_PROJECT_ID_IDX")]
public partial class PimsManagementFile
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("MANAGEMENT_FILE_ID")]
    public long ManagementFileId { get; set; }

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
    /// Foreign key to the PIMS_ACQUISITION_FUNDING_TYPE table.
    /// </summary>
    [Column("ACQUISITION_FUNDING_TYPE_CODE")]
    [StringLength(20)]
    public string AcquisitionFundingTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_MANAGEMENT_FILE_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("MANAGEMENT_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ManagementFileStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_MANAGEMENT_FILE_PURPOSE_TYPE table.
    /// </summary>
    [Required]
    [Column("MANAGEMENT_FILE_PURPOSE_TYPE_CODE")]
    [StringLength(20)]
    public string ManagementFilePurposeTypeCode { get; set; }

    /// <summary>
    /// Unique name given to the management file.
    /// </summary>
    [Required]
    [Column("FILE_NAME")]
    [StringLength(500)]
    public string FileName { get; set; }

    /// <summary>
    /// Legacy formatted file number assigned to the acquisition file.  Format follows YY-XXXXXX-ZZ where YY = MoTI region number, XXXXXX = generated integer sequence number,  and ZZ = file suffix number (defaulting to &apos;01&apos;).   Required due to some files having t
    /// </summary>
    [Column("LEGACY_FILE_NUM")]
    [StringLength(100)]
    public string LegacyFileNum { get; set; }

    /// <summary>
    /// Free text description of the file&apos;s purpose.
    /// </summary>
    [Column("FILE_PURPOSE")]
    [StringLength(2000)]
    public string FilePurpose { get; set; }

    /// <summary>
    /// Additional details of the management file.
    /// </summary>
    [Column("ADDITIONAL_DETAILS")]
    [StringLength(2000)]
    public string AdditionalDetails { get; set; }

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

    [ForeignKey("AcquisitionFundingTypeCode")]
    [InverseProperty("PimsManagementFiles")]
    public virtual PimsAcquisitionFundingType AcquisitionFundingTypeCodeNavigation { get; set; }

    [ForeignKey("ManagementFilePurposeTypeCode")]
    [InverseProperty("PimsManagementFiles")]
    public virtual PimsManagementFilePurposeType ManagementFilePurposeTypeCodeNavigation { get; set; }

    [ForeignKey("ManagementFileStatusTypeCode")]
    [InverseProperty("PimsManagementFiles")]
    public virtual PimsManagementFileStatusType ManagementFileStatusTypeCodeNavigation { get; set; }

    [InverseProperty("ManagementFile")]
    public virtual ICollection<PimsManagementActivity> PimsManagementActivities { get; set; } = new List<PimsManagementActivity>();

    [InverseProperty("ManagementFile")]
    public virtual ICollection<PimsManagementFileContact> PimsManagementFileContacts { get; set; } = new List<PimsManagementFileContact>();

    [InverseProperty("ManagementFile")]
    public virtual ICollection<PimsManagementFileDocument> PimsManagementFileDocuments { get; set; } = new List<PimsManagementFileDocument>();

    [InverseProperty("ManagementFile")]
    public virtual ICollection<PimsManagementFileNote> PimsManagementFileNotes { get; set; } = new List<PimsManagementFileNote>();

    [InverseProperty("ManagementFile")]
    public virtual ICollection<PimsManagementFileProperty> PimsManagementFileProperties { get; set; } = new List<PimsManagementFileProperty>();

    [InverseProperty("ManagementFile")]
    public virtual ICollection<PimsManagementFileTeam> PimsManagementFileTeams { get; set; } = new List<PimsManagementFileTeam>();

    [ForeignKey("ProductId")]
    [InverseProperty("PimsManagementFiles")]
    public virtual PimsProduct Product { get; set; }

    [ForeignKey("ProjectId")]
    [InverseProperty("PimsManagementFiles")]
    public virtual PimsProject Project { get; set; }
}
