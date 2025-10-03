using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_ROLE")]
[Index("KeycloakGroupId", Name = "ROLE_KEYCLOAK_GROUP_ID_IDX")]
[Index("RoleUid", Name = "ROLE_ROLE_UID_IDX")]
public partial class PimsRole
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("ROLE_ID")]
    public long RoleId { get; set; }

    /// <summary>
    /// Unique identifier assigned to the role.
    /// </summary>
    [Column("ROLE_UID")]
    public Guid RoleUid { get; set; }

    /// <summary>
    /// Unique identifier assigned to the keycloak group.
    /// </summary>
    [Column("KEYCLOAK_GROUP_ID")]
    public Guid? KeycloakGroupId { get; set; }

    /// <summary>
    /// Name assigned to the role.
    /// </summary>
    [Required]
    [Column("NAME")]
    [StringLength(100)]
    public string Name { get; set; }

    /// <summary>
    /// Description of the role.
    /// </summary>
    [Column("DESCRIPTION")]
    [StringLength(500)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates that the role is public.
    /// </summary>
    [Column("IS_PUBLIC")]
    public bool IsPublic { get; set; }

    /// <summary>
    /// Indicates if the record is disabled and therefore not selectable or displayed.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the role.
    /// </summary>
    [Column("SORT_ORDER")]
    public int SortOrder { get; set; }

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

    [InverseProperty("Role")]
    public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; } = new List<PimsAccessRequest>();

    [InverseProperty("Role")]
    public virtual ICollection<PimsRoleClaim> PimsRoleClaims { get; set; } = new List<PimsRoleClaim>();

    [InverseProperty("Role")]
    public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; } = new List<PimsUserOrganization>();

    [InverseProperty("Role")]
    public virtual ICollection<PimsUserRole> PimsUserRoles { get; set; } = new List<PimsUserRole>();
}
