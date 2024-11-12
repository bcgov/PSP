using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// An associative entity to define multiple addresses for a person.
/// </summary>
[Table("PIMS_ORGANIZATION_ADDRESS")]
[Index("AddressId", Name = "ORGADD_ADDRESS_ID_IDX")]
[Index("AddressUsageTypeCode", Name = "ORGADD_ADDRESS_USAGE_TYPE_CODE_IDX")]
[Index("OrganizationId", Name = "ORGADD_ORGANIZATION_ID_IDX")]
[Index("OrganizationId", "AddressId", "AddressUsageTypeCode", Name = "ORGADD_UNQ_ADDR_TYPE_TUC", IsUnique = true)]
public partial class PimsOrganizationAddress
{
    [Key]
    [Column("ORGANIZATION_ADDRESS_ID")]
    public long OrganizationAddressId { get; set; }

    [Column("ORGANIZATION_ID")]
    public long OrganizationId { get; set; }

    [Column("ADDRESS_ID")]
    public long AddressId { get; set; }

    [Required]
    [Column("ADDRESS_USAGE_TYPE_CODE")]
    [StringLength(20)]
    public string AddressUsageTypeCode { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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

    [ForeignKey("AddressId")]
    [InverseProperty("PimsOrganizationAddresses")]
    public virtual PimsAddress Address { get; set; }

    [ForeignKey("AddressUsageTypeCode")]
    [InverseProperty("PimsOrganizationAddresses")]
    public virtual PimsAddressUsageType AddressUsageTypeCodeNavigation { get; set; }

    [ForeignKey("OrganizationId")]
    [InverseProperty("PimsOrganizationAddresses")]
    public virtual PimsOrganization Organization { get; set; }
}
