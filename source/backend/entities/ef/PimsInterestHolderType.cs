using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Tables that contains the codes and associated descriptions of the interest holder types, such as solicitors, representatives, and interest holders.
/// </summary>
[Table("PIMS_INTEREST_HOLDER_TYPE")]
public partial class PimsInterestHolderType
{
    /// <summary>
    /// Codified version of the interest holder types, such as solicitors, representatives, and interest holders.
    /// </summary>
    [Key]
    [Column("INTEREST_HOLDER_TYPE_CODE")]
    [StringLength(20)]
    public string InterestHolderTypeCode { get; set; }

    /// <summary>
    /// Description of the interest holder types, such as solicitors, representatives, and interest holders.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Display order of the codes.
    /// </summary>
    [Column("DISPLAY_ORDER")]
    public int? DisplayOrder { get; set; }

    /// <summary>
    /// Indicates if the code value is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    [Column("CONCURRENCY_CONTROL_NUMBER")]
    public long? ConcurrencyControlNumber { get; set; }

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

    [InverseProperty("InterestHolderTypeCodeNavigation")]
    public virtual ICollection<PimsInterestHolder> PimsInterestHolders { get; set; } = new List<PimsInterestHolder>();
}
