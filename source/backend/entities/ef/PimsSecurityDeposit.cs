using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Description of a security deposit associated with a lease.
/// </summary>
[Table("PIMS_SECURITY_DEPOSIT")]
[Index("LeaseId", Name = "SECDEP_LEASE_ID_IDX")]
[Index("SecurityDepositTypeCode", Name = "SECDEP_SECURITY_DEPOSIT_TYPE_CODE_IDX")]
public partial class PimsSecurityDeposit
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("SECURITY_DEPOSIT_ID")]
    public long SecurityDepositId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_SECURITY_DEPOSIT_TYPE table.
    /// </summary>
    [Required]
    [Column("SECURITY_DEPOSIT_TYPE_CODE")]
    [StringLength(20)]
    public string SecurityDepositTypeCode { get; set; }

    /// <summary>
    /// Description of the deposit type If the SECURITY_DEPOSIT_TYPE_CODE has been chosen for this scurity deposit.
    /// </summary>
    [Column("OTHER_DEPOSIT_TYPE_DESC")]
    [StringLength(200)]
    public string OtherDepositTypeDesc { get; set; }

    /// <summary>
    /// Descirption of this security deposit
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(2000)]
    public string Description { get; set; }

    /// <summary>
    /// Amount paid of this security deposit
    /// </summary>
    [Column("AMOUNT_PAID", TypeName = "money")]
    public decimal AmountPaid { get; set; }

    /// <summary>
    /// Date of this security deposit
    /// </summary>
    [Column("DEPOSIT_DATE")]
    public DateOnly DepositDate { get; set; }

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

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsSecurityDeposits")]
    public virtual PimsLease Lease { get; set; }

    [InverseProperty("SecurityDeposit")]
    public virtual PimsSecurityDepositHolder PimsSecurityDepositHolder { get; set; }

    [InverseProperty("SecurityDeposit")]
    public virtual ICollection<PimsSecurityDepositReturn> PimsSecurityDepositReturns { get; set; } = new List<PimsSecurityDepositReturn>();

    [ForeignKey("SecurityDepositTypeCode")]
    [InverseProperty("PimsSecurityDeposits")]
    public virtual PimsSecurityDepositType SecurityDepositTypeCodeNavigation { get; set; }
}
