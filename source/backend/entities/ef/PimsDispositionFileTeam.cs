using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table to associate an acquisition file to a person.
/// </summary>
[Table("PIMS_DISPOSITION_FILE_TEAM")]
[Index("DispositionFileId", Name = "DSPFTM_DISPOSITION_FILE_ID_IDX")]
[Index("DispositionFileId", "DspFlTeamProfileTypeCode", Name = "DSPFTM_DSP_FILE_PROFILE_TUC", IsUnique = true)]
[Index("DspFlTeamProfileTypeCode", Name = "DSPFTM_DSP_FL_TEAM_PROFILE_TYPE_CODE_IDX")]
[Index("OrganizationId", Name = "DSPFTM_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "DSPFTM_PERSON_ID_IDX")]
[Index("PrimaryContactId", Name = "DSPFTM_PRIMARY_CONTACT_ID_IDX")]
public partial class PimsDispositionFileTeam
{
    /// <summary>
    /// Unique auto-generated surrogate primary key
    /// </summary>
    [Key]
    [Column("DISPOSITION_FILE_TEAM_ID")]
    public long DispositionFileTeamId { get; set; }

    /// <summary>
    /// Foreign key value for the dispostion file
    /// </summary>
    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    /// <summary>
    /// Foreign key value for the person.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Foreign key value for the organization.
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Foreign key value for the primary contact person.
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Code value for the disposition file profile type.
    /// </summary>
    [Required]
    [Column("DSP_FL_TEAM_PROFILE_TYPE_CODE")]
    [StringLength(20)]
    public string DspFlTeamProfileTypeCode { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update.  If this is done then the update will succeed, provided that the row was not updated by any
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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

    [ForeignKey("DispositionFileId")]
    [InverseProperty("PimsDispositionFileTeams")]
    public virtual PimsDispositionFile DispositionFile { get; set; }

    [ForeignKey("DspFlTeamProfileTypeCode")]
    [InverseProperty("PimsDispositionFileTeams")]
    public virtual PimsDspFlTeamProfileType DspFlTeamProfileTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsDispositionFileTeams")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsDispositionFileTeamPeople")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsDispositionFileTeamPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
