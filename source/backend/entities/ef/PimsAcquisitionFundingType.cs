using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Pims.Dal.Entities;

/// <summary>
/// Codified values for the acquistion funding type.
/// </summary>
[Table("PIMS_ACQUISITION_FUNDING_TYPE")]
public partial class PimsAcquisitionFundingType
{
    /// <summary>
    /// Code value for the acquistion funding type.
    /// </summary>
    [Key]
    [Column("ACQUISITION_FUNDING_TYPE_CODE")]
    [StringLength(20)]
    public string AcquisitionFundingTypeCode { get; set; }

    /// <summary>
    /// Description of the acquistion funding type.
    /// </summary>
    [Required]
    [Column("DESCRIPTION")]
    [StringLength(200)]
    public string Description { get; set; }

    /// <summary>
    /// Indicates if the code value is inactive.
    /// </summary>
    [Column("IS_DISABLED")]
    public bool IsDisabled { get; set; }

    /// <summary>
    /// Designates a preferred presentation order of the code descriptions.
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

    [InverseProperty("AcquisitionFundingTypeCodeNavigation")]
    public virtual ICollection<PimsAcquisitionFile> PimsAcquisitionFiles { get; set; } = new List<PimsAcquisitionFile>();

    [InverseProperty("AcquisitionFundingTypeCodeNavigation")]
    public virtual ICollection<PimsManagementFile> PimsManagementFiles { get; set; } = new List<PimsManagementFile>();
}
