using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Associates a property to a property management actity (many-to-many).
/// </summary>
[Table("PIMS_MANAGEMENT_ACTIVITY_PROPERTY")]
[Index("PimsManagementActivityId", Name = "PRPRAC_PIMS_PROPERTY_ACTIVITY_ID_IDX")]
[Index("PropertyId", Name = "PRPRAC_PROPERTY_ID_IDX")]
public partial class PimsManagementActivityProperty
{
    [Key]
    [Column("MANAGEMENT_ACTIVITY_PROPERTY_ID")]
    public long ManagementActivityPropertyId { get; set; }

    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("PIMS_MANAGEMENT_ACTIVITY_ID")]
    public long PimsManagementActivityId { get; set; }

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

    [ForeignKey("PimsManagementActivityId")]
    [InverseProperty("PimsManagementActivityProperties")]
    public virtual PimsManagementActivity PimsManagementActivity { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsManagementActivityProperties")]
    public virtual PimsProperty Property { get; set; }
}
