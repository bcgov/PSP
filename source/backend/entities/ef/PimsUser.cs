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
    [Key]
    [Column("USER_ID")]
    public long UserId { get; set; }

    [Column("PERSON_ID")]
    public long PersonId { get; set; }

    [Column("USER_TYPE_CODE")]
    [StringLength(20)]
    public string UserTypeCode { get; set; }

    /// <summary>
    /// Accepted identifier of a user (e.g. IDIR).
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
    /// Date/time that this user was identified as a PIMS user,.
    /// </summary>
    [Column("ISSUE_DATE", TypeName = "datetime")]
    public DateTime IssueDate { get; set; }

    /// <summary>
    /// Expiry date/time of this user account.
    /// </summary>
    [Column("EXPIRY_DATE", TypeName = "datetime")]
    public DateTime? ExpiryDate { get; set; }

    /// <summary>
    /// Indicates if this user account is disabled.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool? IsDisabled { get; set; }

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
