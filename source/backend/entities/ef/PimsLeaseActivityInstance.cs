using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_LEASE_ACTIVITY_INSTANCE")]
    [Index(nameof(ActivityInstanceId), Name = "LSACIN_ACTIVITY_INSTANCE_ID_IDX")]
    [Index(nameof(LeaseId), nameof(ActivityInstanceId), Name = "LSACIN_LEASE_ACTIVITY_INSTANCE_TUC", IsUnique = true)]
    [Index(nameof(LeaseId), Name = "LSACIN_LEASE_ID_IDX")]
    public partial class PimsLeaseActivityInstance
    {
        [Key]
        [Column("LEASE_ACTIVITY_INSTANCE_ID")]
        [StringLength(40)]
        public string LeaseActivityInstanceId { get; set; }
        [Column("ACTIVITY_INSTANCE_ID")]
        public long ActivityInstanceId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
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

        [ForeignKey(nameof(ActivityInstanceId))]
        [InverseProperty(nameof(PimsActivityInstance.PimsLeaseActivityInstances))]
        public virtual PimsActivityInstance ActivityInstance { get; set; }
        [ForeignKey(nameof(LeaseId))]
        [InverseProperty(nameof(PimsLease.PimsLeaseActivityInstances))]
        public virtual PimsLease Lease { get; set; }
    }
}
