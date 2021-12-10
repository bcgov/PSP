﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_SECURITY_DEPOSIT_TYPE")]
    public partial class PimsSecurityDepositType
    {
        public PimsSecurityDepositType()
        {
            PimsSecurityDepositReturns = new HashSet<PimsSecurityDepositReturn>();
            PimsSecurityDeposits = new HashSet<PimsSecurityDeposit>();
        }

        [Key]
        [Column("SECURITY_DEPOSIT_TYPE_CODE")]
        [StringLength(20)]
        public string SecurityDepositTypeCode { get; set; }
        [Required]
        [Column("DESCRIPTION")]
        [StringLength(200)]
        public string Description { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        [Column("DISPLAY_ORDER")]
        public int? DisplayOrder { get; set; }
        [Column("CONCURRENCY_CONTROL_NUMBER")]
        public long ConcurrencyControlNumber { get; set; }
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

        [InverseProperty(nameof(PimsSecurityDepositReturn.SecurityDepositTypeCodeNavigation))]
        public virtual ICollection<PimsSecurityDepositReturn> PimsSecurityDepositReturns { get; set; }
        [InverseProperty(nameof(PimsSecurityDeposit.SecurityDepositTypeCodeNavigation))]
        public virtual ICollection<PimsSecurityDeposit> PimsSecurityDeposits { get; set; }
    }
}
