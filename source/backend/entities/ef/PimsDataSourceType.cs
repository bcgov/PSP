using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Describes the source system of the data (PAIMS, LIS, etc.)
/// </summary>
[Table("PIMS_DATA_SOURCE_TYPE")]
public partial class PimsDataSourceType
{
    /// <summary>
    /// Code value of the source system of the data (PAIMS, LIS, etc.)
    /// </summary>
    [Key]
    [Column("DATA_SOURCE_TYPE_CODE")]
    [StringLength(20)]
    public string DataSourceTypeCode { get; set; }

    /// <summary>
    /// Description of the source system of the data (PAIMS, LIS, etc.)
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code is still in use.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Defines the default display order of the descriptions.
    /// </summary>
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

    [InverseProperty("DataSourceTypeCodeNavigation")]
    public virtual ICollection<PimsDocumentQueue> PimsDocumentQueues { get; set; } = new List<PimsDocumentQueue>();

    [InverseProperty("DataSourceTypeCodeNavigation")]
    public virtual ICollection<PimsHistoricalFileNumber> PimsHistoricalFileNumbers { get; set; } = new List<PimsHistoricalFileNumber>();

    [InverseProperty("PropertyDataSourceTypeCodeNavigation")]
    public virtual ICollection<PimsProperty> PimsProperties { get; set; } = new List<PimsProperty>();
}
