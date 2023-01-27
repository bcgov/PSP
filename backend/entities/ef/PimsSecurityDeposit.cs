using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_SECURITY_DEPOSIT")]
    [Index(nameof(LeaseId), Name = "SECDEP_LEASE_ID_IDX")]
    [Index(nameof(SecurityDepositTypeCode), Name = "SECDEP_SECURITY_DEPOSIT_TYPE_CODE_IDX")]
    public partial class PimsSecurityDeposit
    {
        public PimsSecurityDeposit()
        {
            PimsSecurityDepositReturns = new HashSet<PimsSecurityDepositReturn>();
        }

        [Key]
        [Column("SECURITY_DEPOSIT_ID")]
        public long SecurityDepositId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Required]
        [Column("SECURITY_DEPOSIT_TYPE_CODE")]
        [StringLength(20)]
        public string SecurityDepositTypeCode { get; set; }
        [Column("OTHER_DEPOSIT_TYPE_DESC")]
        [StringLength(200)]
        public string OtherDepositTypeDesc { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(2000)]
        public string Description { get; set; }
        [Column("AMOUNT_PAID", TypeName = "money")]
        public decimal AmountPaid { get; set; }
        [Column("DEPOSIT_DATE", TypeName = "date")]
        public DateTime DepositDate { get; set; }
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

        [ForeignKey(nameof(LeaseId))]
        [InverseProperty(nameof(PimsLease.PimsSecurityDeposits))]
        public virtual PimsLease Lease { get; set; }
        [ForeignKey(nameof(SecurityDepositTypeCode))]
        [InverseProperty(nameof(PimsSecurityDepositType.PimsSecurityDeposits))]
        public virtual PimsSecurityDepositType SecurityDepositTypeCodeNavigation { get; set; }
        [InverseProperty("SecurityDeposit")]
        public virtual PimsSecurityDepositHolder PimsSecurityDepositHolder { get; set; }
        [InverseProperty(nameof(PimsSecurityDepositReturn.SecurityDeposit))]
        public virtual ICollection<PimsSecurityDepositReturn> PimsSecurityDepositReturns { get; set; }
    }
}
