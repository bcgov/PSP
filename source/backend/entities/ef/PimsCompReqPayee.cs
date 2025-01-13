using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table to support multiple payees on a compensation requisition.
/// </summary>
[Table("PIMS_COMP_REQ_PAYEE")]
[Index("AcquisitionFileTeamId", Name = "CMPRQP_ACQUISITION_FILE_TEAM_ID_IDX")]
[Index("CompensationRequisitionId", Name = "CMPRQP_COMPENSATION_REQUISITION_ID_IDX")]
[Index("InterestHolderId", Name = "CMPRQP_INTEREST_HOLDER_ID_IDX")]
public partial class PimsCompReqPayee
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("COMP_REQ_PAYEE_ID")]
    public long CompReqPayeeId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_COMPENSATION_REQUISITION table.
    /// </summary>
    [Column("COMPENSATION_REQUISITION_ID")]
    public long? CompensationRequisitionId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_ACQUISITION_OWNER table.
    /// </summary>
    [Column("ACQUISITION_OWNER_ID")]
    public long? AcquisitionOwnerId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_INTEREST_HOLDER table.
    /// </summary>
    [Column("INTEREST_HOLDER_ID")]
    public long? InterestHolderId { get; set; }

    /// <summary>
    /// Foreign key reference to the PIMS_ACQUISITION_FILE_TEAM table.
    /// </summary>
    [Column("ACQUISITION_FILE_TEAM_ID")]
    public long? AcquisitionFileTeamId { get; set; }

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

    [ForeignKey("AcquisitionFileTeamId")]
    [InverseProperty("PimsCompReqPayees")]
    public virtual PimsAcquisitionFileTeam AcquisitionFileTeam { get; set; }

    [ForeignKey("AcquisitionOwnerId")]
    [InverseProperty("PimsCompReqPayees")]
    public virtual PimsAcquisitionOwner AcquisitionOwner { get; set; }

    [ForeignKey("CompensationRequisitionId")]
    [InverseProperty("PimsCompReqPayees")]
    public virtual PimsCompensationRequisition CompensationRequisition { get; set; }

    [ForeignKey("InterestHolderId")]
    [InverseProperty("PimsCompReqPayees")]
    public virtual PimsInterestHolder InterestHolder { get; set; }
}
