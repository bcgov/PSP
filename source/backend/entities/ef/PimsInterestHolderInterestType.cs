using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Tables that contains the codes and associated descriptions of the interest holder interest types.
/// </summary>
[Table("PIMS_INTEREST_HOLDER_INTEREST_TYPE")]
public partial class PimsInterestHolderInterestType
{
    /// <summary>
    /// Codified version of the interest holder interest type.
    /// </summary>
    [Key]
    [Column("INTEREST_HOLDER_INTEREST_TYPE_CODE")]
    [StringLength(20)]
    public string InterestHolderInterestTypeCode { get; set; }

    /// <summary>
    /// Description of the interest holder interest type.
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
    public bool? IsDisabled { get; set; }

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

    [InverseProperty("InterestHolderInterestTypeCodeNavigation")]
    public virtual ICollection<PimsPropInthldrInterestTyp> PimsPropInthldrInterestTyps { get; set; } = new List<PimsPropInthldrInterestTyp>();
}
