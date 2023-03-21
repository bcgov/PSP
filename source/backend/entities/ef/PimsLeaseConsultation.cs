using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_CONSULTATION")]
    [Index(nameof(ConsultationStatusTypeCode), Name = "LESCON_CONSULTATION_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(ConsultationTypeCode), Name = "LESCON_CONSULTATION_TYPE_CODE_IDX")]
    [Index(nameof(ConsultationTypeCode), nameof(LeaseId), Name = "LESCON_LEASE_CONSULTATION_TUC", IsUnique = true)]
    [Index(nameof(LeaseId), Name = "LESCON_LEASE_ID_IDX")]
    public partial class PimsLeaseConsultation
    {
        [Key]
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
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }

        [ForeignKey(nameof(ConsultationStatusTypeCode))]
        [InverseProperty(nameof(PimsConsultationStatusType.PimsLeaseConsultations))]
        public virtual PimsConsultationStatusType ConsultationStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(ConsultationTypeCode))]
        [InverseProperty(nameof(PimsConsultationType.PimsLeaseConsultations))]
        public virtual PimsConsultationType ConsultationTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(LeaseId))]
        [InverseProperty(nameof(PimsLease.PimsLeaseConsultations))]
        public virtual PimsLease Lease { get; set; }
    }
}
