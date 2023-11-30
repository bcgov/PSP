using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Defines the contacts that are associated with this property.
/// </summary>
[Table("PIMS_PROP_PROP_PURPOSE")]
[Index("PropertyId", Name = "PRPRPU_PROPERTY_ID_IDX")]
[Index("PropertyPurposeTypeCode", Name = "PRPRPU_PROPERTY_PURPOSE_TYPE_CODE_IDX")]
public partial class PimsPropPropPurpose
{
    [Key]
    [Column("PROP_PROP_PURPOSE_ID")]
    public long PropPropPurposeId { get; set; }

    /// <summary>
    /// Primary key of the associated property.
    /// </summary>
    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    /// <summary>
    /// Primary key of the associated property purpose.
    /// </summary>
    [Required]
    [Column("PROPERTY_PURPOSE_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyPurposeTypeCode { get; set; }

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

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropPropPurposes")]
    public virtual PimsProperty Property { get; set; }

    [ForeignKey("PropertyPurposeTypeCode")]
    [InverseProperty("PimsPropPropPurposes")]
    public virtual PimsPropertyPurposeType PropertyPurposeTypeCodeNavigation { get; set; }
}
