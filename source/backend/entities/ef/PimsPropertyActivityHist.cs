using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_PROPERTY_ACTIVITY_HIST")]
[Index("PropertyActivityHistId", "EndDateHist", Name = "PIMS_PRPACT_H_UK", IsUnique = true)]
public partial class PimsPropertyActivityHist
{
    [Key]
    [Column("_PROPERTY_ACTIVITY_HIST_ID")]
    public long PropertyActivityHistId { get; set; }

    [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
    public DateTime EffectiveDateHist { get; set; }

    [Column("END_DATE_HIST", TypeName = "datetime")]
    public DateTime? EndDateHist { get; set; }

    [Column("PIMS_PROPERTY_ACTIVITY_ID")]
    public long PimsPropertyActivityId { get; set; }

    [Required]
    [Column("PROP_MGMT_ACTIVITY_TYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivityTypeCode { get; set; }

    [Required]
    [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivitySubtypeCode { get; set; }

    [Required]
    [Column("PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string PropMgmtActivityStatusTypeCode { get; set; }

    [Column("SERVICE_PROVIDER_PERSON_ID")]
    public long? ServiceProviderPersonId { get; set; }

    [Column("SERVICE_PROVIDER_ORG_ID")]
    public long? ServiceProviderOrgId { get; set; }

    [Column("REQUEST_ADDED_DT")]
    public DateOnly RequestAddedDt { get; set; }

    [Column("COMPLETION_DT")]
    public DateOnly? CompletionDt { get; set; }

    [Column("DESCRIPTION")]
    [StringLength(500)]
    public string Description { get; set; }

    [Column("REQUEST_SOURCE")]
    [StringLength(2000)]
    public string RequestSource { get; set; }

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
