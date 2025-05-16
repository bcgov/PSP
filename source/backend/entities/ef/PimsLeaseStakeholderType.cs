using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Code table describing the type of tenant on a lease.
/// </summary>
[Table("PIMS_LEASE_STAKEHOLDER_TYPE")]
public partial class PimsLeaseStakeholderType
{
    /// <summary>
    /// Code representing the types of stakeholders on a lease.
    /// </summary>
    [Key]
    [Column("LEASE_STAKEHOLDER_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseStakeholderTypeCode { get; set; }

    /// <summary>
    /// Description of the types of stakeholders on a lease.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code is related to payable leases.
    /// </summary>
    [Column("IS_PAYABLE_RELATED")]
    public bool? IsPayableRelated { get; set; }

    /// <summary>
    /// Indicates if the code is currently active.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Specifies a specific order to visually present the code.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("LeaseStakeholderTypeCodeNavigation")]
    public virtual ICollection<PimsLeaseStakeholder> PimsLeaseStakeholders { get; set; } = new List<PimsLeaseStakeholder>();
}
