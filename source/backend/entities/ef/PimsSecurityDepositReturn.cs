﻿using System;
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
    [Key]
    [Column("SECURITY_DEPOSIT_RETURN_ID")]
    public long SecurityDepositReturnId { get; set; }

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

    [InverseProperty("SecurityDepositReturn")]
    public virtual PimsSecurityDepositReturnHolder PimsSecurityDepositReturnHolder { get; set; }

    [ForeignKey("SecurityDepositId")]
    [InverseProperty("PimsSecurityDepositReturns")]
    public virtual PimsSecurityDeposit SecurityDeposit { get; set; }
}
