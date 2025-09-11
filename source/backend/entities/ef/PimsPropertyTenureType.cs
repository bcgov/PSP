using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// A code table to store property tenure codes. Tenure is defined as : &quot;The act, right, manner or term of holding something(as a landed property)&quot; In this case, tenure is required on Properties to indicate MoTT&apos;s legal tenure on the property. The land parcel
/// </summary>
[Table("PIMS_PROPERTY_TENURE_TYPE")]
public partial class PimsPropertyTenureType
{
    [Key]
    [Column("PROPERTY_TENURE_TYPE_CODE")]
    [StringLength(20)]
    public string PropertyTenureTypeCode { get; set; }

    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("PropertyTenureTypeCodeNavigation")]
    public virtual ICollection<PimsPropPropTenureTyp> PimsPropPropTenureTyps { get; set; } = new List<PimsPropPropTenureTyp>();
}
