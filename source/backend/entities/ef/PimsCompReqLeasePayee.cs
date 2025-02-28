﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Desribes the relationship between a lease stakeholder and a compensation requisition.
/// </summary>
[Table("PIMS_COMP_REQ_LEASE_PAYEE")]
[Index("CompensationRequisitionId", Name = "CRLESP_COMPENSATION_REQUISITION_ID_IDX")]
[Index("LeaseLicenseTeamId", Name = "CRLESP_LEASE_LICENSE_TEAM_ID_IDX")]
[Index("LeaseStakeholderId", Name = "CRLESP_LEASE_STAKEHOLDER_ID_IDX")]
public partial class PimsCompReqLeasePayee
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("COMP_REQ_LEASE_PAYEE_ID")]
    public long CompReqLeasePayeeId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_COMPENSATION_REQUISITION table.
    /// </summary>
    [Column("COMPENSATION_REQUISITION_ID")]
    public long CompensationRequisitionId { get; set; }

    /// <summary>
    /// Foreign key to the LEASE_STAKEHOLDER table.
    /// </summary>
    [Column("LEASE_STAKEHOLDER_ID")]
    public long? LeaseStakeholderId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_LEASE_LICENSE_TEAM table.
    /// </summary>
    [Column("LEASE_LICENSE_TEAM_ID")]
    public long? LeaseLicenseTeamId { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the record was created by the user.
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

    [ForeignKey("CompensationRequisitionId")]
    [InverseProperty("PimsCompReqLeasePayees")]
    public virtual PimsCompensationRequisition CompensationRequisition { get; set; }

    [ForeignKey("LeaseLicenseTeamId")]
    [InverseProperty("PimsCompReqLeasePayees")]
    public virtual PimsLeaseLicenseTeam LeaseLicenseTeam { get; set; }

    [ForeignKey("LeaseStakeholderId")]
    [InverseProperty("PimsCompReqLeasePayees")]
    public virtual PimsLeaseStakeholder LeaseStakeholder { get; set; }
}
