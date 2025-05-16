using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Associates a property management activity to a Ministry contact (many-to-many).
/// </summary>
[Table("PIMS_PROP_ACT_MIN_CONTACT")]
[Index("PersonId", Name = "PRACMC_PERSON_ID_IDX")]
[Index("PimsPropertyActivityId", Name = "PRACMC_PIMS_PROPERTY_ACTIVITY_ID_IDX")]
public partial class PimsPropActMinContact
{
    [Key]
    [Column("PROP_ACT_MIN_CONTACT_ID")]
    public long PropActMinContactId { get; set; }

    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long PimsPropertyActivityId { get; set; }

    [Column("PERSON_ID")]
    public long PersonId { get; set; }

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
    [InverseProperty("PimsPropActMinContacts")]
    public virtual PimsPerson Person { get; set; }

    [ForeignKey("PimsPropertyActivityId")]
    [InverseProperty("PimsPropActMinContacts")]
    public virtual PimsPropertyActivity PimsPropertyActivity { get; set; }
}
