using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace Pims.Dal.Entities
{
    [Table("PIMS_ACCESS_REQUEST")]
    [Index(nameof(AccessRequestStatusTypeCode), Name = "ACRQST_ACCESS_REQUEST_STATUS_TYPE_CODE_IDX")]
    [Index(nameof(RegionCode), Name = "ACRQST_REGION_CODE_IDX")]
    [Index(nameof(RoleId), Name = "ACRQST_ROLE_ID_IDX")]
    [Index(nameof(UserId), Name = "ACRQST_USER_ID_IDX")]
    [Index(nameof(RegionCode), nameof(RoleId), nameof(UserId), Name = "ACRQST_USER_ROLE_REGION_TUC", IsUnique = true)]
    [Index(nameof(UserTypeCode), Name = "ACRQST_USER_TYPE_CODE_IDX")]
    public partial class PimsAccessRequest
    {
        public PimsAccessRequest()
        {
            PimsAccessRequestOrganizations = new HashSet<PimsAccessRequestOrganization>();
        }

        [Key]
        [Column("ACCESS_REQUEST_ID")]
        public long AccessRequestId { get; set; }
        [Column("USER_ID")]
        public long UserId { get; set; }
        [Column("ROLE_ID")]
        public long? RoleId { get; set; }
        [Column("USER_TYPE_CODE")]
        [StringLength(20)]
        public string UserTypeCode { get; set; }
        [Required]
        [Column("ACCESS_REQUEST_STATUS_TYPE_CODE")]
        [StringLength(20)]
        public string AccessRequestStatusTypeCode { get; set; }
        [Column("REGION_CODE")]
        public short RegionCode { get; set; }
        [Column("NOTE")]
        public string Note { get; set; }
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

        [ForeignKey(nameof(AccessRequestStatusTypeCode))]
        [InverseProperty(nameof(PimsAccessRequestStatusType.PimsAccessRequests))]
        public virtual PimsAccessRequestStatusType AccessRequestStatusTypeCodeNavigation { get; set; }
        [ForeignKey(nameof(RegionCode))]
        [InverseProperty(nameof(PimsRegion.PimsAccessRequests))]
        public virtual PimsRegion RegionCodeNavigation { get; set; }
        [ForeignKey(nameof(RoleId))]
        [InverseProperty(nameof(PimsRole.PimsAccessRequests))]
        public virtual PimsRole Role { get; set; }
        [ForeignKey(nameof(UserId))]
        [InverseProperty(nameof(PimsUser.PimsAccessRequests))]
        public virtual PimsUser User { get; set; }
        [ForeignKey(nameof(UserTypeCode))]
        [InverseProperty(nameof(PimsUserType.PimsAccessRequests))]
        public virtual PimsUserType UserTypeCodeNavigation { get; set; }
        [InverseProperty(nameof(PimsAccessRequestOrganization.AccessRequest))]
        public virtual ICollection<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; }
    }
}
