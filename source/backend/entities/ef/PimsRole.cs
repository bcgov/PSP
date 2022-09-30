using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ROLE")]
    [Index(nameof(KeycloakGroupId), Name = "ROLE_KEYCLOAK_GROUP_ID_IDX")]
    [Index(nameof(RoleUid), Name = "ROLE_ROLE_UID_IDX")]
    public partial class PimsRole
    {
        public PimsRole()
        {
            PimsAccessRequests = new HashSet<PimsAccessRequest>();
            PimsRoleClaims = new HashSet<PimsRoleClaim>();
            PimsUserOrganizations = new HashSet<PimsUserOrganization>();
            PimsUserRoles = new HashSet<PimsUserRole>();
        }

        [Key]
        [Column("ROLE_ID")]
        public long RoleId { get; set; }
        [Column("ROLE_UID")]
        public Guid RoleUid { get; set; }
        [Column("KEYCLOAK_GROUP_ID")]
        public Guid? KeycloakGroupId { get; set; }
        [Required]
        [Column("NAME")]
        [StringLength(100)]
        public string Name { get; set; }
        [Column("DESCRIPTION")]
        [StringLength(500)]
        public string Description { get; set; }
        [Required]
        [Column("IS_PUBLIC")]
        public bool? IsPublic { get; set; }
        [Required]
        [Column("IS_DISABLED")]
        public bool? IsDisabled { get; set; }
        [Column("SORT_ORDER")]
        public int SortOrder { get; set; }
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

        [InverseProperty(nameof(PimsAccessRequest.Role))]
        public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; }
        [InverseProperty(nameof(PimsRoleClaim.Role))]
        public virtual ICollection<PimsRoleClaim> PimsRoleClaims { get; set; }
        [InverseProperty(nameof(PimsUserOrganization.Role))]
        public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; }
        [InverseProperty(nameof(PimsUserRole.Role))]
        public virtual ICollection<PimsUserRole> PimsUserRoles { get; set; }
    }
}
