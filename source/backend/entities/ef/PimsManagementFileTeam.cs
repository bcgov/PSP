using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table contains the relationship between the management file and the associated properties.
/// </summary>
[Table("PIMS_MANAGEMENT_FILE_TEAM")]
[Index("ManagementFileId", Name = "MGMFTM_MANAGEMENT_FILE_ID_IDX")]
[Index("ManagementFileProfileTypeCode", Name = "MGMFTM_MANAGEMENT_FILE_PROFILE_TYPE_CODE_IDX")]
[Index("OrganizationId", Name = "MGMFTM_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "MGMFTM_PERSON_ID_IDX")]
[Index("PrimaryContactId", Name = "MGMFTM_PRIMARY_CONTACT_ID_IDX")]
public partial class PimsManagementFileTeam
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("PIMS_MANAGEMENT_FILE_TEAM_ID")]
    public long PimsManagementFileTeamId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_MANAGEMENT_FILE table.
    /// </summary>
    [Column("MANAGEMENT_FILE_ID")]
    public long ManagementFileId { get; set; }

    /// <summary>
    /// Foreign key to the team member (PIMS_PERSON).
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Foreign key to the team member&apos;s organization (PIMS_ORGANIZATION).
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Foreign key to the primary contact for the organization  (PIMS_PERSON).
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_MANAGEMENT_FILE_PROFILE_TYPE table.
    /// </summary>
    [Column("MANAGEMENT_FILE_PROFILE_TYPE_CODE")]
    [StringLength(20)]
    public string ManagementFileProfileTypeCode { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o.
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

    [ForeignKey("ManagementFileId")]
    [InverseProperty("PimsManagementFileTeams")]
    public virtual PimsManagementFile ManagementFile { get; set; }

    [ForeignKey("ManagementFileProfileTypeCode")]
    [InverseProperty("PimsManagementFileTeams")]
    public virtual PimsManagementFileProfileType ManagementFileProfileTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsManagementFileTeams")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsManagementFileTeamPeople")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsManagementFileTeamPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
