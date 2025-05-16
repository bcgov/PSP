using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DISPOSITION_APPRAISAL_HIST")]
[Index("DispositionAppraisalHistId", "EndDateHist", Name = "PIMS_DSPAPP_H_UK", IsUnique = true)]
public partial class PimsDispositionAppraisalHist
{
    [Key]
    [Column("_DISPOSITION_APPRAISAL_HIST_ID")]
    public long DispositionAppraisalHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("DISPOSITION_APPRAISAL_ID")]
    public long DispositionAppraisalId { get; set; }

    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    [Column("APPRAISED_AMT", TypeName = "money")]
    public decimal? AppraisedAmt { get; set; }

    [Column("APPRAISAL_DT")]
    public DateOnly? AppraisalDt { get; set; }

    [Column("BCA_VALUE_AMT", TypeName = "money")]
    public decimal? BcaValueAmt { get; set; }

    [Column("BCA_ROLL_YEAR")]
    public short? BcaRollYear { get; set; }

    [Column("LIST_PRICE_AMT", TypeName = "money")]
    public decimal? ListPriceAmt { get; set; }

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
