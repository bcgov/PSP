using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_FILE_NUMBER_HIST")]
[Index("FileNumberHistId", "EndDateHist", Name = "PIMS_FILNUM_H_UK", IsUnique = true)]
public partial class PimsFileNumberHist
{
    [Key]
    [Column("_FILE_NUMBER_HIST_ID")]
    public long FileNumberHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("FILE_NUMBER_ID")]
    public long FileNumberId { get; set; }

    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Required]
    [Column("FILE_NUMBER_TYPE_CODE")]
    [StringLength(20)]
    public string FileNumberTypeCode { get; set; }

    [Required]
    [Column("FILE_NUMBER")]
    [StringLength(500)]
    public string FileNumber { get; set; }

    [Column("OTHER_FILE_NUMBER_TYPE")]
    [StringLength(200)]
    public string OtherFileNumberType { get; set; }

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
