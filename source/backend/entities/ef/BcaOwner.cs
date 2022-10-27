using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("BCA_OWNER")]
    [Index(nameof(OwnershipGroupId), Name = "BCAOWN_OWNERSHIP_GROUP_ID_IDX")]
    public partial class BcaOwner
    {
        [Key]
        [Column("OWNER_ID")]
        public long OwnerId { get; set; }
        [Required]
        [Column("OWNERSHIP_GROUP_ID")]
        [StringLength(32)]
        public string OwnershipGroupId { get; set; }
        [Column("FIRST_NAME")]
        [StringLength(255)]
        public string FirstName { get; set; }
        [Column("MIDDLE_NAME")]
        [StringLength(255)]
        public string MiddleName { get; set; }
        [Column("MIDDLE_INITIAL")]
        [StringLength(1)]
        public string MiddleInitial { get; set; }
        [Column("COMPANY_OR_LAST_NAME")]
        [StringLength(255)]
        public string CompanyOrLastName { get; set; }
        [Column("OWNER_SEQUENCE_ID")]
        [StringLength(32)]
        public string OwnerSequenceId { get; set; }
        [Column("EQUITY_TYPE")]
        [StringLength(16)]
        public string EquityType { get; set; }
        [Column("EQUITY_TYPE_DESCRIPTION")]
        [StringLength(255)]
        public string EquityTypeDescription { get; set; }
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

        [ForeignKey(nameof(OwnershipGroupId))]
        [InverseProperty(nameof(BcaOwnershipGroup.BcaOwners))]
        public virtual BcaOwnershipGroup OwnershipGroup { get; set; }
    }
}
