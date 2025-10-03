using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the details of the return of a security deposit.
/// </summary>
[Table("PIMS_SECURITY_DEPOSIT_RETURN")]
[Index("SecurityDepositId", Name = "SDRTRN_SECURITY_DEPOSIT_ID_IDX")]
public partial class PimsSecurityDepositReturn
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("SECURITY_DEPOSIT_RETURN_ID")]
    public long SecurityDepositReturnId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_SECURITY_DEPOSIT table.
    /// </summary>
    [Column("SECURITY_DEPOSIT_ID")]
    public long SecurityDepositId { get; set; }

    /// <summary>
    /// Date the lease/license was terminated or surrendered
    /// </summary>
    [Column("TERMINATION_DATE", TypeName = "datetime")]
    public DateTime TerminationDate { get; set; }

    /// <summary>
    /// Amount of claims against the deposit
    /// </summary>
    [Column("CLAIMS_AGAINST", TypeName = "money")]
    public decimal? ClaimsAgainst { get; set; }

    /// <summary>
    /// Amount returned minus claims
    /// </summary>
    [Column("RETURN_AMOUNT", TypeName = "money")]
    public decimal ReturnAmount { get; set; }

    /// <summary>
    /// Date of deposit return
    /// </summary>
    [Column("RETURN_DATE", TypeName = "datetime")]
    public DateTime ReturnDate { get; set; }

    /// <summary>
    /// Interest paid on the deposit to the deposit holder
    /// </summary>
    [Column("INTEREST_PAID", TypeName = "money")]
    public decimal? InterestPaid { get; set; }

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

    [InverseProperty("SecurityDepositReturn")]
    public virtual PimsSecurityDepositReturnHolder PimsSecurityDepositReturnHolder { get; set; }

    [ForeignKey("SecurityDepositId")]
    [InverseProperty("PimsSecurityDepositReturns")]
    public virtual PimsSecurityDeposit SecurityDeposit { get; set; }
}
