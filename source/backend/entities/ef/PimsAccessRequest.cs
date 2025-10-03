using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

[Table("PIMS_ACCESS_REQUEST")]
[Index("AccessRequestStatusTypeCode", Name = "ACRQST_ACCESS_REQUEST_STATUS_TYPE_CODE_IDX")]
[Index("RegionCode", Name = "ACRQST_REGION_CODE_IDX")]
[Index("RoleId", Name = "ACRQST_ROLE_ID_IDX")]
[Index("UserId", Name = "ACRQST_USER_ID_IDX")]
[Index("RegionCode", "RoleId", "UserId", Name = "ACRQST_USER_ROLE_REGION_TUC", IsUnique = true)]
[Index("UserTypeCode", Name = "ACRQST_USER_TYPE_CODE_IDX")]
public partial class PimsAccessRequest
{
    /// <summary>
    /// System-generated unique surrogate primary key.
    /// </summary>
    [Key]
    [Column("ACCESS_REQUEST_ID")]
    public long AccessRequestId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_USER table.
    /// </summary>
    [Column("USER_ID")]
    public long UserId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ROLE table.
    /// </summary>
    [Column("ROLE_ID")]
    public long? RoleId { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_USER_TYPE table.
    /// </summary>
    [Column("USER_TYPE_CODE")]
    [StringLength(20)]
    public string UserTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_ACCESS_REQUEST_STATUS_TYPE table.
    /// </summary>
    [Required]
    [Column("ACCESS_REQUEST_STATUS_TYPE_CODE")]
    [StringLength(20)]
    public string AccessRequestStatusTypeCode { get; set; }

    /// <summary>
    /// Foreign key to the PIMS_REGION table.
    /// </summary>
    [Column("REGION_CODE")]
    public short RegionCode { get; set; }

    /// <summary>
    /// Note associated with this access request.
    /// </summary>
    [Column("NOTE")]
    [StringLength(1000)]
    public string Note { get; set; }

    /// <summary>
    /// Application code is responsible for retrieving the row and then incrementing the value of the CONCURRENCY_CONTROL_NUMBER column by one prior to issuing an update. If this is done then the update will succeed, provided that the row was not updated by any o
    /// </summary>
    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

    /// <summary>
    /// The date and time the user created the record.
    /// </summary>
    [Column("APP_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppCreateTimestamp { get; set; }

    /// <summary>
    /// The user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USERID")]
    [StringLength(30)]
    public string AppCreateUserid { get; set; }

    /// <summary>
    /// GUID of the user that created the record.
    /// </summary>
    [Column("APP_CREATE_USER_GUID")]
    public Guid? AppCreateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that created the record.
    /// </summary>
    [Required]
    [Column("APP_CREATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppCreateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was updated by the user.
    /// </summary>
    [Column("APP_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime AppLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string AppLastUpdateUserid { get; set; }

    /// <summary>
    /// GUID of the user that updated the record.
    /// </summary>
    [Column("APP_LAST_UPDATE_USER_GUID")]
    public Guid? AppLastUpdateUserGuid { get; set; }

    /// <summary>
    /// User directory of the user that updated the record.
    /// </summary>
    [Required]
    [Column("APP_LAST_UPDATE_USER_DIRECTORY")]
    [StringLength(30)]
    public string AppLastUpdateUserDirectory { get; set; }

    /// <summary>
    /// The date and time the record was created.
    /// </summary>
    [Column("DB_CREATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbCreateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created the record.
    /// </summary>
    [Required]
    [Column("DB_CREATE_USERID")]
    [StringLength(30)]
    public string DbCreateUserid { get; set; }

    /// <summary>
    /// The date and time the record was created or last updated.
    /// </summary>
    [Column("DB_LAST_UPDATE_TIMESTAMP", TypeName = "datetime")]
    public DateTime DbLastUpdateTimestamp { get; set; }

    /// <summary>
    /// The user or proxy account that created or last updated the record.
    /// </summary>
    [Required]
    [Column("DB_LAST_UPDATE_USERID")]
    [StringLength(30)]
    public string DbLastUpdateUserid { get; set; }

    [ForeignKey("AccessRequestStatusTypeCode")]
    [InverseProperty("PimsAccessRequests")]
    public virtual PimsAccessRequestStatusType AccessRequestStatusTypeCodeNavigation { get; set; }

    [InverseProperty("AccessRequest")]
    public virtual ICollection<PimsAccessRequestOrganization> PimsAccessRequestOrganizations { get; set; } = new List<PimsAccessRequestOrganization>();

    [ForeignKey("RegionCode")]
    [InverseProperty("PimsAccessRequests")]
    public virtual PimsRegion RegionCodeNavigation { get; set; }

    [ForeignKey("RoleId")]
    [InverseProperty("PimsAccessRequests")]
    public virtual PimsRole Role { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PimsAccessRequests")]
    public virtual PimsUser User { get; set; }

    [ForeignKey("UserTypeCode")]
    [InverseProperty("PimsAccessRequests")]
    public virtual PimsUserType UserTypeCodeNavigation { get; set; }
}
