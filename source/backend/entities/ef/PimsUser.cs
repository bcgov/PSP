using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Information associated with an identified PIMS system user.
/// </summary>
[Table("PIMS_USER")]
[Index("BusinessIdentifierValue", Name = "USER_BUSINESS_IDENTIFIER_VALUE_IDX")]
[Index("GuidIdentifierValue", Name = "USER_GUID_IDENTIFIER_VALUE_IDX")]
[Index("PersonId", Name = "USER_PERSON_ID_IDX")]
[Index("UserTypeCode", Name = "USER_USER_TYPE_CODE_IDX")]
public partial class PimsUser
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("USER_ID")]
    public long UserId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table.
    /// </summary>
    [Column("PERSON_ID")]
    public long PersonId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_USER_TYPE table.
    /// </summary>
    [Column("USER_TYPE_CODE")]
    [StringLength(20)]
    public string UserTypeCode { get; set; }

    /// <summary>
    /// Accepted identifier of a user (e.g. IDIR)
    /// </summary>
    [Required]
    [Column("BUSINESS_IDENTIFIER_VALUE")]
    [StringLength(30)]
    public string BusinessIdentifierValue { get; set; }

    /// <summary>
    /// Unique GUID associated with the user.
    /// </summary>
    [Column("GUID_IDENTIFIER_VALUE")]
    public Guid? GuidIdentifierValue { get; set; }

    /// <summary>
    /// Role/position assigned to the user.
    /// </summary>
    [Column("POSITION")]
    [StringLength(100)]
    public string Position { get; set; }

    /// <summary>
    /// Notes associated with this user.
    /// </summary>
    [Column("NOTE")]
    [StringLength(1000)]
    public string Note { get; set; }

    /// <summary>
    /// Last date/time the user was logged into PIMS.
    /// </summary>
    [Column("LAST_LOGIN", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    /// <summary>
    /// Identifier of the person that approved the creation of this PIMS user.
    /// </summary>
    [Column("APPROVED_BY_ID")]
    [StringLength(30)]
    public string ApprovedById { get; set; }

    /// <summary>
    /// Date/time that this user was identified as a PIMS user,
    /// </summary>
    [Column("ISSUE_DATE", TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Expiry date/time of this user account.
    /// </summary>
    [Column("EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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

    [ForeignKey("PersonId")]
    [InverseProperty("PimsUsers")]
    public virtual PimsPerson Person { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; } = new List<PimsAccessRequest>();

    [InverseProperty("User")]
    public virtual ICollection<PimsRegionUser> PimsRegionUsers { get; set; } = new List<PimsRegionUser>();

    [InverseProperty("User")]
    public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; } = new List<PimsUserOrganization>();

    [InverseProperty("User")]
    public virtual ICollection<PimsUserRole> PimsUserRoles { get; set; } = new List<PimsUserRole>();

    [ForeignKey("UserTypeCode")]
    [InverseProperty("PimsUsers")]
    public virtual PimsUserType UserTypeCodeNavigation { get; set; }
}
