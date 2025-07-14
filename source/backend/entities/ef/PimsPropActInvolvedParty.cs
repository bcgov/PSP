using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Associates a property management activity to a vendor (many-to-many).
/// </summary>
[Table("PIMS_PROP_ACT_INVOLVED_PARTY")]
[Index("OrganizationId", Name = "PAINVP_ORGANIZATION_ID_IDX")]
[Index("PersonId", Name = "PAINVP_PERSON_ID_IDX")]
[Index("PimsManagementActivityId", Name = "PAINVP_PIMS_PROPERTY_ACTIVITY_ID_IDX")]
public partial class PimsPropActInvolvedParty
{
    [Key]
    [Column("PROP_ACT_INVOLVED_PARTY_ID")]
    public long PropActInvolvedPartyId { get; set; }

    [Column("PIMS_MANAGEMENT_ACTIVITY_ID")]
    public long PimsManagementActivityId { get; set; }

    [Column("PERSON_ID")]
    public long? PersonId { get; set; }

    [Column("ORGANIZATION_ID")]
    public long? OrganizationId { get; set; }

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

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsPropActInvolvedParties")]
    public virtual PimsOrganization Organization { get; set; }

    [ForeignKey("PersonId")]
    [InverseProperty("PimsPropActInvolvedParties")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PimsManagementActivityId")]
    [InverseProperty("PimsPropActInvolvedParties")]
    public virtual PimsManagementActivity PimsManagementActivity { get; set; }
}
