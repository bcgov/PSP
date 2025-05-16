using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_HISTORICAL_FILE_NUMBER_HIST")]
[Index("HistoricalFileNumberHistId", "EndDateHist", Name = "PIMS_HFLNUM_H_UK", IsUnique = true)]
public partial class PimsHistoricalFileNumberHist
{
    [Key]
    [Column("_HISTORICAL_FILE_NUMBER_HIST_ID")]
    public long HistoricalFileNumberHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("HISTORICAL_FILE_NUMBER_ID")]
    public long HistoricalFileNumberId { get; set; }

    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Column("DATA_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string DataSourceTypeCode { get; set; }

    [Required]
    [Column("HISTORICAL_FILE_NUMBER_TYPE_CODE")]
    [StringLength(20)]
    public string HistoricalFileNumberTypeCode { get; set; }

    [Required]
    [Column("HISTORICAL_FILE_NUMBER")]
    [StringLength(500)]
    public string HistoricalFileNumber { get; set; }

    [Column("OTHER_HIST_FILE_NUMBER_TYPE_CODE")]
    [StringLength(200)]
    public string OtherHistFileNumberTypeCode { get; set; }

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
}
