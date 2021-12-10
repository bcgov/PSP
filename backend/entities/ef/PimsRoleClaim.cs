using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ROLE_CLAIM")]
    [Index(nameof(ClaimId), Name = "ROLCLM_CLAIM_ID_IDX")]
    [Index(nameof(RoleId), nameof(ClaimId), Name = "ROLCLM_ROLE_CLAIM_TUC", IsUnique = true)]
    [Index(nameof(RoleId), Name = "ROLCLM_ROLE_ID_IDX")]
    public partial class PimsRoleClaim
    {
        [Key]
        [Column("ROLE_CLAIM_ID")]
        public long RoleClaimId { get; set; }
        [Column("ROLE_ID")]
        public long RoleId { get; set; }
        [Column("CLAIM_ID")]
        public long ClaimId { get; set; }
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

        [ForeignKey(nameof(ClaimId))]
        [InverseProperty(nameof(PimsClaim.PimsRoleClaims))]
        public virtual PimsClaim Claim { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(PimsRole.PimsRoleClaims))]
        public virtual PimsRole Role { get; set; }
    }
}
