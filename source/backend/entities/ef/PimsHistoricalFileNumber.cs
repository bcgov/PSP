﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Table containing the historical file numbers associated with a property.
/// </summary>
[Table("PIMS_HISTORICAL_FILE_NUMBER")]
[Index("DataSourceTypeCode", Name = "HFLNUM_DATA_SOURCE_TYPE_CODE_IDX")]
[Index("HistoricalFileNumber", Name = "HFLNUM_HISTORICAL_FILE_NUMBER_IDX")]
[Index("PropertyId", Name = "HFLNUM_PROPERTY_ID_IDX")]
public partial class PimsHistoricalFileNumber
{
    /// <summary>
    /// Generated surrogate primary key
    /// </summary>
    [Key]
    [Column("HISTORICAL_FILE_NUMBER_ID")]
    public long HistoricalFileNumberId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_PROPERTY table.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Foreign key indicating the source of the data.
    /// </summary>
    [Column("DATA_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string DataSourceTypeCode { get; set; }

    /// <summary>
    /// Foreign key describing the historical file number type.
    /// </summary>
    [Required]
    [Column("HISTORICAL_FILE_NUMBER_TYPE_CODE")]
    [StringLength(20)]
    public string HistoricalFileNumberTypeCode { get; set; }

    /// <summary>
    /// The historical file number value.
    /// </summary>
    [Required]
    [Column("HISTORICAL_FILE_NUMBER")]
    [StringLength(500)]
    public string HistoricalFileNumber { get; set; }

    /// <summary>
    /// Description of the historical file number type that&apos;s not currently listed.
    /// </summary>
    [Column("OTHER_HIST_FILE_NUMBER_TYPE_CODE")]
    [StringLength(200)]
    public string OtherHistFileNumberTypeCode { get; set; }

    /// <summary>
    /// Indicates if the record is disabled.
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

    [ForeignKey("DataSourceTypeCode")]
    [InverseProperty("PimsHistoricalFileNumbers")]
    public virtual PimsDataSourceType DataSourceTypeCodeNavigation { get; set; }

    [ForeignKey("HistoricalFileNumberTypeCode")]
    [InverseProperty("PimsHistoricalFileNumbers")]
    public virtual PimsHistoricalFileNumberType HistoricalFileNumberTypeCodeNavigation { get; set; }

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsHistoricalFileNumbers")]
    public virtual PimsProperty Property { get; set; }
}
