using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Entity containing information regarding the owner of an acquisition file.
/// </summary>
[Table("PIMS_ACQUISITION_OWNER")]
[Index("AcquisitionFileId", Name = "ACQOWN_ACQUISITION_FILE_ID_IDX")]
[Index("AddressId", Name = "ACQOWN_ADDRESS_ID_IDX")]
[Index("LastNameAndCorpName", Name = "ACQOWN_LAST_NAME_OR_CORP_NAME_1_IDX")]
public partial class PimsAcquisitionOwner
{
    [Key]
    [Column("ACQUISITION_OWNER_ID")]
    public long AcquisitionOwnerId { get; set; }

    [Column("ACQUISITION_FILE_ID")]
    public long? AcquisitionFileId { get; set; }

    [Column("ADDRESS_ID")]
    public long? AddressId { get; set; }

    /// <summary>
    /// Indicates that this is the file&apos;s primary owner.
    /// </summary>
    [Column("IS_PRIMARY_OWNER")]
    public bool IsPrimaryOwner { get; set; }

    /// <summary>
    /// Indicates if the owner is an organization.  Default value is FALSE, indicating that the owner is a person.
    /// </summary>
    [Column("IS_ORGANIZATION")]
    public bool IsOrganization { get; set; }

    /// <summary>
    /// Name of the owner (person or organization).  If person, surname.
    /// </summary>
    [Column("LAST_NAME_AND_CORP_NAME")]
    [StringLength(300)]
    public string LastNameAndCorpName { get; set; }

    /// <summary>
    /// Optional name field if required.
    /// </summary>
    [Column("OTHER_NAME")]
    [StringLength(300)]
    public string OtherName { get; set; }

    /// <summary>
    /// Given name of the owner (person).
    /// </summary>
    [Column("GIVEN_NAME")]
    [StringLength(300)]
    public string GivenName { get; set; }

    /// <summary>
    /// Incorporation number of the organization.
    /// </summary>
    [Column("INCORPORATION_NUMBER")]
    [StringLength(50)]
    public string IncorporationNumber { get; set; }

    /// <summary>
    /// Registration number of the organization.
    /// </summary>
    [Column("REGISTRATION_NUMBER")]
    [StringLength(50)]
    public string RegistrationNumber { get; set; }

    /// <summary>
    /// Email address to be used for contacting the owner.
    /// </summary>
    [Column("CONTACT_EMAIL_ADDR")]
    [StringLength(250)]
    public string ContactEmailAddr { get; set; }

    /// <summary>
    /// Phone number to be used for contacting the owner.
    /// </summary>
    [Column("CONTACT_PHONE_NUM")]
    [StringLength(20)]
    public string ContactPhoneNum { get; set; }

    /// <summary>
    /// Date the owner record became effective. Defaults to current date/time.
    /// </summary>
    [Column("EFFECTIVE_DATE", TypeName = "datetime")]
    public DateTime? EffectiveDate { get; set; }

    /// <summary>
    /// Date the owner record expired.
    /// </summary>
    [Column("EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

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

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsAcquisitionOwners")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("AddressId")]
    [InverseProperty("PimsAcquisitionOwners")]
    public virtual PimsAddress Address { get; set; }

    [InverseProperty("AcquisitionOwner")]
    public virtual ICollection<PimsCompReqPayee> PimsCompReqPayees { get; set; } = new List<PimsCompReqPayee>();

    [InverseProperty("AcquisitionOwner")]
    public virtual ICollection<PimsExpropriationPayment> PimsExpropriationPayments { get; set; } = new List<PimsExpropriationPayment>();
}
