using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_CONSULTATION_HIST")]
    [Index(nameof(LeaseConsultationHistId), nameof(EndDateHist), Name = "PIMS_LESCON_H_UK", IsUnique = true)]
    public partial class PimsLeaseConsultationHist
    {
        [Key]
        [Column("_LEASE_CONSULTATION_HIST_ID")]
        public long LeaseConsultationHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("LEASE_CONSULTATION_ID")]
        public long LeaseConsultationId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Required]
        [Column("CONSULTATION_TYPE_CODE")]
        [StringLength(20)]
        public string ConsultationTypeCode { get; set; }
        [Required]
        [Column("CONSULTATION_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string ConsultationStatusTypeCode { get; set; }
        [Column("OTHER_DESCRIPTION")]
        [StringLength(2000)]
        public string OtherDescription { get; set; }
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppCreateTimestamp { get; set; }
        [Required]
        [Column("APP_CREATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppCreateUserDirectory { get; set; }
        [Column("APP_CREATE_USER_GUID")]
        public Guid? AppCreateUserGuid { get; set; }
        [Required]
        [Column("APP_CREATE_USERID")]
        [StringLength(30)]
        public string AppCreateUserid { get; set; }
        [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
        public DateTime AppLastUpdateTimestamp { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
        [StringLength(30)]
        public string AppLastUpdateUserDirectory { get; set; }
        [Column("APP_LAST_UPDATE_USER_GUID")]
        public Guid? AppLastUpdateUserGuid { get; set; }
        [Required]
        [Column("APP_LAST_UPDATE_USERID")]
        [StringLength(30)]
        public string AppLastUpdateUserid { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
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
}
