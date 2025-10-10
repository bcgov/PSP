using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table to associate an acquisition file to a person.
/// </summary>
[Table("PIMS_ACQUISITION_FILE_TEAM")]
[Index("AcquisitionFileId", "AcqFlTeamProfileTypeCode", Name = "ACQNTM_FILE_PROFILE_TUC", IsUnique = true)]
[Index("OrganizationId", Name = "ACQNTM_ORGANIZATION_ID_IDX")]
[Index("PrimaryContactId", Name = "ACQNTM_PRIMARY_CONTACT_ID_IDX")]
[Index("AcquisitionFileId", Name = "ACQPER_ACQUISITION_FILE_ID_IDX")]
[Index("AcqFlTeamProfileTypeCode", Name = "ACQPER_ACQ_FL_PERSON_PROFILE_TYPE_CODE_IDX")]
[Index("PersonId", Name = "ACQPER_PERSON_ID_IDX")]
public partial class PimsAcquisitionFileTeam
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("ACQUISITION_FILE_TEAM_ID")]
    public long AcquisitionFileTeamId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQUISITION_FILE table.
    /// </summary>
    [Column("ACQUISITION_FILE_ID")]
    public long AcquisitionFileId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PERSON table for an associated person.
    /// </summary>
    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORGANIZATION table for the organization
    /// </summary>
    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ORGANIZATION table for the organization&apos;s primary contact.
    /// </summary>
    [Column("PRIMARY_CONTACT_ID")]
    public long? PrimaryContactId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACQ_FL_TEAM_PROFILE_TYPE table.
    /// </summary>
    [Required]
    [Column("ACQ_FL_TEAM_PROFILE_TYPE_CODE")]
    [StringLength(20)]
    public string AcqFlTeamProfileTypeCode { get; set; }

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

    [ForeignKey("AcqFlTeamProfileTypeCode")]
    [InverseProperty("PimsAcquisitionFileTeams")]
    public virtual PimsAcqFlTeamProfileType AcqFlTeamProfileTypeCodeNavigation { get; set; }

    [ForeignKey("AcquisitionFileId")]
    [InverseProperty("PimsAcquisitionFileTeams")]
    public virtual PimsAcquisitionFile AcquisitionFile { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsAcquisitionFileTeams")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsAcquisitionFileTeamPeople")]
    public virtual PimsPerson Person { get; set; }

    [InverseProperty("AcquisitionFileTeam")]
    public virtual ICollection<PimsCompReqAcqPayee> PimsCompReqAcqPayees { get; set; } = new List<PimsCompReqAcqPayee>();

    [ForeignKey("PrimaryContactId")]
    [InverseProperty("PimsAcquisitionFileTeamPrimaryContacts")]
    public virtual PimsPerson PrimaryContact { get; set; }
}
