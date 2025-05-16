using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Common table that contains the codes and associated descriptions of the various checklist item status types.
/// </summary>
[Table("PIMS_CHKLST_ITEM_STATUS_TYPE")]
public partial class PimsChklstItemStatusType
{
    /// <summary>
    /// Codified version of the various checklist item status types.
    /// </summary>
    [Key]
    [Column("CHKLST_ITEM_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ChklstItemStatusTypeCode { get; set; }

    /// <summary>
    /// Description of the various checklist item status type.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Display order of the codes.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the code value is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

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

    [InverseProperty("ChklstItemStatusTypeCodeNavigation")]
    public virtual ICollection<PimsAcquisitionChecklistItem> PimsAcquisitionChecklistItems { get; set; } = new List<PimsAcquisitionChecklistItem>();

    [InverseProperty("ChklstItemStatusTypeCodeNavigation")]
    public virtual ICollection<PimsDispositionChecklistItem> PimsDispositionChecklistItems { get; set; } = new List<PimsDispositionChecklistItem>();

    [InverseProperty("ChklstItemStatusTypeCodeNavigation")]
    public virtual ICollection<PimsLeaseChecklistItem> PimsLeaseChecklistItems { get; set; } = new List<PimsLeaseChecklistItem>();
}
