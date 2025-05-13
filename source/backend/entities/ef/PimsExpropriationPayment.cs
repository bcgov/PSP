using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity continaing the details regarding a Form 8 (Notice of Advance Payment).
/// </summary>
[Table("PIMS_EXPROPRIATION_PAYMENT")]
[Index("AcquisitionFileId", Name = "EXPPMT_ACQUISITION_FILE_ID_IDX")]
[Index("AcquisitionOwnerId", Name = "EXPPMT_ACQUISITION_OWNER_ID_IDX")]
[Index("ExpropriatingAuthority", Name = "EXPPMT_EXPROPRIATING_AUTHORITY_IDX")]
[Index("InterestHolderId", Name = "EXPPMT_INTEREST_HOLDER_ID_IDX")]
public partial class PimsExpropriationPayment
{
    /// <summary>
    /// Unique auto-generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("EXPROPRIATION_PAYMENT_ID")]
    public long ExpropriationPaymentId { get; set; }

    /// <summary>
    /// Foreign key of the acquisition file.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key of the acquisition owner.
    /// </summary>
    [Column("ACQUISITION_OWNER_ID")]
    public long? AcquisitionOwnerId { get; set; }

    /// <summary>
    /// Foreign key of the acquisition interest holder.
    /// </summary>
    [Column("INTEREST_HOLDER_ID")]
    public long? InterestHolderId { get; set; }

    /// <summary>
    /// Foreign key of the expropriating authoritry.
    /// </summary>
    [Column("EXPROPRIATING_AUTHORITY")]
    public long? ExpropriatingAuthority { get; set; }

    /// <summary>
    /// Form 8 description field.  There are lawyer remarks pending.  This field could be used for: - providing remarks particular to an expropriation form, and /or - for any ETL descriptive fields as well as - a place-holder forfields that do not have a mapping.
    /// </summary>
    [Column("DESCRIPTION")]
    [StringLength(2000)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the Form 8 payment is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o.
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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsExpropriationPayments")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("AcquisitionOwnerId")]
    [InverseProperty("PimsExpropriationPayments")]
    public virtual PimsAcquisitionOwner AcquisitionOwner { get; set; }

    [ForeignKey("ExpropriatingAuthority")]
    [InverseProperty("PimsExpropriationPayments")]
    public virtual PimsOrganization ExpropriatingAuthorityNavigation { get; set; }

    [ForeignKey("InterestHolderId")]
    [InverseProperty("PimsExpropriationPayments")]
    public virtual PimsInterestHolder InterestHolder { get; set; }

    [InverseProperty("ExpropriationPayment")]
    public virtual ICollection<PimsExpropPmtPmtItem> PimsExpropPmtPmtItems { get; set; } = new List<PimsExpropPmtPmtItem>();
}
