using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table that contains the lease &amp; license checklist items.
/// </summary>
[Table("PIMS_LEASE_CHECKLIST_ITEM")]
[Index("ChklstItemStatusTypeCode", Name = "LCHKLI_LEASE_CHKLST_ITEM_STATUS_TYPE_CODE_IDX")]
[Index("LeaseChklstItemTypeCode", Name = "LCHKLI_LEASE_CHKLST_ITEM_TYPE_CODE_IDX")]
[Index("LeaseId", Name = "LCHKLI_LEASE_ID_IDX")]
[Index("LeaseId", "LeaseChklstItemTypeCode", Name = "LCHKLI_LEASE_ID_UK", IsUnique = true)]
public partial class PimsLeaseChecklistItem
{
    /// <summary>
    /// Generated surrogate primary key
    /// </summary>
    [Key]
    [Column("LEASE_CHECKLIST_ITEM_ID")]
    public long LeaseChecklistItemId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE table.
    /// </summary>
    [Column("LEASE_ID")]
    public long LeaseId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE_CHKLST_ITEM_TYPE table.
    /// </summary>
    [Required]
    [Column("LEASE_CHKLST_ITEM_TYPE_CODE")]
    [StringLength(20)]
    public string LeaseChklstItemTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_CHKLST_ITEM_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("CHKLST_ITEM_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ChklstItemStatusTypeCode { get; set; }

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

    [ForeignKey("ChklstItemStatusTypeCode")]
    [InverseProperty("PimsLeaseChecklistItems")]
    public virtual PimsChklstItemStatusType ChklstItemStatusTypeCodeNavigation { get; set; }

    [ForeignKey("LeaseId")]
    [InverseProperty("PimsLeaseChecklistItems")]
    public virtual PimsLease Lease { get; set; }

    [ForeignKey("LeaseChklstItemTypeCode")]
    [InverseProperty("PimsLeaseChecklistItems")]
    public virtual PimsLeaseChklstItemType LeaseChklstItemTypeCodeNavigation { get; set; }
}
