using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Resolves many-to-many relationship between PIMS_PROPERTY and PIMS_PROPERTY_ROAD_TYPE
/// </summary>
[Table("PIMS_PROP_PROP_ROAD_TYPE")]
[Index("PropertyId", Name = "PRPRRT_PROPERTY_ID_IDX")]
[Index("PropertyRoadTypeCode", Name = "PRPRRT_PROPERTY_ROAD_TYPE_CODE_IDX")]
[Index("PropertyRoadTypeCode", "PropertyId", Name = "PRPRRT_PROP_ROAD_TYPE_TUC", IsUnique = true)]
public partial class PimsPropPropRoadType
{
    [Key]
    [Column("PROP_PROP_ROAD_TYPE_ID")]
    public long PropPropRoadTypeId { get; set; }

    [Column("PROPERTY_ID")]
    public long PropertyId { get; set; }

    [Required]
    [Column("PROPERTY_ROAD_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyRoadTypeCode { get; set; }

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

    [ForeignKey("PropertyId")]
    [InverseProperty("PimsPropPropRoadTypes")]
    public virtual PimsProperty Property { get; set; }

    [ForeignKey("PropertyRoadTypeCode")]
    [InverseProperty("PimsPropPropRoadTypes")]
    public virtual PimsPropertyRoadType PropertyRoadTypeCodeNavigation { get; set; }
}
