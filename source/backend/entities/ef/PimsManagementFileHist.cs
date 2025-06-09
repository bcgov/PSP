using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_MANAGEMENT_FILE_HIST")]
[Index("ManagementFileHistId", "EndDateHist", Name = "PIMS_MGMTFL_H_UK", IsUnique = true)]
public partial class PimsManagementFileHist
{
    [Key]
    [Column("_MANAGEMENT_FILE_HIST_ID")]
    public long ManagementFileHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("MANAGEMENT_FILE_ID")]
    public long ManagementFileId { get; set; }

    [Column("PROJECT_ID")]
    public long? ProjectId { get; set; }

    [Column("PRODUCT_ID")]
    public long? ProductId { get; set; }

    [Column("ACQUISITION_FUNDING_TYPE_CODE")]
    [StringLength(20)]
    public string AcquisitionFundingTypeCode { get; set; }

    [Required]
    [Column("MANAGEMENT_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string ManagementFileStatusTypeCode { get; set; }

    [Required]
    [Column("MANAGEMENT_FILE_PURPOSE_TYPE_CODE")]
    [StringLength(20)]
    public string ManagementFilePurposeTypeCode { get; set; }

    [Required]
    [Column("FILE_NAME")]
    [StringLength(500)]
    public string FileName { get; set; }

    [Column("LEGACY_FILE_NUM")]
    [StringLength(100)]
    public string LegacyFileNum { get; set; }

    [Column("FILE_PURPOSE")]
    [StringLength(2000)]
    public string FilePurpose { get; set; }

    [Column("ADDITIONAL_DETAILS")]
    [StringLength(2000)]
    public string AdditionalDetails { get; set; }

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
