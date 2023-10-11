using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_PROPERTY_ACTIVITY_HIST")]
    [Index(nameof(PropertyActivityHistId), nameof(EndDateHist), Name = "PIMS_PRPACT_H_UK", IsUnique = true)]
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
        [Column("PROPERTY_ID")]
        public long PropertyId { get; set; }
        [Required]
        [Column("PROP_MGMT_ACTIVITY_TYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivityTypeCode { get; set; }
        [Required]
        [Column("PROP_MGMT_ACTIVITY_SUBTYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivitySubtypeCode { get; set; }
        [Column("PROP_MGMT_ACTIVITY_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string PropMgmtActivityStatusTypeCode { get; set; }
        [Column("MINISTRY_CONTACT_ID")]
        public long? MinistryContactId { get; set; }
        [Column("VENDOR_ID")]
        public long? VendorId { get; set; }
        [Column("REQUEST_RECEIVED_DT", TypeName = "date")]
        public DateTime? RequestReceivedDt { get; set; }
        [Column("COMPLETION_DT", TypeName = "date")]
        public DateTime? CompletionDt { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("REQUEST_SOURCE")]
        [StringLength(2000)]
        public string RequestSource { get; set; }
        [Column("INVOLVED_PARTY")]
        [StringLength(2000)]
        public string InvolvedParty { get; set; }
        [Column("PRETAX_AMT", TypeName = "money")]
        public decimal? PretaxAmt { get; set; }
        [Column("GST_AMT", TypeName = "money")]
        public decimal? GstAmt { get; set; }
        [Column("PST_AMT", TypeName = "money")]
        public decimal? PstAmt { get; set; }
        [Column("TOTAL_AMT", TypeName = "money")]
        public decimal? TotalAmt { get; set; }
        [Column("IS_DISABLED")]
        public bool IsDisabled { get; set; }
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
}
