using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_USER")]
    [Index(nameof(BusinessIdentifierValue), Name = "USER_BUSINESS_IDENTIFIER_VALUE_IDX")]
    [Index(nameof(GuidIdentifierValue), Name = "USER_GUID_IDENTIFIER_VALUE_IDX")]
    [Index(nameof(PersonId), Name = "USER_PERSON_ID_IDX")]
    [Index(nameof(UserTypeCode), Name = "USER_USER_TYPE_CODE_IDX")]
    public partial class PimsUser
    {
        public PimsUser()
        {
            PimsAccessRequests = new HashSet<PimsAccessRequest>();
            PimsRegionUsers = new HashSet<PimsRegionUser>();
            PimsUserOrganizations = new HashSet<PimsUserOrganization>();
            PimsUserRoles = new HashSet<PimsUserRole>();
        }

        [Key]
        [Column("USER_ID")]
        public long UserId { get; set; }
        [Column("PERSON_ID")]
        public long PersonId { get; set; }
        [Column("USER_TYPE_CODE")]
        [StringLength(20)]
        public string UserTypeCode { get; set; }
        [Required]
        [Column("BUSINESS_IDENTIFIER_VALUE")]
        [StringLength(30)]
        public string BusinessIdentifierValue { get; set; }
        [Column("GUID_IDENTIFIER_VALUE")]
        public Guid? GuidIdentifierValue { get; set; }
        [Column("POSITION")]
        [StringLength(100)]
        public string Position { get; set; }
        [Column("NOTE")]
        [StringLength(1000)]
        public string Note { get; set; }
        [Column("LAST_LOGIN", TypeName = "datetime")]
        public DateTime? LastLogin { get; set; }
        [Column("APPROVED_BY_ID")]
        [StringLength(30)]
        public string ApprovedById { get; set; }
        [Column("ISSUE_DATE", TypeName = "datetime")]
        public DateTime IssueDate { get; set; }
        [Column("EXPIRY_DATE", TypeName = "datetime")]
        public DateTime? ExpiryDate { get; set; }
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

        [ForeignKey(nameof(PersonId))]
        [InverseProperty(nameof(PimsPerson.PimsUsers))]
        public virtual PimsPerson Person { get; set; }
        [ForeignKey(nameof(UserTypeCode))]
        [InverseProperty(nameof(PimsUserType.PimsUsers))]
        public virtual PimsUserType UserTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsAccessRequest.User))]
        public virtual ICollection<PimsAccessRequest> PimsAccessRequests { get; set; }
        [InverseProperty(nameof(PimsRegionUser.User))]
        public virtual ICollection<PimsRegionUser> PimsRegionUsers { get; set; }
        [InverseProperty(nameof(PimsUserOrganization.User))]
        public virtual ICollection<PimsUserOrganization> PimsUserOrganizations { get; set; }
        [InverseProperty(nameof(PimsUserRole.User))]
        public virtual ICollection<PimsUserRole> PimsUserRoles { get; set; }
    }
}
