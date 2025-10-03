using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_SECURITY_DEPOSIT_HOLDER")]
[Index("OrganizationId", Name = "SCDPHL_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "SCDPHL_PERSON_ID_IDX")]
[Index("SecurityDepositId", Name = "SCDPHL_SECURITY_DEPOSIT_ID_IDX")]
[Index("SecurityDepositId", Name = "SCDPHL_SECURITY_DEPOSIT_ID_TUC", IsUnique = true)]
public partial class PimsSecurityDepositHolder
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("SECURITY_DEPOSIT_HOLDER_ID")]
    public long SecurityDepositHolderId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_SECURITY_DEPOSIT table.
    /// </summary>
    [Column("SECURITY_DEPOSIT_ID")]
    public long SecurityDepositId { get; set; }

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

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsSecurityDepositHolders")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsSecurityDepositHolders")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("SecurityDepositId")]
    [InverseProperty("PimsSecurityDepositHolder")]
    public virtual PimsSecurityDeposit SecurityDeposit { get; set; }
}
