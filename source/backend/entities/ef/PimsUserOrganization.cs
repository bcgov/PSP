using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_USER_ORGANIZATION")]
    [Index(nameof(OrganizationId), Name = "USRORG_ORGANIZATION_ID_IDX")]
    [Index(nameof(RoleId), Name = "USRORG_ROLE_ID_IDX")]
    [Index(nameof(UserId), Name = "USRORG_USER_ID_IDX")]
    [Index(nameof(OrganizationId), nameof(UserId), nameof(RoleId), Name = "USRORG_USER_ROLE_ORGANIZATION_TUC", IsUnique = true)]
    public partial class PimsUserOrganization
    {
        [Key]
        [Column("USER_ORGANIZATION_ID")]
        public long UserOrganizationId { get; set; }
        [Column("ORGANIZATION_ID")]
        public long OrganizationId { get; set; }
        [Column("USER_ID")]
        public long UserId { get; set; }
        [Column("ROLE_ID")]
        public long RoleId { get; set; }
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

        [ForeignKey(nameof(OrganizationId))]
        [InverseProperty(nameof(PimsOrganization.PimsUserOrganizations))]
        public virtual PimsOrganization Organization { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(PimsRole.PimsUserOrganizations))]
        public virtual PimsRole Role { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(PimsUser.PimsUserOrganizations))]
        public virtual PimsUser User { get; set; }
    }
}
