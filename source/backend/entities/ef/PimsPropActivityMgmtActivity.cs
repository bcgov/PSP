﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table contains the many-to-many relationship between the proeprty activity file and the associated property management activity type and subtype.
/// </summary>
[Table("PIMS_PROP_ACTIVITY_MGMT_ACTIVITY")]
[Index("PimsManagementActivityId", Name = "PACMAC_PIMS_MANAGEMENT_ACTIVITY_ID_IDX")]
[Index("PropMgmtActivitySubtypeCode", Name = "PACMAC_PROP_MGMT_ACTIVITY_SUBTYPE_CODE_IDX")]
[Index("PimsManagementActivityId", "PropMgmtActivitySubtypeCode", Name = "PACMAC_UNIQUE_ACTIVITY_TUC", IsUnique = true)]
public partial class PimsPropActivityMgmtActivity
{
    /// <summary>
    /// Generated surrogate primary key.
    /// </summary>
    [Key]
    [Column("PROP_ACTVTY_MGMT_ACTVTY_TYP_ID")]
    public long PropActvtyMgmtActvtyTypId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY_ACTIVITY table.
    /// </summary>
    [Column("PIMS_MANAGEMENT_ACTIVITY_ID")]
    public long PimsManagementActivityId { get; set; }

    /// <summary>
    /// Foreign key to the PROP_MGMT_ACTIVITY_SUBTYPE table.
    /// </summary>
    [Required]
    [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivitySubtypeCode { get; set; }

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

    [ForeignKey("PimsManagementActivityId")]
    [InverseProperty("PimsPropActivityMgmtActivities")]
    public virtual PimsManagementActivity PimsManagementActivity { get; set; }

    [ForeignKey("PropMgmtActivitySubtypeCode")]
    [InverseProperty("PimsPropActivityMgmtActivities")]
    public virtual PimsPropMgmtActivitySubtype PropMgmtActivitySubtypeCodeNavigation { get; set; }
}
