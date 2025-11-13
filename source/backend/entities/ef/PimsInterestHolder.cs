using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Documents the interest holders that have an stake in the acquisition.
/// </summary>
[Table("PIMS_INTEREST_HOLDER")]
[Index("AcquisitionFileId", Name = "INTHLD_ACQUISITION_FILE_ID_IDX")]
[Index("InterestHolderTypeCode", Name = "INTHLD_INTEREST_HOLDER_TYPE_CODE_IDX")]
[Index("OrganizationId", Name = "INTHLD_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "INTHLD_PERSON_ID_IDX")]
[Index("PrimaryContactId", Name = "INTHLD_PRIMARY_CONTACT_ID_IDX")]
public partial class PimsInterestHolder
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("INTEREST_HOLDER_ID")]
    public long InterestHolderId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FILE table.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORGANIZATION table.
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.  Primary contact for the organization
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_INTEREST_HOLDER_TYPE table.
    /// </summary>
    [Required]
    [Column("INTEREST_HOLDER_TYPE_CODE")]
    [StringLength(20)]
    public string InterestHolderTypeCode { get; set; }

    /// <summary>
    /// Additional comment concerning the owener representative.
    /// </summary>
    [Column("COMMENT")]
    [StringLength(2000)]
    public string Comment { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsInterestHolders")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("InterestHolderTypeCode")]
    [InverseProperty("PimsInterestHolders")]
    public virtual PimsInterestHolderType InterestHolderTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsInterestHolders")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsInterestHolderPeople")]
    public virtual PimsPerson Person { get; set; }

    [InverseProperty("InterestHolder")]
    public virtual ICollection<PimsCompReqAcqPayee> PimsCompReqAcqPayees { get; set; } = new List<PimsCompReqAcqPayee>();

    [InverseProperty("InterestHolder")]
    public virtual ICollection<PimsExpropOwnerHistory> PimsExpropOwnerHistories { get; set; } = new List<PimsExpropOwnerHistory>();

    [InverseProperty("InterestHolder")]
    public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; } = new List<PimsExpropriationPayment>();

    [InverseProperty("InterestHolder")]
    public virtual ICollection<PimsInthldrPropInterest> PimsInthldrPropInterests { get; set; } = new List<PimsInthldrPropInterest>();

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsInterestHolderPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
