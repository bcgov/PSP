using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_SECURITY_DEPOSIT_RETURN_HIST")]
    [Index(nameof(SecurityDepositReturnHistId), nameof(EndDateHist), Name = "PIMS_SDRTRN_H_UK", IsUnique = true)]
    public partial class PimsSecurityDepositReturnHist
    {
        [Key]
        [Column("_SECURITY_DEPOSIT_RETURN_HIST_ID")]
        public long SecurityDepositReturnHistId { get; set; }
        [Column("EFFECTIVE_DATE_HIST", TypeName = "datetime")]
        public DateTime EffectiveDateHist { get; set; }
        [Column("END_DATE_HIST", TypeName = "datetime")]
        public DateTime? EndDateHist { get; set; }
        [Column("SECURITY_DEPOSIT_RETURN_ID")]
        public long SecurityDepositReturnId { get; set; }
        [Column("LEASE_ID")]
        public long LeaseId { get; set; }
        [Required]
        [Column("SECURITY_DEPOSIT_TYPE_CODE")]
        [StringLength(20)]
        public string SecurityDepositTypeCode { get; set; }
        [Column("TERMINATION_DATE", TypeName = "datetime")]
        public DateTime TerminationDate { get; set; }
        [Column("CLAIMS_AGAINST", TypeName = "money")]
        public decimal? ClaimsAgainst { get; set; }
        [Column("RETURN_AMOUNT", TypeName = "money")]
        public decimal ReturnAmount { get; set; }
        [Column("RETURN_DATE", TypeName = "datetime")]
        public DateTime ReturnDate { get; set; }
        [Required]
        [Column("PAYEE_NAME")]
        [StringLength(100)]
        public string PayeeName { get; set; }
        [Column("PAYEE_ADDRESS")]
        [StringLength(500)]
        public string PayeeAddress { get; set; }
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
        [Column("SECURITY_DEPOSIT_ID")]
        public long? SecurityDepositId { get; set; }
    }
}
