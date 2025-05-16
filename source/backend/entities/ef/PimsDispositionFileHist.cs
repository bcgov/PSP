using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_DISPOSITION_FILE_HIST")]
[Index("DispositionFileHistId", "EndDateHist", Name = "PIMS_DISPFL_H_UK", IsUnique = true)]
public partial class PimsDispositionFileHist
{
    [Key]
    [Column("_DISPOSITION_FILE_HIST_ID")]
    public long DispositionFileHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("DISPOSITION_FILE_ID")]
    public long DispositionFileId { get; set; }

    [Required]
    [Column("DISPOSITION_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionStatusTypeCode { get; set; }

    [Required]
    [Column("DISPOSITION_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionFileStatusTypeCode { get; set; }

    [Required]
    [Column("DISPOSITION_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionTypeCode { get; set; }

    [Column("DISPOSITION_FUNDING_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionFundingTypeCode { get; set; }

    [Column("DISPOSITION_INITIATING_DOC_TYPE_CODE")]
    [StringLength(20)]
    public string DispositionInitiatingDocTypeCode { get; set; }

    [Column("DSP_PHYS_FILE_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string DspPhysFileStatusTypeCode { get; set; }

    [Column("DSP_INITIATING_BRANCH_TYPE_CODE")]
    [StringLength(20)]
    public string DspInitiatingBranchTypeCode { get; set; }

    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    [Column("PROJECT_ID")]
    public long? ProjectId { get; set; }

    [Column("PRODUCT_ID")]
    public long? ProductId { get; set; }

    [Column("FILE_NUMBER")]
    [StringLength(20)]
    public string FileNumber { get; set; }

    [Column("FILE_NAME")]
    [StringLength(200)]
    public string FileName { get; set; }

    [Column("FILE_REFERENCE")]
    [StringLength(200)]
    public string FileReference { get; set; }

    [Column("OTHER_DISPOSITION_TYPE")]
    [StringLength(200)]
    public string OtherDispositionType { get; set; }

    [Column("OTHER_INITIATING_DOC_TYPE")]
    [StringLength(200)]
    public string OtherInitiatingDocType { get; set; }

    [Column("ASSIGNED_DT")]
    public DateOnly? AssignedDt { get; set; }

    [Column("COMPLETED_DT")]
    public DateOnly? CompletedDt { get; set; }

    [Column("INITIATING_DOCUMENT_DT")]
    public DateOnly? InitiatingDocumentDt { get; set; }

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
